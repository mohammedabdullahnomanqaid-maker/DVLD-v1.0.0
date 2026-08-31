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
    public partial class frmRenewedLicense : Form
    {
        decimal ApplicationFees=0;
         decimal LicenseFees=0;
        int _LicenseID = -1;
        public frmRenewedLicense()
        {
            InitializeComponent();
        }

        private void LoadApplicationInfo()
        {
            clsLicense License = clsLicense.Find(_LicenseID);
            if (License == null)
            {
              _ResetDefaultValue();
                return;
            }

             LicenseFees      = uctShowLicenseInfoWithFilter1.License.PaidFees;

            lbExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(uctShowLicenseInfoWithFilter1.License.LicenseClassInfo.DefaultValidityLength));
            lbLicenseFees.Text =LicenseFees.ToString();
            lbRenewedLicenseID.Text = "[???]";
            lbRLApplicationID.Text = "[???]";
            lbTotalFees.Text = (ApplicationFees+LicenseFees).ToString();
            lbOldLLicenseID.Text = _LicenseID.ToString();
            tbNotes.Text = uctShowLicenseInfoWithFilter1.License.Notes;

            if (IsMatchRules())
                btnIssue.Enabled = true;
            else
                btnIssue.Enabled = false;
        }
        private bool IsMatchRules()
        {
            if (!uctShowLicenseInfoWithFilter1.License.IsLicenseExpired())
            {
                MessageBox.Show("Selected License is not yet expired,it will expire on " + uctShowLicenseInfoWithFilter1.License.ExpirationDate.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!clsLicense.IsActiveLicense(_LicenseID))
            {
                MessageBox.Show("Selected License is not active,choose another one", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
      
        private void uctShowLicenseInfoWithFilter1_onLicenseSelected(int obj)
        {
            _LicenseID = obj;
            lkShowLicenseHistory.Enabled = (_LicenseID!=-1);
            lkShowLicenseInfo.Enabled = false;
            LoadApplicationInfo();

        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to renew this license ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;

            clsLicense NewLicense = uctShowLicenseInfoWithFilter1.License.RenewLicense(tbNotes.Text.Trim(),clsGlobal.CurrentUser.UserID);
           if(NewLicense==null)
            {
                MessageBox.Show("Error : Data did not save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnIssue.Enabled = false;
            lkShowLicenseInfo.Enabled = true;
            lbRLApplicationID.Text = NewLicense.ApplicationID.ToString();
            lbRenewedLicenseID.Text = NewLicense.LicenseID.ToString();
            MessageBox.Show("License Renewed Successfully with ID=" + NewLicense.LicenseID.ToString(), "Successed", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void lkShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowLicenseHistory(clsBPeople.GetPersonIDByLicenseID(_LicenseID));
            frm.ShowDialog();
        }

        private void lkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }

        private void frmRenewedLicense_Shown(object sender, EventArgs e)
        {
            uctShowLicenseInfoWithFilter1.FocusOnTbFilter();
        }
        private void _ResetDefaultValue()
        {
            btnIssue.Enabled = false;
           // lbApplicationDate.Text = "[???]";
            //lbApplicationFees.Text = "[???]";
            //lbCreatedBy.Text = "[???]";
            lbExpirationDate.Text = "[???]";
            //lbIssueDate.Text = "[???]";
            lbLicenseFees.Text = "[???]";
            lbRenewedLicenseID.Text = "[???]";
            lbRLApplicationID.Text = "[???]";
            lbTotalFees.Text = "[???]";
            lbOldLLicenseID.Text = "[???]";

        }

        private void frmRenewedLicense_Load(object sender, EventArgs e)
        {
            lbCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            lbIssueDate.Text =clsFormat.DateToShort(DateTime.Now);
            lbApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            ApplicationFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.RenewApplication).ApplicationTypesFees;
            lbApplicationFees.Text = ApplicationFees.ToString();

        }
    }
}
