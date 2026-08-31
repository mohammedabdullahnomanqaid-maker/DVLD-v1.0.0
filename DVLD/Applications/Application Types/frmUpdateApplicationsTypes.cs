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
    public partial class frmUpdateApplicationsTypes : Form
    {
        private int _ApplicationTypeID=-1;
        private clsApplicationTypes _ApplicationTypes;
        public frmUpdateApplicationsTypes(int ApplicationTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = ApplicationTypeID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUpdateApplicationsTypes_Load(object sender, EventArgs e)
        {
            _ApplicationTypes = clsApplicationTypes.Find(_ApplicationTypeID);
            if (_ApplicationTypes == null)
            {
                MessageBox.Show("No ApplicationTypes with ApplicationTypeID = " + _ApplicationTypeID.ToString(), "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            //incase find perform this upload
            tbApplicationTypeTitle.Text = _ApplicationTypes.ApplicationTypesTitle;
            tbApplicationTypeFees.Text = Convert.ToString(_ApplicationTypes.ApplicationTypesFees);
            lbApplicationTypeID.Text = _ApplicationTypeID.ToString();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _ApplicationTypes.ApplicationTypeID =Convert.ToInt32(lbApplicationTypeID.Text.Trim());
            _ApplicationTypes.ApplicationTypesTitle = tbApplicationTypeTitle.Text.Trim();
            _ApplicationTypes.ApplicationTypesFees =Convert.ToDecimal(tbApplicationTypeFees.Text.Trim());

            if (_ApplicationTypes.Save())
            MessageBox.Show("Data Saved Successfully.", "Savd Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void tbApplicationTypeFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void btnSave_MouseEnter(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnSave, 88, 31);

        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnSave, 86, 29);

        }

        private void tbApplicationTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbApplicationTypeTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbApplicationTypeTitle, "Title can not be empty.");
            }
            else
                errorProvider1.SetError(tbApplicationTypeTitle, null);
        }

        private void tbApplicationTypeFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbApplicationTypeFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbApplicationTypeFees, "Fees can not be empty.");
            }
            else
                errorProvider1.SetError(tbApplicationTypeFees, null);

        }
    }
}
