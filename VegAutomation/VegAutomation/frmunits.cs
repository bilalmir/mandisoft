using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VegAutomation.AppUtlity;
using System.Data.SqlClient;

namespace VegAutomation
{
    public partial class frmunits : Form
    {
        public frmunits()
        {
            InitializeComponent();
            loadUnitList();
        }

        private void frmunits_Load(object sender, EventArgs e)
        {
           
            this.WindowState = FormWindowState.Maximized;
            loadUnitList();
        }
        private void loadUnitList()
        {
            string selectQuery = "select * from tblunits";
            DataSet ds = DBAccess.GetData(selectQuery);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dtgrdUnits.DataSource = ds.Tables[0];

                }
            }
        }
    }
}
