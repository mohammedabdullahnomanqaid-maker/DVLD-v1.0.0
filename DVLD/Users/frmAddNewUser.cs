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
    public partial class frmAddNewUser : Form
    {
        private int _UserID = -1;
        private int _PersonID = -1;


        private enum enMode { AddMode,UpdateMode};
        private enMode _Mode = enMode.UpdateMode;
        public frmAddNewUser()
        {
            InitializeComponent();
            _Mode = enMode.AddMode;
        }

        public frmAddNewUser(int UserID)
        {
            InitializeComponent();
            _UserID = UserID ;
            _Mode = enMode.UpdateMode;
        }

        private clsUsers _User;
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _User.UserName = tbUserName.Text.Trim();
            _User.Password = tbPassword.Text.Trim();
            _User.PersonID = uctSearchAndShowPersonCard1.PersonID;
            _User.IsActive = chkIsActive.Checked;



            if (_User.Save())
            {
                lbAddUpdateUser.Text = "Update User";
                this.Text = "Update User";
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _Mode = enMode.UpdateMode;
                lbUserID.Text = _User.UserID.ToString();
            }
            else
                MessageBox.Show("Error : Data is not saved successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void tbUserName_Validating(object sender, CancelEventArgs e)
        {
            if (tbUserName.Text.Trim() == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(tbUserName, "UserName can not be blank!");
                return;
            }

            if (_Mode == enMode.AddMode)
            {
                if (clsUsers.IsUserNameExistByUsername(tbUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(tbUserName, "This User is already in use");
                    return;
                }
                else
                {
                    errorProvider1.SetError(tbUserName, "");
                }
            }
            else
            {
                //incase update mode
                if(tbUserName.Text.Trim() != _User.UserName)
                {
                    if (clsUsers.IsUserNameExistByUsername(tbUserName.Text.Trim()))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(tbUserName, "This User is already in use");
                        return;
                    }
                    else
                    {
                        errorProvider1.SetError(tbUserName, "");
                    }
                }
            }
           
        }

        private void tbConfirmPassword_Validating_1(object sender, CancelEventArgs e)
        {
            if (tbPassword.Text.Trim() != tbConfirmPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(tbConfirmPassword, "Password Confirmation does not match passwrod !");
                return;
            }
            errorProvider1.SetError(tbConfirmPassword, "");
        }

        private void tbPassword_Validating(object sender, CancelEventArgs e)
        {
            if (tbPassword.Text.Trim() == "")
            {
                errorProvider1.SetError(tbPassword, "Password can not be blank!");
                e.Cancel = true;
                return;
            }
            errorProvider1.SetError(tbPassword, "");
        }

        private void btnNext_Click_1(object sender, EventArgs e)
        {
            //incase of update mode
            if (_Mode == enMode.UpdateMode)
            {
                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
                tabControl1.SelectedTab = tabControl1.TabPages["tpLoginInfo"];
                return;
            }
            //incase of add new mode
            if (uctSearchAndShowPersonCard1.PersonID != -1)
            {
                if (clsUsers.IsUserExistByPersonID(uctSearchAndShowPersonCard1.PersonID))
                {
                    MessageBox.Show("Selected Person already has a user, choose another one .", "Select Another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    tpLoginInfo.Enabled = true;
                    btnSave.Enabled = true;
                    
                    tabControl1.SelectedIndex = 1;
                }
            }
            else
            {
                MessageBox.Show("Please Select Person !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                uctSearchAndShowPersonCard1.FilterFocus();
            }

        }

        private void _LoadData()
        {
            uctSearchAndShowPersonCard1.FilterEnable = false;
            _User = clsUsers.FindUserByUserID(_UserID);
            if (_User == null)
            {
                MessageBox.Show("No User with PersonID = " +_PersonID , "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            lbUserID.Text =_User.UserID.ToString();
            tbUserName.Text = _User.UserName;
            tbPassword.Text = _User.Password;
            tbConfirmPassword.Text = _User.Password;
            chkIsActive.Checked = _User.IsActive;
            uctSearchAndShowPersonCard1.LoadPersonInfo(_User.PersonID);
        }

        private void _ResetDefaultValues()
        {
            if (_Mode == enMode.AddMode)
            {
                lbAddUpdateUser.Text = "Add New User";
                this.Text = "Add New User";
                btnSave.Enabled = false ;
                tpLoginInfo.Enabled = false;
                _User = new clsUsers();
                uctSearchAndShowPersonCard1.FilterEnable = true;
                uctSearchAndShowPersonCard1.FilterFocus();
            }
            else
            {
                lbAddUpdateUser.Text = "Update User";
                this.Text = "Update User";
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;
                uctSearchAndShowPersonCard1.FilterFocus();
            }
            tbUserName.Clear();
            tbPassword.Clear();
            tbConfirmPassword.Clear();
            chkIsActive.Checked = true;
        }
        private void frmAddNewUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.UpdateMode)
                _LoadData();
        }

        private void btnNext_MouseEnter(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnNext, 88, 31);
        }

        private void btnNext_MouseLeave(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnNext, 86, 29);
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
            clsUI.MauseEnterLeave(btnClose, 86,29);

        }
    }
}
