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
    public partial class frmIssueDrivingLicenseForFirstTime : Form
    {
        clsLicense _License;
        private int _LocalDrivingLicenseApplicationID = -1;
        public frmIssueDrivingLicenseForFirstTime(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmIssueDrivingLicenseForFirstTime_Load(object sender, EventArgs e)
        {
            tbNote.Focus();
            clsLocalDrivingLicenseApplication LocalDrivingLicesneApplication = clsLocalDrivingLicenseApplication.Find(_LocalDrivingLicenseApplicationID);
            if (LocalDrivingLicesneApplication == null)
            {
                MessageBox.Show("No Applicaiton with ID=" + _LocalDrivingLicenseApplicationID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if (!LocalDrivingLicesneApplication.IsPassAllTest())
            {
                MessageBox.Show("Person Should Pass All Tests First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            int LicenseID = LocalDrivingLicesneApplication.GetActiveLicenseID();
            if (LicenseID!=-1)
            {

                MessageBox.Show("Person already has License before with License ID=" + LicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            uctShowLocalDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(_LocalDrivingLicenseApplicationID);
            int LicenseID = _LocalDrivingLicenseApplication.IssuedLicense(clsGlobal.CurrentUser.UserID,tbNote.Text.Trim());
            if (LicenseID!=-1)
            {
                MessageBox.Show("License issued successfully with license ID = " + LicenseID.ToString(), "Successed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
                MessageBox.Show("Data did not Saved.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
