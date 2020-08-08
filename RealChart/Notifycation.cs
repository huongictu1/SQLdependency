using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealChart
{
    public partial class Notifycation : MetroForm
    {
        DataRow drow;
        public Notifycation()
        {
            InitializeComponent();
        }
        public Notifycation(DataRow dr)
        {
            InitializeComponent();
            drow = dr;
        }
        int count = 5;
        private void timer1_Tick(object sender, EventArgs e)
        {
            metroTile1.Text = count.ToString();
            count--;           
            if (count == 0)
            {
                this.Close();
            }
        }

        private void Notifycation_Load(object sender, EventArgs e)
        {
            metroLabel1.Text = drow["name"].ToString();
            metroLabel2.Text = drow["age"].ToString();
            metroLabel3.Text = drow["address"].ToString();
            timer1.Enabled = true;
            timer1.Start();        
        }

        private void Notifycation_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
        }
    }
}
