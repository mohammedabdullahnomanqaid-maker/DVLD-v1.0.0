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
using System.Data;
using System.IO;

namespace Course19
{
    public partial class frmPeople : Form
    {
       //All columns
        private static DataTable _dtAllInfoOfPeople = clsBPeople.GetAllInfoOfPeople();
        // _dtPeople the columns that you need
        private DataTable _dtPeople = _dtAllInfoOfPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
            "FirstName", "SecondName", "ThirdName","LastName",
            "Gendor", "DateOfBirth", "CountryName",
            "Phone", "Email");
   
        private void _RefreshPeoplList()
        {
               DataTable _dtAllInfoOfPeople = clsBPeople.GetAllInfoOfPeople();
        
        _dtPeople = _dtAllInfoOfPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
            "FirstName", "SecondName", "ThirdName", "LastName",
            "Gendor", "DateOfBirth", "CountryName",
            "Phone", "Email");

            dgvPeople.DataSource = _dtPeople;
            lbRecords.Text = _dtPeople.Rows.Count.ToString();
    }
        public frmPeople()
        {
            InitializeComponent();
        }
   
        //done
        private void frmPeople_Load(object sender, EventArgs e)
        {
            dgvPeople.DataSource = _dtPeople;
            lbRecords.Text = _dtPeople.Rows.Count.ToString();
            cbFilter.SelectedIndex = 0;

            if(dgvPeople.Rows.Count>0)
            {
                dgvPeople.Columns[0].HeaderText = "Person ID";
                dgvPeople.Columns[0].Width = 110;

                dgvPeople.Columns[1].HeaderText = "National No";
                dgvPeople.Columns[1].Width = 120;

                dgvPeople.Columns[2].HeaderText = "First Name";
                dgvPeople.Columns[2].Width = 120;

                dgvPeople.Columns[3].HeaderText = "Second Name";
                dgvPeople.Columns[3].Width = 140;

                dgvPeople.Columns[4].HeaderText = "Third Name";
                dgvPeople.Columns[4].Width = 120;

                dgvPeople.Columns[5].HeaderText = "Last Name";
                dgvPeople.Columns[5].Width = 110;

                dgvPeople.Columns[6].HeaderText = "Gendor";
                dgvPeople.Columns[6].Width = 120;

                dgvPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvPeople.Columns[7].Width = 140;

                dgvPeople.Columns[8].HeaderText = "Nationality";
                dgvPeople.Columns[8].Width = 120;

                dgvPeople.Columns[9].HeaderText = "Phone";
                dgvPeople.Columns[9].Width = 120;

                dgvPeople.Columns[10].HeaderText = "Email";
                dgvPeople.Columns[10].Width = 170;
            }
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(picAddPerson, 50, 50);
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(picAddPerson, 50, 43);
        }

        private void picAddPerson_Click(object sender, EventArgs e)
        {
          
            Form frm = new frmAddEditPerson();
            frm.ShowDialog();

            _RefreshPeoplList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnClose,88,31);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnClose, 86, 29);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
           if(MessageBox.Show("Are you sure you want to delete Person [" + dgvPeople.CurrentRow.Cells[0].Value + "]", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)==DialogResult.OK)
            {
                if (clsBPeople.DeletePerson((int)dgvPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person Deleted Successfully", "Successed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshPeoplList();
                }
                else
                {
                    MessageBox.Show("Person was not Deleted because it has data link to it", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
           
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmAddEditPerson((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshPeoplList();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            frmShowPersonDetails frm = new frmShowPersonDetails((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshPeoplList();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Form frm = new frmAddEditPerson();
            frm.ShowDialog();

            _RefreshPeoplList();
        }
        //done
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilterValue.Visible = (cbFilter.Text != "None");
          
            if(tbFilterValue.Visible)
            {
                tbFilterValue.Clear();
                tbFilterValue.Focus();
            }

        }
        //done
        private void tbFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = string.Empty;
            switch (cbFilter.SelectedItem.ToString())
            {
                case "PersonID":
                    FilterColumn = "PersonID";
                    break;

                case "FirstName":
                    FilterColumn = "FirstName";
                    break;

                case "SecondName":
                    FilterColumn = "SecondName";
                    break;

                case "LastName":
                    FilterColumn = "LastName";
                    break;

                case "CountryName":
                    FilterColumn = "CountryName";
                    break;

                case "Gendor":
                    FilterColumn = "Gendor";
                    break;

                case "Phone":
                    FilterColumn = "Phone";
                    break;

                case "Email":
                    FilterColumn = "Email";
                    break;


                case "NationalNo":
                    FilterColumn = "NationalNo";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if (tbFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtPeople.DefaultView.RowFilter = "";
                lbRecords.Text = dgvPeople.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "PersonID")
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}]={1}", FilterColumn, tbFilterValue.Text.Trim());
            else
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] like '{1}%'", FilterColumn, tbFilterValue.Text.Trim());

            lbRecords.Text = dgvPeople.Rows.Count.ToString();

        }
        //done
        private void tbFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "PersonID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
