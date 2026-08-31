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
    public partial class frmDriversList : Form
    {
        private static DataTable _dtAllDrivers;
        public frmDriversList()
        {
            InitializeComponent();
        }

        private void frmDriversList_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            _dtAllDrivers = clsDrivers.GetAllDrivers();
            dgvDrivers.DataSource = _dtAllDrivers;

            if (dgvDrivers.Rows.Count > 0)
            {
                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[0].Width = 100;

                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[1].Width = 100;

                dgvDrivers.Columns[2].HeaderText = "National No";
                dgvDrivers.Columns[2].Width = 110;

                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[3].Width = 300;

                dgvDrivers.Columns[4].HeaderText = "Date";
                dgvDrivers.Columns[4].Width = 150;

                dgvDrivers.Columns[5].HeaderText = "Is Active";
                dgvDrivers.Columns[5].Width = 90;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Visible = (cbFilterBy.Text != "None");
            tbFilter.Clear();
            tbFilter.Focus();
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            string ColumnFilter = string.Empty;

            switch (cbFilterBy.Text)
            {
                case "Driver ID":
                    ColumnFilter = "DriverID";
                    break;

                case "Person ID":
                    ColumnFilter = "PersonID";
                    break;

                case "Full Name":
                    ColumnFilter = "FullName";
                    break;

                case "National No":
                    ColumnFilter = "NationalNo";
                    break;
            }

            if (string.IsNullOrEmpty(tbFilter.Text)||cbFilterBy.Text=="None")
            {
                _dtAllDrivers.DefaultView.RowFilter = "";
                return;
            }

            if (cbFilterBy.Text.Trim() == "Driver ID" || cbFilterBy.Text.Trim() == "Person ID")
                _dtAllDrivers.DefaultView.RowFilter = string.Format($"{ColumnFilter} = {tbFilter.Text.Trim()}");
            else
                _dtAllDrivers.DefaultView.RowFilter = string.Format($"{ColumnFilter} like '{tbFilter.Text.Trim()}%'");
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonDetails((int)dgvDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }

        private void issueInternationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = -1;
             LicenseID = clsLicense.GetActiveLicenseIDWhenLicenseIsOrdinaryLicenseClassByPersonID((int)dgvDrivers.CurrentRow.Cells[1].Value);
           if(LicenseID==-1)
            {
                MessageBox.Show("An International driving license can not be issued. Please ensure that the person has an active license of Ordinary driving class.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Form frm = new frmAddInternationalLicense(LicenseID);
            frm.ShowDialog();
        }

        private void showLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowLicenseHistory((int)dgvDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }
    }
}
