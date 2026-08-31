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
using System.Data;

namespace Course19
{
    public partial class uctDriverLicenses : UserControl
    {
        private int _DriverID = -1;
        static private DataTable _dtLocalDrivingLicense;
        static private DataTable _dtinternationalDrivingLicense;
        public uctDriverLicenses()
        {
            InitializeComponent();
        }
        public void LoadInfoByDriverID(int DriverID)
        {
          
                _DriverID =DriverID;
           
            _LoadLocalDrivingLicenseInfo();
            _LoadInternationalDrivingLicenseInfo();
        }
        public void LoadInfoByPersonID(int PersonID)
        {
            clsDrivers Driver = clsDrivers.FindByPersonID(PersonID);
            if (Driver != null)
            {
                _DriverID = Driver.DriverID;
            }
            _LoadLocalDrivingLicenseInfo();
            _LoadInternationalDrivingLicenseInfo();
        }
        private void _LoadInternationalDrivingLicenseInfo()
        {
            _dtLocalDrivingLicense = clsInternationalLicense.GetAllInternationalLicenseByDriverID(_DriverID);
            dgvInternational.DataSource = _dtLocalDrivingLicense;
            if (dgvInternational.Rows.Count > 0)
            {
                dgvInternational.Columns[0].HeaderText = "Int.License ID";
                dgvInternational.Columns[0].Width = 110;

                dgvInternational.Columns[1].HeaderText = "Application ID";
                dgvInternational.Columns[1].Width = 120;

                dgvInternational.Columns[2].HeaderText = "License ID";
                dgvInternational.Columns[2].Width = 85;

                dgvInternational.Columns[3].HeaderText = "Issue Date";
                dgvInternational.Columns[3].Width = 120;

                dgvInternational.Columns[4].HeaderText = "Expiration Date";
                dgvInternational.Columns[4].Width = 120;

                dgvInternational.Columns[5].HeaderText = "Is Active";
                dgvInternational.Columns[5].Width = 80;
            }
            lbRecordInternational.Text = dgvInternational.Rows.Count.ToString();

        }

        private void _LoadLocalDrivingLicenseInfo()
        {
         _dtinternationalDrivingLicense = clsLicense.GetLicenseInfoOfHistoryByDriverID(_DriverID);

            dgvLocalLicense.DataSource = _dtinternationalDrivingLicense;

            if (dgvLocalLicense.Rows.Count > 0)
            {
                dgvLocalLicense.Columns[0].Width = 50;
                dgvLocalLicense.Columns[0].HeaderText = "Lic.ID";

                dgvLocalLicense.Columns[1].Width = 55;
                dgvLocalLicense.Columns[1].HeaderText = "App.ID";

                dgvLocalLicense.Columns[2].Width = 197;
                dgvLocalLicense.Columns[2].HeaderText = "Class Name";

                dgvLocalLicense.Columns[3].Width = 129;
                dgvLocalLicense.Columns[3].HeaderText = "Issue Date";

                dgvLocalLicense.Columns[4].Width = 130;
                dgvLocalLicense.Columns[4].HeaderText = "Expiration Date";

                dgvLocalLicense.Columns[5].Width = 73;
                dgvLocalLicense.Columns[5].HeaderText = "Is Active";
            }
            lbRecordLocal.Text = dgvLocalLicense.Rows.Count.ToString();

        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowLicenseInfo((int)dgvLocalLicense.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
            Form frm = new frmShowInternationalLicenseInfo((int)dgvInternational.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
    }
}
