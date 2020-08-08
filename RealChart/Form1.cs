using MetroFramework.Forms;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealChart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        void LoadData()
        {
            DataTable dt1 = (DataTable)dataGridView1.DataSource;
            string connectionString = @"data source=localhost;
   initial catalog = BondDB; persist security info = True;
            Integrated Security = SSPI; ";
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlCommand cmd = new SqlCommand("select id, name, age, address from dbo.tbl_demo", cn);
                    cmd.Notification = null;
                    SqlDependency sqlDependency = new SqlDependency(cmd);
                    sqlDependency.OnChange += new OnChangeEventHandler(OnDependencyChange);
                    DataTable dt = new DataTable("demo");
                    dt.Load(cmd.ExecuteReader());
                    DataTable dt2 = dt;
                    dataGridView1.DataSource = dt;
                    try
                    {
                        DataRow dr = isNew(dt1, dt2);
                        if (dr != null)
                        {
                            new Notifycation(dr).ShowDialog();
                        }
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    
                }
            }
            catch (Exception)
            {
                dataGridView1.DataSource = null;
            }
        }
        delegate void UpdateData();
        public void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            //remember alter database enable broker service
            SqlDependency sqlDependency = sender as SqlDependency;
            sqlDependency.OnChange -= OnDependencyChange;
            UpdateData updateData = new UpdateData(LoadData);
            this.Invoke(updateData, null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                                   Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            string connectionString = @"data source=localhost;
   initial catalog = BondDB; persist security info = True;
            Integrated Security = SSPI; ";
            SqlClientPermission sqlClientPermission = new SqlClientPermission(System.Security.Permissions.PermissionState.Unrestricted);
            sqlClientPermission.Demand();
            SqlDependency.Start(connectionString);
            LoadData();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string connectionString = @"data source=localhost;
   initial catalog = BondDB; persist security info = True;
            Integrated Security = SSPI; ";
            SqlDependency.Stop(connectionString);
            Application.Exit();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
        }
        private DataRow isNew(DataTable table1, DataTable table2)
        {
            try
            {
                DataTable TableC = table2.AsEnumerable().Where(ra => !table1.AsEnumerable().Any(rb => rb.Field<int>("id") == ra.Field<int>("id"))).CopyToDataTable();
                return TableC.Rows[0];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
