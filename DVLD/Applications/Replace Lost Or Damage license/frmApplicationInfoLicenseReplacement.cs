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
using static BussinseLayer.clsLicense;

namespace Course19
{
    public partial class frmApplicationInfoLicenseReplacement : Form
    {
        int _LicenseID = -1;
        clsLicense _License;
        public frmApplicationInfoLicenseReplacement()
        {
            InitializeComponent();
        }
        private enIssueReason GetIssueReason()
        {
            if (rbDamagedLicense.Checked)
                return enIssueReason.DamageReplacement;
            else
                return enIssueReason.LostReplacement;
        }
        private int _GetApplicationTypeID()
        {
            //this will decide which type of lost.

            if (rbDamagedLicense.Checked)
                return (int)clsApplications.enApplicationType.ReplaceForDamage;
            else
                return (int)clsApplications.enApplicationType.ReplaceForLost;
        }

        private void _ResetDefaultValues()
        {
           // lbApplicationDate.Text = "[???]";
            lbApplicationFees.Text = "[???]";
           // lbCreatedBy.Text = "[???]";
            lbLRApplicationID.Text = "[???]";
            lbOldLLicenseID.Text = "[???]";
            lbReplacedLicenseID.Text = "[???]";

        }
        private void uctShowLicenseInfoWithFilter1_onLicenseSelected(int obj)
        {
            lkShowLicenseInfo.Enabled = false;
            lbOldLLicenseID.Text = obj.ToString();
            _LicenseID = obj;

            lkShowLicenseHistory.Enabled = (_LicenseID != -1);

            if (_LicenseID == -1)
            {
                btnIssue.Enabled = false;
                _ResetDefaultValues();
                return;
            }

            if (!clsLicense.IsActiveLicense(_LicenseID))
            {
                btnIssue.Enabled = false;
                MessageBox.Show("Selected license is not active ,choose an active license.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnIssue.Enabled = true;


        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lbTitle.Text = "Replacement for Damaged License";
            this.Text = lbTitle.Text.Trim();
            lbApplicationFees.Text = clsApplicationTypes.Find(_GetApplicationTypeID()).ApplicationTypesFees.ToString();

        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lbTitle.Text = "Replacement for Lost License";
            lbApplicationFees.Text = clsApplicationTypes.Find(_GetApplicationTypeID()).ApplicationTypesFees.ToString();
            this.Text = lbTitle.Text.Trim();
        }

        private void frmApplicationInfoLicenseReplacement_Load(object sender, EventArgs e)
        {
            rbDamagedLicense.Checked = true;

            lbApplicationDate.Text = DateTime.Now.ToString();
            lbCreatedBy.Text = clsGlobal.CurrentUser.UserName;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to replace this license ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;

            clsLicense NewLicense = uctShowLicenseInfoWithFilter1.License.Replace(GetIssueReason(),clsGlobal.CurrentUser.UserID);

            if (NewLicense == null)
            {
                MessageBox.Show("Data did not save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _LicenseID = NewLicense.LicenseID;
            btnIssue.Enabled = false;
            lkShowLicenseInfo.Enabled = true;
            lbLRApplicationID.Text = NewLicense.ApplicationID.ToString();
            lbReplacedLicenseID.Text = NewLicense.LicenseID.ToString();
            MessageBox.Show("License Replaced Successfully with ID=" + NewLicense.LicenseID.ToString(), "Successed", MessageBoxButtons.OK, MessageBoxIcon.Information);
      
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

        private void frmApplicationInfoLicenseReplacement_Shown(object sender, EventArgs e)
        {
            uctShowLicenseInfoWithFilter1.FocusOnTbFilter();
        }
    }
}
