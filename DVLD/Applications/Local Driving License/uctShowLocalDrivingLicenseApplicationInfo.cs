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
    public partial class uctShowLocalDrivingLicenseApplicationInfo : UserControl
    {
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _LicenseID = -1;
        private int _LocalDrivingLicenseApplicationID;
        public int LocalDrivingLicenseApplicationID
        {
            get { return _LocalDrivingLicenseApplicationID; }
        }

       
        public clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication
        {
            get { return _LocalDrivingLicenseApplication; }
        }
        public uctShowLocalDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }
       
        private void _ResetDefaultValue()
        {
            lbDLAppID.Text = "[???]";
            lbPassedTest.Text = "[???]";
            lbAppliedFor.Text = "[???]";
            uctApplicationBasicInfo1.ResetDefaultValue();
        }
        private void _FillLocalDrivingLicenseApplicationInfo()
        {


            lbDLAppID.Text = _LocalDrivingLicenseApplicationID.ToString();
            lbPassedTest.Text = _LocalDrivingLicenseApplication.GetTestPassedCount().ToString() + '/' + '3';
            lbAppliedFor.Text = clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName;
            //here we start fill application object and after that use it in localDrivingLicenseApplication because there is inhirtance
            uctApplicationBasicInfo1.LoadApplicationBasicInfoByApplicationID(_LocalDrivingLicenseApplication.ApplicationID);
            //my note : this will check if there is the same PersonID and same Class and active show link of show license
            _LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();
            //incase there is license enable the show link.
            lkShowLicenseInfo.Enabled = (_LicenseID!=-1);
        }
        public void LoadApplicationInfoByLocalDrivingAppID(int LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(LocalDrivingLicenseApplicationID);
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            if (_LocalDrivingLicenseApplication == null)
            {
                _ResetDefaultValue();
                MessageBox.Show("L.D.L.App ID =" + LocalDrivingLicenseApplicationID.ToString() + " Not Found !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillLocalDrivingLicenseApplicationInfo();
        }
        private void lkViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowPersonDetails(_LocalDrivingLicenseApplication.ApplicationPersonID);
            frm.ShowDialog();
        }

        private void lkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            Form frm = new frmShowLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }
    }
}
