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
    public partial class uctShowUserInfo : UserControl
    {
        private int _UserID = -1;
        private clsUsers _User;
        public uctShowUserInfo()
        {
            InitializeComponent();
        }
        public int UserID
        {
            get { return _UserID; }
        }
        public clsUsers UserInfo
        {
            get { return _User; }
        }

        public void LoadData(int UserID)
        {
          _User= clsUsers.FindUserByUserID(UserID);
            if (_User == null)
            {
                _ResetPersonInfo();
                MessageBox.Show("No User with User ID = " + UserID, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            _FillUserInfo();
        }

        private void _FillUserInfo()
        {
            lbUserID.Text = _User.UserID.ToString();
            lbUsername.Text = _User.UserName;
            _UserID = _User.UserID;
            if (_User.IsActive)
                lbIsActive.Text = "Yes";
            else
                lbIsActive.Text = "No";

            uctShowPersonDetails1.LoadData(_User.PersonID);

        }

        private void _ResetPersonInfo()
        {
            lbUserID.Text = "[????]";
            lbUsername.Text = "[????]";
            lbIsActive.Text = "[????]"; 
            uctShowPersonDetails1.ResetPersonInfo();
        }
    }
}
