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
    public partial class frmManagementUsers : Form
    {
        private static DataTable _dtAllUsers = clsUsers.GetAllUsers();
        
        private DataTable _dtUser=_dtAllUsers.DefaultView.ToTable(false,"UserID","PersonID"
            ,"FullName","UserName","IsActive");

      private void  _RefreshDGV()
        {
              DataTable _dtAllUsers = clsUsers.GetAllUsers();

         _dtUser = _dtAllUsers.DefaultView.ToTable(false, "UserID", "PersonID"
            , "FullName", "UserName", "IsActive");

            dgvUsers.DataSource = _dtUser;
            lbRecords.Text = dgvUsers.Rows.Count.ToString();
        }
        public frmManagementUsers()
        {
            InitializeComponent();
        }

        private void frmManagementUsers_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            dgvUsers.DataSource = _dtUser;
            if (dgvUsers.Rows.Count > 0)
            {
                dgvUsers.Columns["UserID"].HeaderText = "User ID";
                dgvUsers.Columns["UserID"].Width = 100;

                dgvUsers.Columns["PersonID"].HeaderText = "Person ID";
                dgvUsers.Columns["PersonID"].Width = 100;

                dgvUsers.Columns["FullName"].HeaderText = "Full Name";
                dgvUsers.Columns["FullName"].Width = 300;

                dgvUsers.Columns["UserName"].HeaderText = "UserName";
                dgvUsers.Columns["UserName"].Width = 120;

                dgvUsers.Columns["IsActive"].HeaderText = "Is Active";
                dgvUsers.Columns["IsActive"].Width = 90;

            }
            lbRecords.Text = dgvUsers.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnClose, 88, 31);

        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnClose, 86, 29);

        }

        private void picAddUser_MouseEnter(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(picAddUser, 52, 56);
           
        }

        private void picAddUser_MouseLeave(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(picAddUser, 49, 53);
        }

        private void picAddUser_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddNewUser();
            frm.ShowDialog();

            _RefreshDGV();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

            if(cbFilterBy.Text=="Is Active")
            {
                cbIsActive.Visible = true;
                tbFilter.Visible = false;
                cbIsActive.Focus();
                cbIsActive.SelectedIndex = 0;
            }
            else
            {
                tbFilter.Visible = (cbFilterBy.Text != "None");
                cbIsActive.Visible = false;

                tbFilter.Focus();
                tbFilter.Clear();
                
            }

        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            string ColumnFilter = string.Empty;


            switch (cbFilterBy.Text)
            {
                case "User ID":
                    ColumnFilter = "UserID";
                    break;

                case "Person ID":
                    ColumnFilter = "PersonID";
                    break;

                case "UserName":
                    ColumnFilter = "UserName";
                    break;

                case "Full Name":
                    ColumnFilter = "FullName";
                    break;

                default:
                    ColumnFilter = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (tbFilter.Text.Trim() == "" || cbFilterBy.Text == "None")
            {
                _dtUser.DefaultView.RowFilter = "";
                lbRecords.Text = _dtUser.Rows.Count.ToString();
                return;
            }

            if (cbFilterBy.Text == "User ID" || cbFilterBy.Text == "Person ID")
                _dtUser.DefaultView.RowFilter = string.Format($"{ColumnFilter}={tbFilter.Text}");
            else
                _dtUser.DefaultView.RowFilter = string.Format($"{ColumnFilter} like '{tbFilter.Text}%'");

            lbRecords.Text = dgvUsers.Rows.Count.ToString();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                short ColumnFilterOfActive = -1;

                switch (cbIsActive.Text)
                {
                case "All":
                    break;

                    case "Yes":
                        ColumnFilterOfActive = 1;
                        break;

                    case "No":
                        ColumnFilterOfActive = 0;
                        break;


                }

            if (cbIsActive.Text == "All")
                _dtUser.DefaultView.RowFilter = "";
            else
                _dtUser.DefaultView.RowFilter = string.Format($"IsActive ={ColumnFilterOfActive}");

                lbRecords.Text = dgvUsers.Rows.Count.ToString();
              
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmAddNewUser((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshDGV();
            frmManagementUsers_Load(null, null);//it is a way to do refresh it makes like the method _RefreshDGV();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmShowUserDetails((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void addUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmAddNewUser();
            frm.ShowDialog();
            _RefreshDGV();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            if (MessageBox.Show("Are you sure you want to delete Person [" + dgvUsers.CurrentRow.Cells[0].Value + "]", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                if (clsUsers.DeleteUser((int)dgvUsers.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person Deleted Successfully", "Successed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshDGV();
                }
                else
                {
                    MessageBox.Show("Person was not Deleted because it has data link to it", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmChangePassword((int)dgvUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

    }
}
