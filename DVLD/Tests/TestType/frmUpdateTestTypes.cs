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
    public partial class frmUpdateTestTypes : Form
    {
        private clsTestTypes.enTestType _TestTypeID;
        private clsTestTypes _TestType;
        public frmUpdateTestTypes(clsTestTypes.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestTypeID = TestTypeID; 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUpdateTestTypes_Load(object sender, EventArgs e)
        {
            _TestType = clsTestTypes.Find((int)_TestTypeID);

            if (_TestType == null)
            {
                MessageBox.Show("Could not find Test Type with id = " + _TestTypeID.ToString(), "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lbTestTypeID.Text = _TestTypeID.ToString();
            tbTestTypeTitle.Text = _TestType.TestTypeTitle;
            tbTestTypeDescription.Text = _TestType.TestTypeDescription;
            tbTestTypeFees.Text = _TestType.TestTypesFees.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            _TestType.TestTypeID = _TestTypeID;
            _TestType.TestTypeTitle = tbTestTypeTitle.Text.Trim();
            _TestType.TestTypeDescription = tbTestTypeDescription.Text.Trim();
            _TestType.TestTypesFees =Convert.ToDecimal(tbTestTypeFees.Text.Trim());

            if (_TestType.Save())
                MessageBox.Show("Data Saved Successfully.", "Save Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnSave_MouseEnter(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnSave, 88, 31);
        }

        private void btnSave_MouseLeave(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnSave, 86, 29);

        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnClose, 88, 31);

        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnClose, 86, 29);

        }

        private void tbTestTypeFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tbTestTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTestTypeTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbTestTypeTitle, "Title can not be empty.");
            }
            else
                errorProvider1.SetError(tbTestTypeTitle, null);
        }

        private void tbTestTypeFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTestTypeFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbTestTypeFees, "Fees can not be empty.");
            }
            else
                errorProvider1.SetError(tbTestTypeFees, null);

        }

        private void tbTestTypeDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTestTypeDescription.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbTestTypeDescription, "Description can not be empty.");
            }
            else
                errorProvider1.SetError(tbTestTypeDescription, null);

        }
    }
}
