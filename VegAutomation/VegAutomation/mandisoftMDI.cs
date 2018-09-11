using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VegAutomation
{
    public partial class mandisoftMDI : Form
    {
        frmunits units = new frmunits();
        public mandisoftMDI()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mandisoftMDI_Load(object sender, EventArgs e)
        {
            
            units.MdiParent = this;
        }

        private void unitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            units.Show();
        }
    }
}
