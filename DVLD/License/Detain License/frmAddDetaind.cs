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
    public partial class frmAddDetaind : Form
    {
        private int _LicenseID = -1;
        public frmAddDetaind()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void _ResetDefaultValue()
        {
            lkShowLicenseInfo.Enabled = false;
          //  lbCreatedBy.Text = "[???]";
            lbLicenseID.Text = "[???]";
           // lbDetainDate.Text = "[???]";
            lbDetainID.Text= "[???]";
        }
        private bool _IsMatchRules()
        {
            if (!clsLicense.IsActiveLicense(_LicenseID))
            {
                MessageBox.Show("Selected License is not active , select an active license.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (clsDetaind.IsLicenseDetaind(_LicenseID))
            {
                MessageBox.Show("Selected License is already detain , choose another one.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void _SetValuesOfDetainInfo()
        {
            lbLicenseID.Text = _LicenseID.ToString();


            if (_IsMatchRules())
                btnDetain.Enabled = true;
            else
                btnDetain.Enabled = false;
        }

        private void uctShowLicenseInfoWithFilter1_onLicenseSelected(int obj)
        {
            _LicenseID = obj;
            lkShowLicenseHistory.Enabled = (_LicenseID != -1);

            if (_LicenseID==-1)
            {
                _ResetDefaultValue();
                return;
            }

                _SetValuesOfDetainInfo();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to detain this license ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;

            int DetainID = uctShowLicenseInfoWithFilter1.License.Detain(Convert.ToDecimal(tbFineFees.Text.Trim()),clsGlobal.CurrentUser.UserID);

            if (DetainID == -1)
            {
                MessageBox.Show("Detaind did not save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            lkShowLicenseInfo.Enabled = true;
            lbDetainID.Text = DetainID.ToString();
            btnDetain.Enabled = false;

            MessageBox.Show("License Detaind Successfully with ID=" + DetainID.ToString(), "Successed", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void frmAddDetaind_Shown(object sender, EventArgs e)
        {
            uctShowLicenseInfoWithFilter1.FocusOnTbFilter();
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

        private void tbFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (tbFineFees.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(tbFineFees,"This field is required!");
            }    
            else
                errorProvider1.SetError(tbFineFees, "");

        }

        private void frmAddDetaind_Load(object sender, EventArgs e)
        {
            lbCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            lbDetainDate.Text =clsFormat.DateToShort(DateTime.Now);

        }
    }
}
