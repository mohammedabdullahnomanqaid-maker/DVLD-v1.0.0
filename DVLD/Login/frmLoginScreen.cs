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
using System.IO;

namespace Course19
{
    public partial class frmLoginScreen : Form
    {

        public frmLoginScreen()
        {
            InitializeComponent();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnLogin, 77, 32);

        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnLogin, 75, 30);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUsers _User = clsUsers.FindUserInfoByUsernameAndPassword(tbUserName.Text.Trim(), tbPassword.Text.Trim());

            if(_User!=null)
            {
                if (chkRemeberMe.Checked)
                {
                    Properties.Settings.Default.Save();
                    //store username and password
                    clsGlobal.RemeberUsernameAndPassword(tbUserName.Text.Trim(), tbPassword.Text.Trim());
                }
                else
                {
                    //store empty username and password 
                    clsGlobal.RemeberUsernameAndPassword("", "");

                }

                if (!_User.IsActive)
                {
                    tbUserName.Focus();
                    MessageBox.Show("Your accound is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsGlobal.CurrentUser = _User;
                this.Hide();
                frmDashboard frm = new frmDashboard(this);
                frm.ShowDialog();
                return;
            }


            tbUserName.Focus();
            MessageBox.Show("Invalid Username/Password!", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
           
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void frmLoginScreen_Load(object sender, EventArgs e)
        {
            string Username = string.Empty;
            string Password = string.Empty;

            if (clsGlobal.GetStoredCredential(ref Username, ref Password))
            {
                tbUserName.Text = Username;
                tbPassword.Text = Password;
                chkRemeberMe.Checked = true;
            }
            else
                chkRemeberMe.Checked = false;
        }
    }
}
