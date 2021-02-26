using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Welfare.Models;

namespace Welfare.Data
{
    public class WelfareContext: DbContext
    {
        public WelfareContext(DbContextOptions<WelfareContext> options): base(options)
        {

        }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<CardCheck> CardChecks { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<DeliveryDate> DeliveryDates { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeBus> EmployeeBuses { get; set; }
        public DbSet<EmployeeDisease> EmployeeDiseases { get; set; }
        public DbSet<EmployeeGift> EmployeeGifts { get; set; }
        public DbSet<EmployeeHoliday> EmployeeHolidays { get; set; }
        public DbSet<EmployeeTicket> EmployeeTickets { get; set; }
        public DbSet<EmployeeTour> EmployeeTours { get; set; }
        public DbSet<EventDetail> EventDetails { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<WelfareSystem> WelfareSystems { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<EVENT_SETTING> EVENT_SETTING { get; set; }
        public DbSet<TS_EMPLOYEE_CUSTOMIZE> TS_EMPLOYEE_CUSTOMIZE { get; set; }
        public DbSet<TL_EMPLOYEE_CUSTOMIZE> TL_EMPLOYEE_CUSTOMIZE { get; set; }
        public DbSet<QV_EMPLOYEE_CUSTOMIZE> QV_EMPLOYEE_CUSTOMIZE { get; set; }
        public DbSet<TS_EMPLOYEE_CUSTOMIZE_HISTORY> TS_EMPLOYEE_CUSTOMIZE_HISTORY { get; set; }
        public DbSet<TL_EMPLOYEE_CUSTOMIZE_HISTORY> TL_EMPLOYEE_CUSTOMIZE_HISTORY { get; set; }
        public DbSet<QV_EMPLOYEE_CUSTOMIZE_HISTORY> QV_EMPLOYEE_CUSTOMIZE_HISTORY { get; set; }

        public DbSet<TS_EMPLOYEE_GIFT> TS_EMPLOYEE_GIFT { get; set; }
        public DbSet<TL_EMPLOYEE_GIFT> TL_EMPLOYEE_GIFT { get; set; }
        public DbSet<QV_EMPLOYEE_GIFT> QV_EMPLOYEE_GIFT { get; set; }
        public DbSet<TS_EMPLOYEE_GIFT_HISTORY> TS_EMPLOYEE_GIFT_HISTORY { get; set; }
        public DbSet<TL_EMPLOYEE_GIFT_HISTORY> TL_EMPLOYEE_GIFT_HISTORY { get; set; }
        public DbSet<QV_EMPLOYEE_GIFT_HISTORY> QV_EMPLOYEE_GIFT_HISTORY { get; set; }
    }
}
