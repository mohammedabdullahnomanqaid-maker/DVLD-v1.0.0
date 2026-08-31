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
    public partial class frmAddInternationalLicense : Form
    {
        private int _LicenseID = -1;
        public frmAddInternationalLicense()
        {
            InitializeComponent();

        }
        public frmAddInternationalLicense(int LicenseID)
        {
            InitializeComponent();
            _LicenseID = LicenseID;
            uctShowLicenseInfoWithFilter1.FilterEnable = false;
            uctShowLicenseInfoWithFilter1.LoadInfo(LicenseID);

             lkShowLicenseHistory.Enabled = (_LicenseID!=-1);

            LoadInternationalLicenseInfo(_LicenseID);

            if (_CanMakeInternationalLicense(_LicenseID))
                btnIssue.Enabled = true;
        }
        private bool _CanMakeInternationalLicense(int LicenseID)
        {
            int InternationalLicenseID = -1;

            lkShowLicenseInfo.Enabled = false;

            clsLicense _License = uctShowLicenseInfoWithFilter1.License;
            if (_License == null)
            {
                btnIssue.Enabled = false;
                _ResetDefaultValue();
                return false;
            }

            if ((InternationalLicenseID = clsInternationalLicense.GetInternationalLicenseIDIfExist(LicenseID)) != -1)
            {
                MessageBox.Show("Person already has an active International license with ID = " + InternationalLicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lkShowLicenseInfo.Enabled = true;
                return false;
            }
            if (_License.LicenseClassID != 3)
            {
                MessageBox.Show("Selected license should be Class 3 ,select another one !", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (_License.ExpirationDate.CompareTo(DateTime.Now) < 0)
            {
                MessageBox.Show("Selected License should be renew before ", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        private void uctShowLicenseInfoWithFilter1_onLicenseSelected(int obj)
        {
            _LicenseID = obj;
            lkShowLicenseHistory.Enabled = (_LicenseID!=-1);

            LoadInternationalLicenseInfo(_LicenseID);

            if (_CanMakeInternationalLicense(_LicenseID))
                btnIssue.Enabled = true;

        }
        private void _ResetDefaultValue()
        {
            lkShowLicenseInfo.Enabled = false;
            lbApplicationDate.Text = "[???]";
            lbCreatedBy.Text = "[???]";
            lbExpirationDate.Text = "[???]";
            lbFees.Text = "[???]";
            lbILApplicationID.Text = "[???]";
            lbILLicenseID.Text = "[???]";
            lbIssueDate.Text = "[???]";
            lbLLicenseID.Text = "[???]";
        }
        public void LoadInternationalLicenseInfo(int LicenseID)
        {
            clsLicense _License = clsLicense.Find(LicenseID);
            if (_License == null)
            {
                _ResetDefaultValue();
                return;
            }

            lbApplicationDate.Text = DateTime.Now.ToString();
            lbIssueDate.Text = DateTime.Now.ToString();
            lbExpirationDate.Text = DateTime.Now.AddYears(1).ToString();
            lbCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            lbLLicenseID.Text = LicenseID.ToString();
            lbFees.Text = Convert.ToDecimal(clsApplicationTypes.Find(Convert.ToInt32(clsApplications.enApplicationType.RetakeTest)).ApplicationTypesFees).ToString();

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the license ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;

            clsInternationalLicense _InternationalLicense = new clsInternationalLicense();

            _InternationalLicense.CreatedByUser = clsGlobal.CurrentUser.UserID;
            _InternationalLicense.PaidFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.NewInternational).ApplicationTypesFees;
            _InternationalLicense.ApplicationPersonID = uctShowLicenseInfoWithFilter1.License.DriverInfo.PersonInfo.PersonID ;


            _InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _InternationalLicense.DriverID = uctShowLicenseInfoWithFilter1.License.DriverID;
            _InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            _InternationalLicense.IsActive = true;
            _InternationalLicense.IssuedUsingLocalLicense = _LicenseID;

  
         
         

            if (_InternationalLicense.Save())
            {
                lkShowLicenseInfo.Enabled = true;
                lbILApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
                lbILLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
                MessageBox.Show("International License issued Successfully with ID = "+_InternationalLicense.InternationalLicenseID.ToString(), "Successed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Data did not Save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            btnIssue.Enabled = false;
        }

        private void frmAddInternationalLicense_Shown(object sender, EventArgs e)
        {
            uctShowLicenseInfoWithFilter1.FocusOnTbFilter();
            //we call it here because it focus after complete build the user control and form.
        }

        private void lkShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            Form frm = new frmShowLicenseHistory(uctShowLicenseInfoWithFilter1.License.DriverInfo.PersonInfo.PersonID);
            frm.ShowDialog();
        }

        private void lkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowInternationalLicenseInfo(clsInternationalLicense.GetInternationalLicenseIDIfExist(_LicenseID));
            frm.ShowDialog();
        }
    }
}
