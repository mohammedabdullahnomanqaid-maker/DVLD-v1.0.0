using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussinseLayer;

namespace Course19
{
    public partial class frmShowLicenseHistory : Form
    {
        int _PersonID = -1;
        //public frmShowLicenseHistory()
        //{
        //    InitializeComponent();
        //}
        public frmShowLicenseHistory(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }
        private void frmShowLicenseHistory_Load(object sender, EventArgs e)
        {
            if (_PersonID != -1)
            {
                uctShowPersonDetails1.LoadData(_PersonID);
                uctDriverLicenses1.LoadInfoByPersonID(_PersonID);
            }
        }

    }
}

