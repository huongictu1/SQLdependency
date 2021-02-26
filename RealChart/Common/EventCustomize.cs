using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.Style;
using Welfare.Common;
using Welfare.Data;
using Welfare.Logs;
using Welfare.Models;

namespace Welfare.Controllers
{
    public class EventCustomizeController : BaseController
    {
        private readonly WelfareContext _context;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        
        [Obsolete]
        public EventCustomizeController(WelfareContext context, IHostingEnvironment _hosting)
        {
            _context = context;
            _hostingEnvironment = _hosting;
        }
        public IActionResult ActiveEvent()
        {
            var models = _context.EVENT_SETTING.Where(a => a.EVENT_ACTIVE == true && a.EVENT_TYPE == 6 && a.EVENT_FACTORY.Equals(Factory)).OrderByDescending(x => x.EVENT_DATE);
            if (permission.Equals("supplier"))
            {
                models = models.Where(x => x.ALLOW_SUPPLIER == true).OrderByDescending(x => x.EVENT_DATE);
            }
            ViewBag.isAdmin = permission.Equals("admin");
            ViewBag.Factory = Factory;
            return View(models);
        }

        [Obsolete]
        public IActionResult DisableEvent()
        {
            int deactiveEvent = new WelfareADO().ExecuteNon2("update EVENT_SETTING set EVENT_ACTIVE = '0' where EVENT_CODE = '" + event_code + "'");
            if(deactiveEvent == 1)
            {
                new LogFunc(_hostingEnvironment).writeToLog(HttpContext.Session.GetString("username"), "Disable sự kiện " + event_code);
                return Redirect("/logout");
            }
            return View(nameof(Index));
        }
        [Obsolete]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActiveEvent(IFormCollection iCollection)
        {
            var models = _context.EVENT_SETTING.Where(a => a.EVENT_ACTIVE == true && a.EVENT_TYPE == 6 && a.EVENT_FACTORY.Equals(Factory)).OrderByDescending(x => x.EVENT_DATE);
            if (permission.Equals("supplier"))
            {
                models = models.Where(x => x.ALLOW_SUPPLIER == true).OrderByDescending(x => x.EVENT_DATE);
            }

            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("event_code", iCollection["EVENT_SELECTED"]);
                event_code = HttpContext.Session.GetString("event_code");
                if (!Factory.Equals(event_code.Substring(2,2)))
                {
                    TempData["NotFactory"] = "Vui lòng chọn sự kiện nhà máy của bạn!";
                    ViewBag.NotFactory = "Vui lòng chọn sự kiện nhà máy của bạn!";
                    return View(models);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(models);
        }
        public IActionResult Index()
        {
            
            
            if (event_code.Equals("") || event_code == null)
            {
                return RedirectToAction(nameof(ActiveEvent));
            }
            EVENT_SETTING ev = _context.EVENT_SETTING.Where(a => a.EVENT_CODE.Equals(event_code)).FirstOrDefault();
            ev.EVENT_BACKGROUND = ev.EVENT_BACKGROUND.Split("\\").Last();
            HttpContext.Session.SetString("event_standard", ev.EVENT_STANDARD);
            if(ev == null)
            {
                return RedirectToAction(nameof(ActiveEvent));
            }
            var temp = TempData["NotFactory"];
            if (temp != null)
            {
                ViewBag.NotFactory = temp;
            }
            List<LAST_CHECK_IN> ls1 = GetLastCheckin(3);
            ViewBag.LastCheck = ls1;//list last check in (3 record)
            ViewBag.CardNewest = ls1.Count > 0 ? ls1[0].EMP_CARD : "";
            List<StandardRemain> lstStandard = GetListStandardRemain();
            ViewBag.ListStandard = lstStandard;
            return View(ev);
        }
        public IActionResult AdditionalList()
        {
            
            
            if (!permission.Equals("admin"))
            {
                return RedirectToAction(nameof(Index));
            }
            if (event_code.Equals("") || event_code == null)
            {
                return RedirectToAction(nameof(ActiveEvent));
            }
            EVENT_SETTING ev = _context.EVENT_SETTING.Where(a => a.EVENT_CODE.Equals(event_code)).FirstOrDefault();
            ev.EVENT_BACKGROUND = ev.EVENT_BACKGROUND.Split("\\").Last();
            HttpContext.Session.SetString("event_standard", ev.EVENT_STANDARD);
            if (ev == null)
            {
                return RedirectToAction(nameof(ActiveEvent));
            }
            ViewBag.ListStandard = GetListStandard();
            ViewBag.ListLastAdditional = GetLastAdditional(10);
            return View(ev);
        }
        [Obsolete]
        public async Task<IActionResult> Manager(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            
            
            if (event_code.Equals(""))
            {
                return RedirectToAction(nameof(ActiveEvent));
            }
            if (!typeSystem.Equals("Custom") || !permission.Equals("admin"))
            {
                return Redirect("~/Login");
            }
            ViewData["CurrentSort"] = sortOrder;
            ViewData["StandardSortParam"] = String.IsNullOrEmpty(sortOrder) ? "standard_desc" : "";
            ViewData["EmpSortParam"] = sortOrder == "Emp" ? "emp_desc" : "Emp";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            //var welfareContext = _context.EmployeeTickets.Where(e => e.Active == true).Include(c => c.Employee).Include(c => c.Ticket).OrderByDescending(c => c.ID);
            var dtContext = new WelfareADO().GetDataTable("select * from " + Factory + "_EMPLOYEE_CUSTOMIZE where EVENT_ACTIVE = 1 and EVENT_CODE = '" + event_code + "'");
            var lstContext = new WelfareADO().ConvertDatatableToList<TS_EMPLOYEE_CUSTOMIZE>(dtContext);
            int pageSize = 10;

            //show index of list
            int pNum = pageNumber ?? 1;//page number
            int tRecord = lstContext.Count();//total record
            int TotalPages = (int)Math.Ceiling(tRecord / (double)pageSize);
            bool isLast = !(pNum < TotalPages);
            int toPageCurrent = isLast ? tRecord : (pNum * pageSize);
            ViewBag.pageFrom = (pNum - 1) * pageSize + 1;
            ViewBag.pageTo = toPageCurrent;
            ViewBag.pageTotal = tRecord;
            //var ev = new WelfareADO(_context).GetDataTable("select EVENT_STANDARD from EVENT_SETTING where EVENT_CODE = '" + event_code + "'");
            ViewBag.standard = HttpContext.Session.GetString("event_standard");

            var temp = TempData["UploadGA"];
            if (temp == null || temp.ToString().Length < 1)
            {
                ViewBag.UploadGA = "";
            }
            else
            {
                ViewBag.UploadGA = temp.ToString();
            }
            return View(await PaginatedList<TS_EMPLOYEE_CUSTOMIZE>.CreateAsync1(lstContext, pageNumber ?? 1, pageSize));
        }
        public List<string> GetListStandard()
        {
            
            
            List<string> lstResult = new List<string>();
            DataTable dt = new WelfareADO().GetDataTable("exec sp_DanhSachTieuChi_Customize '" + event_code + "','" + Factory + "'");
            if (dt.Rows.Count < 1)
            {
                return null;
            }
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                lstResult.Add(dr["EVENT_STANDARD"].ToString());
            }
            return lstResult;
        }
        public List<StandardRemain> GetListStandardRemain()
        {
            
            
            List<StandardRemain> lstResult = new List<StandardRemain>();
            DataTable dt = new WelfareADO().GetDataTable("exec sp_DanhSachTieuChiRemain_Customize '" + event_code + "','" + Factory + "'");
            if (dt.Rows.Count < 1)
            {
                return null;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                lstResult.Add(new StandardRemain()
                {
                    standard = dr["EVENT_STANDARD"].ToString(),
                    remain = dr["remain"].ToString()
                }); 
            }
            return lstResult;
        }
        public List<LAST_CHECK_IN> GetLastCheckin(int number)
        {
            
            
            DataTable dt = new WelfareADO().GetDataTable("exec sp_QuetTheGanNhat_Customize '" + event_code + "','" + Factory + "'," + number);
            var lst = new WelfareADO().ConvertDatatableToList<LAST_CHECK_IN>(dt);
            return lst;
        }
        //lấy danh sách đăng ký bổ sung
        public List<LAST_CHECK_IN> GetLastAdditional(int number)
        {
            
            
            DataTable dt = new WelfareADO().GetDataTable(@"select top " + number + @" EMP_CODE, EMP_CARD, EMP_DEPT, EMP_NAME, EVENT_NOTE, EVENT_STANDARD
  from " + Factory + @"_EMPLOYEE_CUSTOMIZE
  where EVENT_ACTIVE = 1 and EVENT_CODE = '" + event_code + "' and EVENT_NOTE = N'Bổ sung' order by EVENT_DATE desc");
            var lst = new WelfareADO().ConvertDatatableToList<LAST_CHECK_IN>(dt);
            return lst;
        }
        public IActionResult CreateEvent()
        {
            return View();
        }

        [Obsolete]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(IFormCollection iCollection)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = "";
                string filePath = "";
                try
                {
                    IFormFile file = iCollection.Files[0];
                    if (file != null)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "image");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName.Split("\\").Last();
                        filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                    if (iCollection["EVENT_NAME"].Equals(""))
                    {
                        ViewBag.MissInfo = "Thông tin thiếu, vui lòng nhập lại!";
                        return View();
                    }
                    new WelfareADO().ExecuteNon2("INSERT INTO EVENT_SETTING([EVENT_CODE],[EVENT_NAME],[EVENT_TYPE],[EVENT_BACKGROUND],[EVENT_STANDARD],[EVENT_DATE],[EVENT_FACTORY],[EVENT_ACTIVE],[ALLOW_SUPPLIER]) " +
                    "VALUES('" + "EV" + iCollection["EVENT_FACTORY"] + DateTime.Now.ToString("yyMMddHHmmss") + "',N'" + iCollection["EVENT_NAME"] + "','" + iCollection["EVENT_TYPE"] + "','" + filePath + "',N'"
                    + iCollection["EVENT_STANDARD"] + "', '" + Convert.ToDateTime(iCollection["EVENT_DATE"]) + "', '" + iCollection["EVENT_FACTORY"] + "',1,'" + iCollection["ALLOW_SUPPLIER"] + "')");
                    return RedirectToAction(nameof(ActiveEvent));
                }
                catch (Exception)
                {
                    ViewBag.MissInfo = "Thông tin thiếu, vui lòng nhập lại!";
                    return View();
                }
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            
            
            if (id == null)
            {
                return NotFound();
            }
            int hasRecord = new WelfareADO().GetScalar("SELECT COUNT(*) FROM " + Factory + "_EMPLOYEE_CUSTOMIZE WHERE ID ='" + id + "' AND EVENT_CODE = '" + event_code + "'");

            return View(hasRecord);
        }

        // POST: Gifts/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            new WelfareADO().ExecuteNon2("UPDATE " + Factory + "_EMPLOYEE_CUSTOMIZE SET EVENT_ACTIVE = 0 WHERE ID ='" + id + "' AND EVENT_CODE = '" + event_code + "'");
            return RedirectToAction(nameof(Index));
        }
        [Obsolete]
        public IActionResult DisableAll()
        {
            
            
            try
            {
                new LogFunc(_hostingEnvironment).writeToLog(HttpContext.Session.GetString("username"), "Loại bỏ Event Customize " + event_code);
                if (!event_code.Equals(""))
                {
                    new WelfareADO().ExecuteNon2("UPDATE " + Factory + "_EMPLOYEE_CUSTOMIZE set EVENT_ACTIVE = 0 where EVENT_ACTIVE = 1 and EVENT_CODE = '" + event_code + "'");
                    new WelfareADO().ExecuteNon2("update " + Factory + "_EMPLOYEE_CUSTOMIZE_HISTORY set ACTIVE = 0 where ACTIVE = 1 and EVENT_CODE = '" + event_code + "'");
                    //new WelfareADO().ExecuteNon2("update GIFT_TOTAL set ACTIVE = 0 where ACTIVE = 1 and EVENT_CODE = '" + event_code + "'");
                }
                return RedirectToAction(nameof(Manager));
            }
            catch
            {
                return RedirectToAction(nameof(Manager));
            }
        }
        [Obsolete]
        public async Task<IActionResult> NGList(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            
            
            if (!typeSystem.Equals("Custom") || !permission.Equals("admin"))
            {
                return Redirect("~/Login");
            }
            ViewBag.sukien = _context.EVENT_SETTING.Where(a => a.EVENT_CODE.Equals(event_code)).FirstOrDefault().EVENT_NAME;
            DataTable listNG = new WelfareADO().GetDataTable("exec sp_DanhSachNG_Customize '" + event_code + "','" + Factory + "'");
            var welfareContext = new WelfareADO().ConvertDatatableToList<TS_EMPLOYEE_CUSTOMIZE_HISTORY>(listNG);
            int pageSize = 10;
            return View(await PaginatedList<TS_EMPLOYEE_CUSTOMIZE_HISTORY>.CreateAsync1(welfareContext, pageNumber ?? 1, pageSize));
        }
        [Obsolete]
        public IActionResult ExportNG()
        {
            try
            {
                var listNG = new WelfareADO().GetDataTable("exec sp_DanhSachNG_Customize '" + event_code + "','" + Factory + "'");
                if (listNG.Rows.Count < 1)
                {
                    return RedirectToAction(nameof(Manager));
                }
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Danh sách NG");
                    worksheet.Cells["A1"].LoadFromDataTable(listNG, PrintHeaders: true);
                    for (var col = 1; col < listNG.Columns.Count + 1; col++)
                    {
                        worksheet.Column(col).AutoFit();
                    }
                    return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NG_list.xlsx");
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Manager));
            }
        }
        [Obsolete]
        public async Task<IActionResult> CardCheckList(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            
            
            if (!typeSystem.Equals("Custom") || !permission.Equals("admin"))
            {
                return Redirect("~/Login");
            }
            ViewBag.sukien = _context.EVENT_SETTING.Where(a => a.EVENT_CODE.Equals(event_code)).FirstOrDefault().EVENT_NAME;
            DataTable historyCheck = new WelfareADO().GetDataTable(@"select h.EMP_CARD, h.SCAN_DATE, EMP_CODE, EMP_NAME, EMP_DEPT, EVENT_STANDARD, EVENT_PCNAME
from " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY h
join " + Factory + @"_EMPLOYEE_CUSTOMIZE e on e.EMP_CARD = h.EMP_CARD
where h.EVENT_CODE = '" + event_code + @"'
and e.EVENT_CODE = '" + event_code + @"'
and EVENT_ACTIVE = 1
and h.ACTIVE = 1
and e.EVENT_STATUS = 'RECEIVED'
union
select h.EMP_CARD, h.SCAN_DATE, '' as EMP_CODE, '' as EMP_NAME, '' as EMP_DEPT, '' as EVENT_STANDARD, '' as EVENT_PCNAME
from " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY h
where h.EVENT_CODE = '" + event_code + @"'
and ACTIVE = 1
and h.EMP_CARD not in (select EMP_CARD from " + Factory + @"_EMPLOYEE_CUSTOMIZE where EVENT_CODE = '" + event_code + @"' and EVENT_ACTIVE = 1)
order by SCAN_DATE desc");
            var welfareContext = new WelfareADO().ConvertDatatableToList<TS_EMPLOYEE_CUSTOMIZE_HISTORY2>(historyCheck);
            int pageSize = 10;
            return View(await PaginatedList<TS_EMPLOYEE_CUSTOMIZE_HISTORY2>.CreateAsync1(welfareContext, pageNumber ?? 1, pageSize));
        }
        public async Task<IActionResult> CardCheckList2(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            if (!typeSystem.Equals("Custom") || !permission.Equals("admin"))
            {
                return Redirect("~/Login");
            }
            ViewBag.sukien = _context.EVENT_SETTING.Where(a => a.EVENT_CODE.Equals(event_code)).FirstOrDefault().EVENT_NAME;
            DataTable historyCheck = new WelfareADO().GetDataTable(@"select distinct EMP_CODE,EMP_CARD,EMP_NAME,EMP_DEPT,c.EVENT_STANDARD,es.EVENT_NAME,EVENT_STATUS, EVENT_PCNAME
from " + Factory + @"_EMPLOYEE_CUSTOMIZE c
join EVENT_SETTING es on es.EVENT_CODE = c.EVENT_CODE
where c.EVENT_CODE = '" + event_code + "' and c.EVENT_ACTIVE = 1");
            var welfareContext = new WelfareADO().ConvertDatatableToList<LAST_CHECK_IN>(historyCheck);
            int pageSize = 10; //show index of list
            int pNum = pageNumber ?? 1;//page number
            int tRecord = historyCheck.Rows.Count;//total record
            int TotalPages = (int)Math.Ceiling(tRecord / (double)pageSize);
            bool isLast = !(pNum < TotalPages);
            int toPageCurrent = isLast ? tRecord : (pNum * pageSize);
            ViewBag.pageFrom = (pNum - 1) * pageSize + 1;
            ViewBag.pageTo = toPageCurrent;
            ViewBag.pageTotal = tRecord;
            return View(await PaginatedList<LAST_CHECK_IN>.CreateAsync1(welfareContext, pageNumber ?? 1, pageSize));
        }
        public IActionResult ExportCustomizeTotal()
        {
            
            
            try
            {
                var listTotal = new WelfareADO().GetDataTable(@"select distinct EMP_CODE,EMP_CARD,EMP_NAME,EMP_DEPT,c.EVENT_STANDARD,es.EVENT_NAME,EVENT_STATUS, EVENT_PCNAME
from " + Factory + @"_EMPLOYEE_CUSTOMIZE c
join EVENT_SETTING es on es.EVENT_CODE = c.EVENT_CODE
where c.EVENT_CODE = '" + event_code + "' and c.EVENT_ACTIVE = 1");
                if (listTotal.Rows.Count < 1)
                {
                    return RedirectToAction(nameof(Manager));
                }
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Danh sách tổng hợp");
                    worksheet.Cells["A1"].LoadFromDataTable(listTotal, PrintHeaders: true);
                    for (var col = 1; col < listTotal.Columns.Count + 1; col++)
                    {
                        worksheet.Column(col).AutoFit();
                    }
                    return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomizeTotal.xlsx");
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Manager));
            }
        }
        public IActionResult ExportHistory()
        {
            
            
            try
            {
                var listTotal = new WelfareADO().GetDataTable(@"select h.EMP_CARD, h.SCAN_DATE, EMP_CODE, EMP_NAME, EMP_DEPT, EVENT_STANDARD, EVENT_PCNAME
from " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY h
join " + Factory + @"_EMPLOYEE_CUSTOMIZE e on e.EMP_CARD = h.EMP_CARD
where h.EVENT_CODE = '" + event_code + @"'
and e.EVENT_CODE = '" + event_code + @"'
and EVENT_ACTIVE = 1
and h.ACTIVE = 1
and e.EVENT_STATUS = 'RECEIVED'
union
select h.EMP_CARD, h.SCAN_DATE, '' as EMP_CODE, '' as EMP_NAME, '' as EMP_DEPT, '' as EVENT_STANDARD, '' as EVENT_PCNAME
from " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY h
where h.EVENT_CODE = '" + event_code + @"'
and ACTIVE = 1
and h.EMP_CARD not in (select EMP_CARD from " + Factory + @"_EMPLOYEE_CUSTOMIZE where EVENT_CODE = '" + event_code + @"' and EVENT_ACTIVE = 1)
order by SCAN_DATE desc");
                if (listTotal.Rows.Count < 1)
                {
                    return RedirectToAction(nameof(Manager));
                }
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Danh sách thẻ đã quẹt");
                    worksheet.Cells["A1"].LoadFromDataTable(listTotal, PrintHeaders: true);
                    for (var col = 1; col < listTotal.Columns.Count + 1; col++)
                    {
                        worksheet.Column(col).AutoFit();
                    }
                    return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomizeTotal.xlsx");
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Manager));
            }
        }
        [Obsolete]
        public IActionResult ExportCardCheckList()
        {
            
            
            try
            {
                var listDuplicate = new WelfareADO().GetDataTable(@"select *
                    from " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY his
                    where his.ACTIVE = 1 and his.EVENT_CODE = '" + event_code + @"' 
                     order by his.ID desc");

                if (listDuplicate.Rows.Count < 1)
                {
                    return RedirectToAction(nameof(Manager));
                }
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Lịch sử quẹt thẻ");
                    worksheet.Cells["A1"].LoadFromDataTable(listDuplicate, PrintHeaders: true);
                    for (var col = 1; col < listDuplicate.Columns.Count + 1; col++)
                    {
                        worksheet.Column(col).AutoFit();
                    }
                    return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DuplicateCustomize.xlsx");
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Manager));
            }
        }
        [Obsolete]
        public async Task<IActionResult> DuplicateList(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            
            
            if (!typeSystem.Equals("Custom") || !permission.Equals("admin"))
            {
                return Redirect("~/Login");
            }
            ViewBag.sukien = _context.EVENT_SETTING.Where(a => a.EVENT_CODE.Equals(event_code)).FirstOrDefault().EVENT_NAME;
            DataTable listDuplicate = new WelfareADO().GetDataTable(@"select his.ID, his.EMP_CARD, his.SCAN_DATE
                    from " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY his
                    where his.ACTIVE = 1 and his.EVENT_CODE = '" + event_code + @"' 
                    and his.EMP_CARD not in (select em.EMP_CARD
                    from " + Factory + @"_EMPLOYEE_CUSTOMIZE em
                    where em.EVENT_ACTIVE = 1 and em.EVENT_CODE = '" + event_code + @"') group by his.ID, his.EMP_CARD, his.SCAN_DATE
                    having count(his.EMP_CARD) > 1
                    order by ID desc");
            var welfareContext = new WelfareADO().ConvertDatatableToList<TS_EMPLOYEE_CUSTOMIZE_HISTORY>(listDuplicate);
            int pageSize = 10;
            return View(await PaginatedList<TS_EMPLOYEE_CUSTOMIZE_HISTORY>.CreateAsync1(welfareContext, pageNumber ?? 1, pageSize));
        }
        [Obsolete]
        public IActionResult ExportDuplicate()
        {
            
            
            try
            {
                var listDuplicate = new WelfareADO().GetDataTable(@"select his.ID, his.EVENT_CODE, his.EMP_CARD, his.SCAN_DATE, his.ACTIVE
                    from " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY his
                    where his.ACTIVE = 1 and his.EVENT_CODE = '" + event_code + @"' 
                    and his.EMP_CARD not in (select em.EMP_CARD
                    from " + Factory + @"_EMPLOYEE_CUSTOMIZE em
                    where em.EVENT_ACTIVE = 1 and em.EVENT_CODE = '" + event_code + @"') group by his.ID, his.EVENT_CODE, his.EMP_CARD, his.SCAN_DATE, his.ACTIVE
                    having count(his.EMP_CARD) > 1
                    order by ID desc");

                if (listDuplicate.Rows.Count < 1)
                {
                    return RedirectToAction(nameof(Manager));
                }
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Danh sách duplicate");
                    worksheet.Cells["A1"].LoadFromDataTable(listDuplicate, PrintHeaders: true);
                    for (var col = 1; col < listDuplicate.Columns.Count + 1; col++)
                    {
                        worksheet.Column(col).AutoFit();
                    }
                    return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DuplicateCustomize.xlsx");
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Manager));
            }
        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            try
            {
                DataTable dt = new DataTable(typeof(T).Name);
                PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in props)
                {
                    dt.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[props.Length];
                    for (int i = 0; i < props.Length; i++)
                    {
                        values[i] = props[i].GetValue(item, null);
                    }
                    dt.Rows.Add(values);
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IActionResult Import()
        {
            if (TempData["lsError"] != null)
            {
                var l = JsonConvert.DeserializeObject<List<ErrorListTicket>>(TempData["lsError"].ToString());
                ViewBag.eModel = l.Count;
                ViewBag.message = TempData["message"];
                ViewBag.CountUpdate = TempData["countupdate"];
                return View();
            }
            else
            {
                return View();
            }
        }
        public IActionResult Received()
        {
            if (TempData["lsError"] != null)
            {
                var l = JsonConvert.DeserializeObject<List<ErrorListTicket>>(TempData["lsError"].ToString());
                ViewBag.eModel = l.Count;
                ViewBag.message = TempData["message"];
                ViewBag.CountUpdate = TempData["countupdate"];
                return View();
            }
            else
            {
                return View();
            }
        }
        [Obsolete]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, [FromServices] IHostingEnvironment env)
        {
            
            
            List<TS_EMPLOYEE_CUSTOMIZE> ls = new List<TS_EMPLOYEE_CUSTOMIZE>();
            try
            {
                if (file == null || file.Length < 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                if (!file.FileName.Contains("EventCustomizeData"))
                {
                    return NotFound();
                }
                using (var memoStream = new MemoryStream())
                {
                    int count = 0;
                    await file.CopyToAsync(memoStream).ConfigureAwait(false);
                    using (var package = new ExcelPackage(memoStream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        Obj2objCustomize obj = ExcelPackageToList(worksheet);
                        List<TS_EMPLOYEE_CUSTOMIZE> lr = obj.lstEmploySuccess;
                        ls = obj.lstEmployError;
                        int tongSoDong = lr.Count;
                        int soLanFor = (int)(tongSoDong / 894) + 1;
                        for (int i = 1; i <= soLanFor; i++)
                        {
                            List<TS_EMPLOYEE_CUSTOMIZE> li = new List<TS_EMPLOYEE_CUSTOMIZE>();
                            if (i < soLanFor)
                            {
                                li = lr.Skip((i - 1) * 894).Take(894).ToList();
                            }
                            else if (i == soLanFor)
                            {
                                li = lr.Skip((i - 1) * 894).ToList();
                            }
                            string sql = "INSERT INTO " + Factory + "_EMPLOYEE_CUSTOMIZE([EMP_CODE],[EMP_CARD],[EMP_NAME],[EMP_DEPT],[EVENT_CODE],[EVENT_STANDARD],[EVENT_DATE],[EVENT_NOTE],[EVENT_STATUS],[EVENT_PCNAME],[EVENT_ACTIVE]) VALUES";
                            foreach (TS_EMPLOYEE_CUSTOMIZE r in li)
                            {
                                try
                                {
                                    sql += "('" + r.EMP_CODE + "','" + r.EMP_CARD + "',N'" + r.EMP_NAME + "',N'" + r.EMP_DEPT + "', '" + event_code + "', N'" + r.EVENT_STANDARD + "', '" + DateTime.Now + "', N'" + r.EVENT_NOTE + "', 'OK', N'" + r.EVENT_PCNAME + "', 1),";
                                }
                                catch
                                {

                                }
                            }
                            try
                            {
                                sql = sql.Substring(0, sql.Length - 1);
                                count += new WelfareADO().ExecuteNon2(sql);
                            }
                            catch (Exception)
                            {

                            }
                        }
                        //foreach (TS_EMPLOYEE_CUSTOMIZE r in lr)
                        //{
                        //    try
                        //    {
                        //        string sql = "INSERT INTO " + Factory + "_EMPLOYEE_CUSTOMIZE([EMP_CODE],[EMP_CARD],[EMP_NAME],[EMP_DEPT],[EVENT_CODE],[EVENT_STANDARD],[EVENT_DATE],[EVENT_NOTE],[EVENT_STATUS],[EVENT_PCNAME],[EVENT_ACTIVE]) " +
                        //            "VALUES('" + r.EMP_CODE + "','" + r.EMP_CARD + "',N'" + r.EMP_NAME + "',N'" + r.EMP_DEPT + "', '" + event_code + "', N'" + r.EVENT_STANDARD + "', '" + DateTime.Now + "', N'" + r.EVENT_NOTE + "', 'OK', N'" + r.EVENT_PCNAME + "', 1)";
                        //        count += new WelfareADO().ExecuteNon2(sql);
                        //    }
                        //    catch(Exception ex)
                        //    {
                        //        continue;
                        //    }
                        //}
                        TempData["lsError"] = JsonConvert.SerializeObject(ls);
                        TempData["lsError1"] = JsonConvert.SerializeObject(ls);
                        TempData["message"] = "Thêm mới " + count + " bản ghi thành công !";
                        new LogFunc(_hostingEnvironment).writeToLog(HttpContext.Session.GetString("username"), "Import dữ liệu Event Customize " + event_code);
                        return RedirectToAction(nameof(Import));
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Import));
            }
        }

        [Obsolete]
        [HttpPost]
        public async Task<IActionResult> UploadReceived(IFormFile file, [FromServices] IHostingEnvironment env)
        {
            
            
            List<TS_EMPLOYEE_CUSTOMIZE> ls = new List<TS_EMPLOYEE_CUSTOMIZE>();
            try
            {
                if (file == null || file.Length < 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                if (!file.FileName.Contains("EventCustomizeDataReceived"))
                {
                    return NotFound();
                }
                using (var memoStream = new MemoryStream())
                {
                    int count = 0;
                    await file.CopyToAsync(memoStream).ConfigureAwait(false);
                    using (var package = new ExcelPackage(memoStream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        Obj2objCustomize obj = ExcelPackageToList(worksheet);
                        List<TS_EMPLOYEE_CUSTOMIZE> lr = obj.lstEmploySuccess;
                        ls = obj.lstEmployError;
                        int tongSoDong = lr.Count;
                        int soLanFor = (int)(tongSoDong / 894) + 1;
                        for (int i = 1; i <= soLanFor; i++)
                        {
                            List<TS_EMPLOYEE_CUSTOMIZE> li = new List<TS_EMPLOYEE_CUSTOMIZE>();
                            if (i < soLanFor)
                            {
                                li = lr.Skip((i - 1) * 894).Take(894).ToList();
                            }
                            else if (i == soLanFor)
                            {
                                li = lr.Skip((i - 1) * 894).ToList();
                            }
                            string sql = "INSERT INTO " + Factory + "_EMPLOYEE_CUSTOMIZE([EMP_CODE],[EMP_CARD],[EMP_NAME],[EMP_DEPT],[EVENT_CODE],[EVENT_STANDARD],[EVENT_DATE],[EVENT_NOTE],[EVENT_STATUS],[EVENT_PCNAME],[EVENT_ACTIVE]) VALUES";
                            string sql_history = @"INSERT INTO " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY([EMP_CARD],[EVENT_CODE],[SCAN_DATE],[ACTIVE]) VALUES";
                            foreach (TS_EMPLOYEE_CUSTOMIZE r in li)
                            {
                                try
                                {
                                    sql += "('" + r.EMP_CODE + "','" + r.EMP_CARD + "',N'" + r.EMP_NAME + "',N'" + r.EMP_DEPT + "', '" + event_code + "', N'" + r.EVENT_STANDARD + "', '" + DateTime.Now + "', N'Bổ sung', 'RECEIVED', N'cvn-welfare', 1),";
                                    sql_history += "('" + r.EMP_CARD + "','" + event_code + "','" + DateTime.Now + "',1),";
                                }
                                catch
                                {

                                }
                            }
                            try
                            {
                                sql = sql.Substring(0, sql.Length - 1);
                                count += new WelfareADO().ExecuteNon2(sql);
                                sql_history = sql_history.Substring(0, sql_history.Length - 1);
                                new WelfareADO().ExecuteNon2(sql_history);
                            }
                            catch (Exception)
                            {

                            }
                        }
                        //foreach (TS_EMPLOYEE_CUSTOMIZE r in lr)
                        //{
                        //    try
                        //    {
                        //        string sql = "INSERT INTO " + Factory + "_EMPLOYEE_CUSTOMIZE([EMP_CODE],[EMP_CARD],[EMP_NAME],[EMP_DEPT],[EVENT_CODE],[EVENT_STANDARD],[EVENT_DATE],[EVENT_NOTE],[EVENT_STATUS],[EVENT_PCNAME],[EVENT_ACTIVE]) " +
                        //            "VALUES('" + r.EMP_CODE + "','" + r.EMP_CARD + "',N'" + r.EMP_NAME + "',N'" + r.EMP_DEPT + "', '" + event_code + "', N'" + r.EVENT_STANDARD + "', '" + DateTime.Now + "', N'" + r.EVENT_NOTE + "', 'OK', N'" + r.EVENT_PCNAME + "', 1)";
                        //        count += new WelfareADO().ExecuteNon2(sql);
                        //    }
                        //    catch(Exception ex)
                        //    {
                        //        continue;
                        //    }
                        //}
                        TempData["lsError"] = JsonConvert.SerializeObject(ls);
                        TempData["lsError1"] = JsonConvert.SerializeObject(ls);
                        TempData["message"] = "Thêm mới " + count + " người nhận quà thành công !";
                        new LogFunc(_hostingEnvironment).writeToLog(HttpContext.Session.GetString("username"), "Import dữ liệu Event Customize Received" + event_code);
                        return RedirectToAction(nameof(Import));
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Import));
            }
        }


        [Obsolete]
        public Obj2objCustomize ExcelPackageToList(ExcelWorksheet worksheet)
        {
            //khởi tạo danh sách đăng ký, danh sách bị lỗi
            List<TS_EMPLOYEE_CUSTOMIZE> lr = new List<TS_EMPLOYEE_CUSTOMIZE>();
            List<TS_EMPLOYEE_CUSTOMIZE> le = new List<TS_EMPLOYEE_CUSTOMIZE>();
            var rowCount = worksheet.Dimension?.Rows;
            if (rowCount.HasValue && rowCount.Value > 1)
            {
                for (int row = 2; row <= rowCount.Value; row++)
                {
                    try
                    {
                        string card, code, name, dept, standard, date, note, status, pcname;
                        code = Convert.ToString(worksheet.Cells[row, 1].Value).Trim().PadLeft(6, '0').GetLast(6);
                        card = Convert.ToString(worksheet.Cells[row, 2].Value).ToString().Trim().PadLeft(5, '0').GetLast(5);
                        name = Convert.ToString(worksheet.Cells[row, 3].Value).ToString().Trim();
                        dept = Convert.ToString(worksheet.Cells[row, 4].Value).ToString().Trim();
                        standard = Convert.ToString(worksheet.Cells[row, 5].Value).ToString().Trim();
                        date = Convert.ToString(worksheet.Cells[row, 6].Value).ToString().Trim();
                        note = Convert.ToString(worksheet.Cells[row, 7].Value).ToString().Trim();
                        status = Convert.ToString(worksheet.Cells[row, 8].Value).ToString().Trim();
                        pcname = Convert.ToString(worksheet.Cells[row, 9].Value).ToString().Trim();
                        if (code.Equals("000000"))
                        {
                            TS_EMPLOYEE_CUSTOMIZE el = new TS_EMPLOYEE_CUSTOMIZE()
                            {
                                EMP_CARD = card,
                                EMP_CODE = "",
                                EMP_DEPT = dept,
                                EMP_NAME = name,
                                EVENT_ACTIVE = false,
                                EVENT_CODE = event_code,
                                EVENT_DATE = DateTime.Now,
                                EVENT_NOTE = note,
                                EVENT_PCNAME = pcname,
                                EVENT_STANDARD = standard,
                                EVENT_STATUS = "Mã nhân viên không xác định"
                            };
                            le.Add(el);
                            continue;
                        }
                        if (card.Equals("00000"))
                        {
                            TS_EMPLOYEE_CUSTOMIZE el = new TS_EMPLOYEE_CUSTOMIZE()
                            {
                                EMP_CARD = "",
                                EMP_CODE = code,
                                EMP_DEPT = dept,
                                EMP_NAME = name,
                                EVENT_ACTIVE = false,
                                EVENT_CODE = event_code,
                                EVENT_DATE = DateTime.Now,
                                EVENT_NOTE = note,
                                EVENT_PCNAME = pcname,
                                EVENT_STANDARD = standard,
                                EVENT_STATUS = "Mã thẻ không xác định"
                            };
                            le.Add(el);
                            continue;
                        }
                        bool r = CheckExistsEmployeeCustomize(card, code, event_code);
                        if (!r)
                        {
                            TS_EMPLOYEE_CUSTOMIZE el = new TS_EMPLOYEE_CUSTOMIZE()
                            {
                                EMP_CARD = card,
                                EMP_CODE = code,
                                EMP_DEPT = dept,
                                EMP_NAME = name,
                                EVENT_ACTIVE = false,
                                EVENT_CODE = event_code,
                                EVENT_DATE = DateTime.Now,
                                EVENT_NOTE = note,
                                EVENT_PCNAME = pcname,
                                EVENT_STANDARD = standard,
                                EVENT_STATUS = status
                            };
                            lr.Add(el);
                            continue;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            return new Obj2objCustomize()
            {
                lstEmploySuccess = lr,
                lstEmployError = le
            };
        }
        [Obsolete]
        public bool CheckExistsEmployeeCustomize(string card, string code, string event_code)
        {
            
            
            try
            {
                return (new WelfareADO().GetDataTable(@"SELECT *
                    FROM " + Factory + @"_EMPLOYEE_CUSTOMIZE
                    WHERE(EMP_CARD = '" + card + "' OR EMP_CODE = '" + code + "') AND EVENT_ACTIVE = 1 AND EVENT_CODE = '" + event_code + "'")).Rows.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [Obsolete]
        public IActionResult ExportListError()
        {
            try
            {
                var l = JsonConvert.DeserializeObject<List<TS_EMPLOYEE_CUSTOMIZE>>(TempData["lsError1"].ToString());
                if (l.Count > 0)
                {
                    DataTable dataTable = ToDataTable<TS_EMPLOYEE_CUSTOMIZE>(l);
                    using (var package1 = new ExcelPackage())
                    {
                        var worksheet1 = package1.Workbook.Worksheets.Add("Error list");
                        worksheet1.Cells["A1"].LoadFromDataTable(dataTable, PrintHeaders: true);
                        for (var col = 1; col < dataTable.Columns.Count + 1; col++)
                        {
                            worksheet1.Column(col).AutoFit();
                        }
                        return File(package1.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EventCustomizeData.xlsx");
                    }
                }
                new LogFunc(_hostingEnvironment).writeToLog(HttpContext.Session.GetString("username"), "Xuất dữ liệu Import Event Customize lỗi " + event_code);
                return RedirectToAction(nameof(Manager));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Manager));
            }
        }
        [Obsolete]
        public IActionResult Sample()
        {
            //var fileNameDownload = "EventCustomizeData.xlsx";
            //var reportFolder = "files";
            //var filePath = Path.Combine(_hostingEnvironment.WebRootPath, reportFolder, fileNameDownload);
            //var fileName = "EventCustomizeData.xlsx";
            //var mimeType = "application/vnd.ms-excel";
            //return File(new FileStream(filePath, FileMode.Open), mimeType, fileName);

            var net = new System.Net.WebClient();
            var data = net.DownloadData(Path.Combine(_hostingEnvironment.WebRootPath, "files\\EventCustomizeData.xlsx"));
            var content = new System.IO.MemoryStream(data);
            var contentType = "APPLICATION/octet-stream";
            var fileName = "EventCustomizeData.xlsx";
            return File(content, contentType, fileName);
        }
        [Obsolete]
        public IActionResult SampleReceived()
        {
            var net = new System.Net.WebClient();
            var data = net.DownloadData(Path.Combine(_hostingEnvironment.WebRootPath, "files\\EventCustomizeDataReceived.xlsx"));
            var content = new System.IO.MemoryStream(data);
            var contentType = "APPLICATION/octet-stream";
            var fileName = "EventCustomizeDataReceived.xlsx";
            return File(content, contentType, fileName);
        }
        [Obsolete]
        public EVENT_SETTING GetDetailEvent(string factory)
        {
            try
            {
                var d = new WelfareADO().GetDataTable("SELECT * FROM EVENT_SETTING WHERE EVENT_CODE = '" + event_code + "' AND EVENT_FACTORY='" + factory + "'");
                if(d.Rows.Count < 1)
                {
                    return null;
                }
                EVENT_SETTING ev = new EVENT_SETTING()
                {
                    EVENT_ACTIVE = Convert.ToBoolean(d.Rows[0][0].ToString()),
                    EVENT_CODE = d.Rows[0][0].ToString(),
                    EVENT_DATE = Convert.ToDateTime(d.Rows[0][0].ToString()),
                    EVENT_FACTORY = d.Rows[0][0].ToString(),
                    EVENT_NAME = d.Rows[0][0].ToString(),
                    EVENT_BACKGROUND = d.Rows[0][0].ToString(),
                    EVENT_STANDARD = d.Rows[0][0].ToString(),
                    EVENT_TYPE = 6
                };
                return ev;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IActionResult StatisticByStandard()
        {
            
            
            DataTable dt = new WelfareADO().GetDataTable("exec sp_ThongKeTheoTieuChi_Customize '" + event_code + "','" + Factory + "'");
            var lst = new WelfareADO().ConvertDatatableToList<STT_BY_STANDARD>(dt);
            STT_BY_STANDARD sumary = new STT_BY_STANDARD()
            {
                standard = "Tổng",
                total = lst.Sum(a => a.total),
                checked1 = lst.Sum(a => a.checked1),
                notcheck = lst.Sum(a => a.notcheck)
            };
            lst.Add(sumary);
            List<REPORT_BY_STANDARD> lstReport = new List<REPORT_BY_STANDARD>();
            foreach(STT_BY_STANDARD item in lst)
            {
                REPORT_BY_STANDARD itemRp = new REPORT_BY_STANDARD()
                {
                    standard = item.standard,
                    checked1 = item.checked1,
                    notcheck = item.notcheck,
                    total = item.total
                };
                lstReport.Add(itemRp);
            }
            return View(lstReport);
        }
        public IActionResult StatisticByDept()
        {
            
            
            DataTable dt = new WelfareADO().GetDataTable("exec sp_ThongKeTheoPhongBan_Customize '" + event_code + "','" + Factory + "'");
            var lst = new WelfareADO().ConvertDatatableToList<STT_BY_DEPT>(dt);
            STT_BY_DEPT sumary = new STT_BY_DEPT()
            {
                dept = "Tổng",
                total = lst.Sum(a => a.total),
                checked1 = lst.Sum(a => a.checked1),
                notcheck = lst.Sum(a => a.notcheck)
            };
            lst.Add(sumary);
            List<REPORT_BY_DEPT> lstReport = new List<REPORT_BY_DEPT>();
            foreach (STT_BY_DEPT item in lst)
            {
                REPORT_BY_DEPT itemRp = new REPORT_BY_DEPT()
                {
                    dept = item.dept,
                    checked1 = item.checked1,
                    notcheck = item.notcheck,
                    total = item.total
                };
                lstReport.Add(itemRp);
            }
            return View(lstReport);
        }
        [Obsolete]
        public IActionResult ExportReport()
        {
            try
            {
                
                

                #region danh sách thống kê theo tiêu chí standard
                DataTable dt1 = new WelfareADO().GetDataTable("exec sp_ThongKeTheoTieuChi_Customize '" + event_code + "','" + Factory + "'");
                var lst1 = new WelfareADO().ConvertDatatableToList<STT_BY_STANDARD>(dt1);
                STT_BY_STANDARD sumary1 = new STT_BY_STANDARD()
                {
                    standard = "Tổng",
                    total = lst1.Sum(a => a.total),
                    checked1 = lst1.Sum(a => a.checked1),
                    notcheck = lst1.Sum(a => a.notcheck)
                };
                lst1.Add(sumary1);
                List<REPORT_BY_STANDARD> lstReport1 = new List<REPORT_BY_STANDARD>();
                foreach (STT_BY_STANDARD item in lst1)
                {
                    REPORT_BY_STANDARD itemRp = new REPORT_BY_STANDARD()
                    {
                        standard = item.standard,
                        checked1 = item.checked1,
                        notcheck = item.notcheck,
                        total = item.total
                    };
                    lstReport1.Add(itemRp);
                }
                DataTable dt11 = ToDataTable<REPORT_BY_STANDARD>(lstReport1);
                #endregion
                int numberRow = dt11.Rows.Count;

                #region danh sách thống kê theo phòng ban
                DataTable dt2 = new WelfareADO().GetDataTable("exec sp_ThongKeTheoPhongBan_Customize '" + event_code + "','" + Factory + "'");
                var lst2 = new WelfareADO().ConvertDatatableToList<STT_BY_DEPT>(dt2);
                STT_BY_DEPT sumary2 = new STT_BY_DEPT()
                {
                    dept = "Tổng",
                    total = lst2.Sum(a => a.total),
                    checked1 = lst2.Sum(a => a.checked1),
                    notcheck = lst2.Sum(a => a.notcheck)
                };
                lst2.Add(sumary2);
                List<REPORT_BY_DEPT> lstReport2 = new List<REPORT_BY_DEPT>();
                foreach (STT_BY_DEPT item in lst2)
                {
                    REPORT_BY_DEPT itemRp = new REPORT_BY_DEPT()
                    {
                        dept = item.dept,
                        checked1 = item.checked1,
                        notcheck = item.notcheck,
                        total = item.total
                    };
                    lstReport2.Add(itemRp);
                }
                DataTable dt22 = ToDataTable<REPORT_BY_DEPT>(lstReport2);
                #endregion
                int numberRow2 = dt22.Rows.Count;

                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "files\\Report by Event Customize.xlsx");
                FileInfo file = new FileInfo(filePath);
                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.First();
                    excelWorksheet.InsertRow(6, numberRow);
                    excelWorksheet.Cells["A6:F" + (numberRow + 5)].Style.Font.Size = 11;
                    excelWorksheet.Cells["A6:F" + (numberRow + 5)].Style.Font.Name = "Times New Roman";
                    excelWorksheet.Cells["A6:F" + (numberRow + 5)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    excelWorksheet.Cells["A6:F" + (numberRow + 5)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    excelWorksheet.Cells["A6:F" + (numberRow + 5)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    excelWorksheet.Cells["A6:F" + (numberRow + 5)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    excelWorksheet.Cells["A6:F" + (numberRow + 5)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Cells["A6"].LoadFromDataTable(dt11, false);

                    int dept = 10 + numberRow;
                    excelWorksheet.InsertRow(dept, numberRow2);
                    excelWorksheet.Cells["A" + dept + ":H" + (numberRow2 + dept)].Style.Font.Size = 11;
                    excelWorksheet.Cells["A" + dept + ":H" + (numberRow2 + dept)].Style.Font.Name = "Times New Roman";
                    excelWorksheet.Cells["A" + dept + ":H" + (numberRow2 + dept)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    excelWorksheet.Cells["A" + dept + ":H" + (numberRow2 + dept)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    excelWorksheet.Cells["A" + dept + ":H" + (numberRow2 + dept)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    excelWorksheet.Cells["A" + dept + ":H" + (numberRow2 + dept)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    excelWorksheet.Cells["A" + dept + ":H" + (numberRow2 + dept)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Cells["A" + dept].LoadFromDataTable(dt22, false);

                    return File(excelPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Thống kê sự kiện.xlsx");
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Manager));
            }
        }
        //22122020 chuyen sang dung bat dong bo
        //public async Task<SumaryEvent> GetSumaryCustomize()
        //{
        //    Factory = Factory;
        //    event_code = HttpContext.Session.GetString("event_code");
        //    try
        //    {
        //        SumaryEvent sg = new SumaryEvent()
        //        {
        //            total = await new WelfareADO().GetScalarAsync(@"select COUNT(*)
        //                from " + Factory + @"_EMPLOYEE_CUSTOMIZE
        //                where EVENT_CODE = '" + event_code + "' and EVENT_ACTIVE = 1"),
        //            check = await new WelfareADO().GetScalarAsync(@"select COUNT(*)
        //                from " + Factory + @"_EMPLOYEE_CUSTOMIZE
        //                where EVENT_CODE = '" + event_code + "' and EVENT_STATUS = 'RECEIVED' and EVENT_ACTIVE = 1")
        //        };
        //        return sg;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        public SumaryEvent GetSumaryCustomize()
        {
            
            
            try
            {
                SumaryEvent sg = new SumaryEvent()
                {
                    total = new WelfareADO().GetScalar(@"select COUNT(*)
                        from " + Factory + @"_EMPLOYEE_CUSTOMIZE
                        where EVENT_CODE = '" + event_code + "' and EVENT_ACTIVE = 1"),
                    check = new WelfareADO().GetScalar(@"select COUNT(*)
                        from " + Factory + @"_EMPLOYEE_CUSTOMIZE
                        where EVENT_CODE = '" + event_code + "' and EVENT_STATUS = 'RECEIVED' and EVENT_ACTIVE = 1")
                };
                return sg;
            }
            catch (Exception)
            {
                return null;
            }
        }
        //public async Task<JsonResult> GetStatisticCurrent()
        //{
        //    Factory = Factory;
        //    event_code = HttpContext.Session.GetString("event_code");
        //    DataTable dt = await new WelfareADO().GetDataTableAsync(@"select em.EVENT_STANDARD as standard, count(*) as total, 
        //        (Select count(*) from " + Factory + @"_EMPLOYEE_CUSTOMIZE e1 where e1.EVENT_STANDARD = em.EVENT_STANDARD and e1.EVENT_ACTIVE = 1 and e1.EVENT_CODE = '" + event_code + @"' and e1.EVENT_STATUS = 'RECEIVED') as checked1,
        //        (Select count(*) from " + Factory + @"_EMPLOYEE_CUSTOMIZE e2 where e2.EVENT_STANDARD = em.EVENT_STANDARD and e2.EVENT_ACTIVE = 1 and e2.EVENT_CODE = '" + event_code + @"' and e2.EVENT_STATUS = 'OK') as notcheck
        //        from " + Factory + @"_EMPLOYEE_CUSTOMIZE em
        //        where em.EVENT_STANDARD in (
        //        select distinct EVENT_STANDARD
        //        from " + Factory + @"_EMPLOYEE_CUSTOMIZE) and em.EVENT_ACTIVE = 1 and em.EVENT_CODE = '" + event_code + @"'
        //        group by EVENT_STANDARD
        //        ");
        //    var lst = new WelfareADO().ConvertDatatableToList<STT_BY_STANDARD>(dt);
        //    STT_BY_STANDARD sumary = new STT_BY_STANDARD()
        //    {
        //        standard = "Tổng",
        //        total = lst.Sum(a => a.total),
        //        checked1 = lst.Sum(a => a.checked1),
        //        notcheck = lst.Sum(a => a.notcheck)
        //    };
        //    lst.Add(sumary);
        //    return Json(lst);
        //}
        public JsonResult GetStatisticCurrent()
        {
            
            
            DataTable dt = new WelfareADO().GetDataTable("exec sp_ThongKeTheoTieuChi_Customize '" + event_code + "','" + Factory + "'");
            var lst = new WelfareADO().ConvertDatatableToList<STT_BY_STANDARD>(dt);
            STT_BY_STANDARD sumary = new STT_BY_STANDARD()
            {
                standard = "Tổng",
                total = lst.Sum(a => a.total),
                checked1 = lst.Sum(a => a.checked1),
                notcheck = lst.Sum(a => a.notcheck)
            };
            lst.Add(sumary);
            return Json(lst);
        }
        public List<REPORT_BY_DEPT> GetDataByDept()
        {
            
            
            DataTable dt = new WelfareADO().GetDataTable("exec sp_ThongKeTheoPhongBan_Customize '" + event_code + "','" + Factory + "'");
            var lst = new WelfareADO().ConvertDatatableToList<STT_BY_DEPT>(dt);
            STT_BY_DEPT sumary = new STT_BY_DEPT()
            {
                dept = "Tổng",
                total = lst.Sum(a => a.total),
                checked1 = lst.Sum(a => a.checked1),
                notcheck = lst.Sum(a => a.notcheck)
            };
            lst.Add(sumary);
            List<REPORT_BY_DEPT> lstReport = new List<REPORT_BY_DEPT>();
            foreach (STT_BY_DEPT item in lst)
            {
                REPORT_BY_DEPT itemRp = new REPORT_BY_DEPT()
                {
                    dept = item.dept,
                    checked1 = item.checked1,
                    notcheck = item.notcheck,
                    total = item.total
                };
                lstReport.Add(itemRp);
            }
            return lstReport;
        }
        public JsonResult Recog(int empid, string pc)
        {
            
            
            try
            {
                var emCustom = new WelfareADO().GetDataTable(@"select *
                    from " + Factory + @"_EMPLOYEE_CUSTOMIZE
                    where ID = " + empid + " and EVENT_CODE = '" + event_code + "' and EVENT_ACTIVE = 1");
                if (emCustom.Rows.Count < 1 && empid > 0)
                {
                    return Json(2);
                }
                //int iUpdate = new WelfareADO().ExecuteNon2(@"UPDATE " + Factory + @"_EMPLOYEE_CUSTOMIZE
                //    SET EVENT_STATUS = 'RECEIVED'
                //    WHERE ID = '" + empid + "'");
                string emp_card = new WelfareADO().GetDataTable("SELECT EMP_CARD FROM " + Factory + "_EMPLOYEE_CUSTOMIZE WHERE ID = " + empid).Rows[0][0].ToString();
                string sqlInsert = @"INSERT INTO " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY([EMP_CARD],[EVENT_CODE],[SCAN_DATE],[ACTIVE])
                            VALUES('" + emp_card + "', '" + event_code + "', '" + DateTime.Now + "', 1)";
                //int iInsert = new WelfareADO().ExecuteNon2(sqlInsert);
                int result = new WelfareADO().UseTransaction(@"UPDATE " + Factory + @"_EMPLOYEE_CUSTOMIZE
                    SET EVENT_STATUS = 'RECEIVED', EVENT_PCNAME = '" + pc + @"'
                    WHERE ID = '" + empid + "'", sqlInsert);
                if (result > 0)
                {
                    return Json(1);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                return Json(0);
            }
        }
        //ghi nhận trường hợp phát bổ sung
        public JsonResult RecogAdd(string name, string code, string card, string dept, string standard)
        {
            if (code.Length < 6 || card.Length < 6)
            {
                return Json(2);
            }
            
            
            try
            {
                var emCustom = new WelfareADO().GetDataTable(@"select *
                    from " + Factory + @"_EMPLOYEE_CUSTOMIZE
                    where EMP_CODE = '" + code + "' and EVENT_CODE = '" + event_code + "' and EVENT_ACTIVE = 1");
                if (emCustom.Rows.Count > 0)
                {
                    return Json(2);
                }
                card = card.Substring(1, 5);
                string sqlInsert = @"
INSERT INTO [dbo].[" + Factory + @"_EMPLOYEE_CUSTOMIZE]
           ([EMP_CODE]
           ,[EMP_CARD]
           ,[EMP_NAME]
           ,[EMP_DEPT]
           ,[EVENT_CODE]
           ,[EVENT_STANDARD]
           ,[EVENT_DATE]
           ,[EVENT_NOTE]
           ,[EVENT_STATUS]
           ,[EVENT_PCNAME]
           ,[EVENT_ACTIVE])
     VALUES
           (N'" + code + @"'
           ,N'" + card + @"'
           ,N'" + name + @"'
           ,N'" + dept + @"'
           ,'" + event_code + @"'
           ,N'" + standard + @"'
           ,getdate()
           ,N'Bổ sung'
           ,'OK'
           ,N'Bổ sung'
           ,1)
";
                int iInsert = new WelfareADO().ExecuteNon2(sqlInsert);
                if (iInsert > 0)
                {
                    return Json(1);
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception)
            {
                return Json(0);
            }
        }
        //22122020 thêm standa để check xem máy tính đó có được phép quẹt loại quà này không
        public async Task<JsonResult> CheckIn(string card_no, string standa)
        {
            standa = String.IsNullOrEmpty(standa) ? "all" : standa + ",";
            
            
            string card = card_no.PadLeft(5, '0').GetLast(5);
            try
            {
                DataTable dt = new WelfareADO().GetDataTable(@"select *
                    from " + Factory + @"_EMPLOYEE_CUSTOMIZE
                    where EVENT_ACTIVE = 1 and EMP_CARD = '" + card + "' and EVENT_CODE = '" + event_code + "'");
                if (dt.Rows.Count > 0)
                {
                    var regisInfo = new TS_EMPLOYEE_CUSTOMIZE()
                    {
                        EMP_CARD = dt.Rows[0]["EMP_CARD"].ToString().Trim(),
                        EMP_CODE = dt.Rows[0]["EMP_CODE"].ToString().Trim(),
                        EMP_DEPT = dt.Rows[0]["EMP_DEPT"].ToString().Trim(),
                        EMP_NAME = dt.Rows[0]["EMP_NAME"].ToString().Trim(),
                        EVENT_ACTIVE = Convert.ToBoolean(dt.Rows[0]["EVENT_ACTIVE"].ToString().Trim()),
                        EVENT_CODE = dt.Rows[0]["EVENT_CODE"].ToString().Trim(),
                        EVENT_DATE = Convert.ToDateTime(dt.Rows[0]["EVENT_DATE"].ToString().Trim()),
                        EVENT_NOTE = dt.Rows[0]["EVENT_NOTE"].ToString().Trim(),
                        EVENT_PCNAME = dt.Rows[0]["EVENT_PCNAME"].ToString().Trim(),
                        EVENT_STANDARD = dt.Rows[0]["EVENT_STANDARD"].ToString().Trim(),
                        EVENT_STATUS = dt.Rows[0]["EVENT_STATUS"].ToString().Trim(),
                        ID = Convert.ToInt32(dt.Rows[0]["ID"])
                    };
                    string check = dt.Rows[0]["EVENT_STATUS"].ToString().Trim();//OK la chua check, RECEIVED la da check in
                    if (check.Equals("RECEIVED"))//đã check in
                    {
                        var dtCheck = new WelfareADO().GetDataTable(@"select SCAN_DATE
                            from " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY
                            where EVENT_CODE = '" + event_code + "' and ACTIVE = 1 and EMP_CARD = '" + regisInfo.EMP_CARD + "'");
                        if(dtCheck.Rows.Count > 0)
                        {
                            return Json(new { code = 1, info = regisInfo, time = dtCheck.Rows[0][0].ToString() });//nếu có thông tin đăng ký và đã check in
                        }
                        return Json(new { code = 1, info = regisInfo, time = " thời gian không xác định" });//nếu có thông tin đăng ký nhưng mất thông tin check in
                    }
                    else if (check.Equals("OK"))//chưa check in
                    {
                        if (standa.Equals("all") || standa.Contains(regisInfo.EVENT_STANDARD + ","))
                        {
                            return Json(new { code = 2, info = regisInfo });//nếu có thông tin đăng ký và đúng loại quà
                        }
                        if (!standa.Contains(regisInfo.EVENT_STANDARD + ","))
                        {
                            return Json(new { code = 4, info = regisInfo });//nếu có thông tin đăng ký và loại quà khác
                        }
                        return Json(new { code = 2, info = regisInfo });//nếu có thông tin đăng ký và đúng loại quà đc nhận
                    }
                    else
                    {
                        string sql = @"INSERT INTO " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY([EMP_CARD],[EVENT_CODE],[SCAN_DATE],[ACTIVE])
                            VALUES('" + card + "', '" + event_code + "', '" + DateTime.Now + "', 1)";
                        await new WelfareADO().ExecuteNon2Async(sql);
                        return Json(new { code = 0 });//không có thông tin đăng ký
                    }
                }
                else
                {
                    string sql = @"INSERT INTO " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY([EMP_CARD],[EVENT_CODE],[SCAN_DATE],[ACTIVE])
                            VALUES('" + card + "', '" + event_code + "', '" + DateTime.Now + "', 1)";
                    await new WelfareADO().ExecuteNon2Async(sql);
                    return Json(new { code = 0 });
                }
            }
            catch (Exception)
            {
                string sql = @"INSERT INTO " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY([EMP_CARD],[EVENT_CODE],[SCAN_DATE],[ACTIVE])
                            VALUES('" + card + "', '" + event_code + "', '" + DateTime.Now + "', 1)";
                await new WelfareADO().ExecuteNon2Async(sql);
                return Json(new { code = 3 });
            }
        }
        //sử dụng cho phát bổ sung
        public async Task<JsonResult> CheckInAdd(string card_no)
        {
            
            
            string card = card_no.PadLeft(5, '0').GetLast(5);
            try
            {
                DataTable dt = new WelfareADO().GetDataTable(@"select *
                    from " + Factory + @"_EMPLOYEE_CUSTOMIZE
                    where EVENT_ACTIVE = 1 and EMP_CARD = '" + card + "' and EVENT_CODE = '" + event_code + "'");
                if (dt.Rows.Count < 1)
                {
                    string sql = @"INSERT INTO " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY([EMP_CARD],[EVENT_CODE],[SCAN_DATE],[ACTIVE])
                            VALUES('" + card + "-bs', '" + event_code + "', '" + DateTime.Now + "', 1)";
                    await new WelfareADO().ExecuteNon2Async(sql);
                    return Json(new { code = 1 });//không có thông tin đăng ký thì insert vào lịch sử quẹt thẻ
                }
                else
                {
                    string sql = @"INSERT INTO " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY([EMP_CARD],[EVENT_CODE],[SCAN_DATE],[ACTIVE])
                            VALUES('" + card + "-du', '" + event_code + "', '" + DateTime.Now + "', 1)";
                    await new WelfareADO().ExecuteNon2Async(sql);
                    return Json(new { code = 0 });
                }
            }
            catch (Exception)
            {
                string sql = @"INSERT INTO " + Factory + @"_EMPLOYEE_CUSTOMIZE_HISTORY([EMP_CARD],[EVENT_CODE],[SCAN_DATE],[ACTIVE])
                            VALUES('" + card + "-er', '" + event_code + "', '" + DateTime.Now + "', 1)";
                await new WelfareADO().ExecuteNon2Async(sql);
                return Json(new { code = 3 });
            }
        }
        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> UploadGA(IFormFile file, [FromServices] IHostingEnvironment env)
        {
            try
            {
                if (file == null || file.Length < 1)
                {
                    TempData["UploadGA"] = "Không có nội dung tải lên !";
                    return RedirectToAction(nameof(Manager));
                }
                if (!file.FileName.Contains("Huong dan Welfare.docx"))
                {
                    TempData["UploadGA"] = "Xem lại tên file hoặc định dạng !";
                    return RedirectToAction(nameof(Manager));
                }
                var uploadDirectory = "downloads/";
                var uploadPath = Path.Combine(env.WebRootPath, uploadDirectory);
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                TempData["UploadGA"] = "Cập nhật file hướng dẫn thành công !";
                return RedirectToAction(nameof(Manager));
            }
            catch (Exception)
            {
                TempData["UploadGA"] = "Có lỗi khi cập nhật file hướng dẫn";
                return RedirectToAction(nameof(Manager));
            }
        }
    }
    public class Obj2objCustomize
    {
        public List<TS_EMPLOYEE_CUSTOMIZE> lstEmploySuccess { get; set; }
        public List<TS_EMPLOYEE_CUSTOMIZE> lstEmployError { get; set; }
    }
    public class LAST_CHECK_IN
    {
        public string EMP_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public string EMP_DEPT { get; set; }
        public string EVENT_STANDARD { get; set; }
        public DateTime SCAN_DATE { get; set; }
        public string EVENT_NOTE { get; set; }
        public string EVENT_NAME { get; set; }
        public string EMP_CARD { get; set; }
        public string EVENT_STATUS { get; set; }
        public string EVENT_PCNAME { get; set; }
    }
    public class STT_BY_STANDARD
    {
        public string standard { get; set; }
        public int total { get; set; }
        public int checked1 { get; set; }
        public int notcheck { get; set; }
    }
    public class STT_BY_DEPT
    {
        public string dept { get; set; }
        public int total { get; set; }
        public int checked1 { get; set; }
        public int notcheck { get; set; }
    }
    public class REPORT_BY_STANDARD
    {
        public string standard { get; set; }
        public int total { get; set; }
        public int checked1 { get; set; }
        public int notcheck { get; set; }
        public string ratechecked
        {
            get
            {
                if(total == 0 || checked1 == 0)
                {
                    return 0 + " %";
                }
                return (checked1 * 100 / total) + " %";
            }
        }
        public string ratenotcheck
        {
            get
            {
                if (notcheck == 0 || total == 0)
                {
                    return 0 + " %";
                }
                return (notcheck * 100 / total) + " %";
            }
        }
    }
    public class REPORT_BY_DEPT
    {
        public string dept { get; set; }
        public int total { get; set; }
        public int checked1 { get; set; }
        public int notcheck { get; set; }
        public string notregister { get { return ""; } }
        public string ratechecked
        {
            get
            {
                if (total == 0 || checked1 == 0)
                {
                    return 0 + " %";
                }
                return (checked1 * 100 / total) + " %";
            }
        }
        public string ratenotcheck
        {
            get
            {
                if (notcheck == 0 || total == 0)
                {
                    return 0 + " %";
                }
                return (notcheck * 100 / total) + " %";
            }
        }
        public string ratenotregister { get { return ""; } }
    }
    public class StandardRemain
    {
        public string standard { get; set; }
        public string remain { get; set; }
    }
}
