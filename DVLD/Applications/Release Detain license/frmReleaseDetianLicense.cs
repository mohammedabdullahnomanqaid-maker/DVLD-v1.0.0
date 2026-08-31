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
    public partial class frmReleaseDetianLicense : Form
    {
        private int _LicenseID = -1;
        decimal ApplicationFees = 0;
        public frmReleaseDetianLicense()
        {
            InitializeComponent();

        }
        public frmReleaseDetianLicense(int LicenseID)
        {
            InitializeComponent();
            _LicenseID = LicenseID;
            uctShowLicenseInfoWithFilter1.FilterEnable = false;
            uctShowLicenseInfoWithFilter1.LoadInfo(LicenseID);
            SetValues();
        }
        private void _ResetDefaultValue()
        {
            lkShowLicenseInfo.Enabled = false;
            //lbApplicationFees.Text = "[$$$]";
            lbFineFees.Text = "[$$$]";
            lbTotalFees.Text = "[$$$]";
            //lbDetainDate.Text = "[??/??/????]";
            lbApplicationID.Text = "[???]";
            lbDetaindID.Text = "[???]";
            lbLicenseID.Text = "[???]";
           // lbCreatedBy.Text = "[???]";
        }
     private void SetValues()
        {
            lkShowLicenseHistory.Enabled = (_LicenseID != -1);

            if (_LicenseID == -1)
            {
                lkShowLicenseInfo.Enabled = false;
                _ResetDefaultValue();
                return;
            }
            if (!uctShowLicenseInfoWithFilter1.License.IsActive)
            {
                MessageBox.Show("Selected License is not detaind ,choose another one!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!clsDetaind.IsLicenseDetaind(_LicenseID))
            {
                MessageBox.Show("Selected License is not detaind ,choose another one!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal FineFees = uctShowLicenseInfoWithFilter1.License.DetainInfo.FineFees;
            lbFineFees.Text = FineFees.ToString();
            lbTotalFees.Text = (FineFees + ApplicationFees).ToString();

            lbApplicationID.Text = "[???]";
            lbDetaindID.Text = uctShowLicenseInfoWithFilter1.License.DetainInfo.DetainID.ToString();
            lbLicenseID.Text = uctShowLicenseInfoWithFilter1.License.LicenseID.ToString();



            btnRelease.Enabled = true;


        }
        private void uctShowLicenseInfoWithFilter1_onLicenseSelected(int obj)
        {
            _LicenseID = obj;
            SetValues();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to renew this license ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;

            int ApplicationID = -1;

            bool IsRelease = uctShowLicenseInfoWithFilter1.License.Release(clsGlobal.CurrentUser.UserID,ref ApplicationID);
            if (IsRelease)
            {
                lkShowLicenseInfo.Enabled = true;
                lbApplicationID.Text = uctShowLicenseInfoWithFilter1.License.ApplicationID.ToString();
                btnRelease.Enabled = false;
                MessageBox.Show("Detaind License Released Successfully.", "Successed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            MessageBox.Show("License did not Release", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        

        }

        private void lkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }

        private void lkShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowLicenseHistory(clsBPeople.GetPersonIDByLicenseID(_LicenseID));
            frm.ShowDialog();
        }

        private void frmReleaseDetianLicense_Shown(object sender, EventArgs e)
        {
            uctShowLicenseInfoWithFilter1.FocusOnTbFilter();
        }
    

        private void frmReleaseDetianLicense_Load(object sender, EventArgs e)
        {
             ApplicationFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.ReleaseDetaind).ApplicationTypesFees;
            lbApplicationFees.Text = ApplicationFees.ToString();
            lbDetainDate.Text = clsFormat.DateToShort(DateTime.Now);
            lbCreatedBy.Text = clsGlobal.CurrentUser.UserName;

        }
    }
}
