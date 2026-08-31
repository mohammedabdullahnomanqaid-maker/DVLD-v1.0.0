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
    public partial class frmChangePassword : Form
    {
        private int _UserID;
        private clsUsers _User;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

        private void tbCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (tbCurrentPassword.Text.Trim() == "")
            {
                errorProvider1.SetError(tbCurrentPassword, "Current Password can not be blank!");
                e.Cancel = true;
                return;
            }
            if (tbCurrentPassword.Text.Trim() != _User.Password)
            {
                errorProvider1.SetError(tbCurrentPassword, "Current Password is wrong!");
                e.Cancel = true;
                return;
            }
            errorProvider1.SetError(tbCurrentPassword, "");
        }

        private void tbNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (tbNewPassword.Text.Trim() == "")
            {
                errorProvider1.SetError(tbNewPassword, "New Password can not be blank!");
                e.Cancel = true;
                return;
            }
            errorProvider1.SetError(tbNewPassword, "");
        }

        private void tbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (tbNewPassword.Text.Trim() != tbConfirmPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(tbConfirmPassword, "Password Confirmation does not match passwrod !");
                return;
            }
            errorProvider1.SetError(tbConfirmPassword, "");
        }
        private void _ResetDefaultValues()
        {
            tbConfirmPassword.Clear();
            tbNewPassword.Clear();
            tbCurrentPassword.Clear();
            tbCurrentPassword.Focus();
        }
     
        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            _User = clsUsers.FindUserByUserID(_UserID);
            if (_User == null)
            {
                MessageBox.Show("Could not Find User with id = " + _UserID,
                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            uctShowUserInfo1.LoadData(_UserID);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _User.Password = tbConfirmPassword.Text.Trim();
            if (_User.Save())
            {
                MessageBox.Show("Password Changed Successfully.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ResetDefaultValues();
            }
            else
            {
                MessageBox.Show("An Erro Occured, Password did not change.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}
