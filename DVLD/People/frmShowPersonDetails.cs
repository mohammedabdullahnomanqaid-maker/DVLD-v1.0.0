using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Course19
{
    public partial class frmShowPersonDetails : Form
    {
       

        public frmShowPersonDetails(int PersonID)
        {
            InitializeComponent();
            uctShowPersonDetails1.LoadData(PersonID);

        }

        public frmShowPersonDetails(string NationalNo)
        {
            InitializeComponent();
            uctShowPersonDetails1.LoadData(NationalNo);

        }
        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnClose, 88,31);
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnClose, 86, 29);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
          
        }

    }
}
