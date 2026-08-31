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
    public partial class frmManagementInternationalLicense : Form
    {
        private static DataTable _dtAllInternationalLicense = clsInternationalLicense.GetAllInternationalLicense();
        private DataTable _dtInternationalLicense = _dtAllInternationalLicense.DefaultView.ToTable(false, "InternationalLicenseID", "ApplicationID", "DriverID", "IssuedUsingLocalLicenseID", "IssueDate", "ExpirationDate", "IsActive");
       
        private void _RefreshData()
        {
            _dtAllInternationalLicense = clsInternationalLicense.GetAllInternationalLicense();
            _dtInternationalLicense = _dtAllInternationalLicense.DefaultView.ToTable(false, "InternationalLicenseID", "ApplicationID", "DriverID", "IssuedUsingLocalLicenseID", "IssueDate", "ExpirationDate", "IsActive");
            dgvInternationalLicense.DataSource = _dtInternationalLicense;
            lbRecords.Text = dgvInternationalLicense.Rows.Count.ToString();
        }

        public frmManagementInternationalLicense()
        {
            InitializeComponent();
        }

        private void frmManagementInternationalLicense_Load(object sender, EventArgs e)
        {
            cbFilter.SelectedIndex = 0;

            dgvInternationalLicense.DataSource = _dtInternationalLicense;
            if (dgvInternationalLicense.Rows.Count > 0)
            {
                dgvInternationalLicense.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicense.Columns[0].Width = 170;

                dgvInternationalLicense.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicense.Columns[1].Width = 170;

                dgvInternationalLicense.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicense.Columns[2].Width = 170;

                dgvInternationalLicense.Columns[3].HeaderText = "License ID";
                dgvInternationalLicense.Columns[3].Width = 170;

                dgvInternationalLicense.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicense.Columns[4].Width = 250;

                dgvInternationalLicense.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalLicense.Columns[5].Width = 250;

                dgvInternationalLicense.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicense.Columns[6].Width = 125;
            }
            lbRecords.Text = dgvInternationalLicense.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewInternationalLicense_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddInternationalLicense();
            frm.ShowDialog();
            _RefreshData();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Form frm = new frmShowPersonDetails(clsBPeople.GetPersonIDByLicenseID((int)dgvInternationalLicense.CurrentRow.Cells[3].Value));
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowInternationalLicenseInfo((int)dgvInternationalLicense.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = clsBPeople.GetPersonIDByLicenseID((int)dgvInternationalLicense.CurrentRow.Cells[3].Value);
            Form frm = new frmShowLicenseHistory(PersonID);
            frm.ShowDialog();
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            string ColumnFilter = string.Empty;
            switch(cbFilter.Text.Trim())
            {
                case "International License ID":
                    ColumnFilter = "InternationalLicenseID";
                    break;

                case "Application ID":
                    ColumnFilter = "ApplicationID";
                    break;

                case "Driver ID":
                    ColumnFilter = "DriverID";
                    break;

                case "Local License ID":
                    ColumnFilter = "IssuedUsingLocalLicenseID";
                    break;
            }
            if (tbFilter.Text.Trim() == "")
            {
                _dtInternationalLicense.DefaultView.RowFilter = "";
                return;
            }

            _dtInternationalLicense.DefaultView.RowFilter = $"{ColumnFilter} ={tbFilter.Text.Trim()}";
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Visible = (cbFilter.Text.Trim() != "None"&& cbFilter.Text.Trim() != "Is Active");

            cbIsActive.Visible = (cbFilter.Text.Trim() == "Is Active");

            tbFilter.Clear();
            tbFilter.Focus();
            cbIsActive.SelectedIndex = 0;
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbIsActive.Text.Trim())
            {
                case "Yes":
                    _dtInternationalLicense.DefaultView.RowFilter = $"IsActive=1";
                    break;

                case "No":
                    _dtInternationalLicense.DefaultView.RowFilter = $"IsActive=0";
                    break;
            }
            if (cbIsActive.Text.Trim() == "All")
                _dtInternationalLicense.DefaultView.RowFilter = "";
        }
    }
}
