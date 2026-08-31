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
    public partial class frmManagementDetaind : Form
    {
        private static DataTable _dtDetaindInfo;
        public frmManagementDetaind()
        {
            InitializeComponent();
        }

        private void frmManagementDetaind_Load(object sender, EventArgs e)
        {
             _dtDetaindInfo = clsDetaind.GetAllDetaind();
            cbFilterBy.SelectedIndex = 0;
            dgvDetaind.DataSource = _dtDetaindInfo;

            if (dgvDetaind.Rows.Count > 0)
            {
                dgvDetaind.Columns[0].HeaderText = "Detaind ID";
                dgvDetaind.Columns[0].Width = 125;

                dgvDetaind.Columns[1].HeaderText = "License ID";
                dgvDetaind.Columns[1].Width = 120;

                dgvDetaind.Columns[2].HeaderText = "Detaind Date";
                dgvDetaind.Columns[2].Width = 180;

                dgvDetaind.Columns[3].HeaderText = "Fine Fees";
                dgvDetaind.Columns[3].Width = 120;

                dgvDetaind.Columns[4].HeaderText = "Release Date";
                dgvDetaind.Columns[4].Width = 180;

                dgvDetaind.Columns[5].HeaderText = "National No";
                dgvDetaind.Columns[5].Width = 130;

                dgvDetaind.Columns[6].HeaderText = "Full Name";
                dgvDetaind.Columns[6].Width = 300;


                dgvDetaind.Columns[7].HeaderText = "R.App ID";
                dgvDetaind.Columns[7].Width = 120;

                dgvDetaind.Columns[8].HeaderText = "Is Released";
                dgvDetaind.Columns[8].Width = 120;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Visible = (cbFilterBy.Text != "None" && cbFilterBy.Text != "Is Release");
            cbIsRelease.Visible = (cbFilterBy.Text == "Is Release");
            tbFilter.Clear();
            cbIsRelease.SelectedIndex = 0;
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            string ColumnFilter = string.Empty;
            switch (cbFilterBy.Text)
            {
                case "Detaind ID":
                    ColumnFilter = "DetainID";
                    break;

                case "Release Application ID":
                    ColumnFilter = "ReleaseApplicationID";
                    break;

                case "Is Release":
                    ColumnFilter = "IsRelease";
                    break;

                case "National No":
                    ColumnFilter = "NationalNo";
                    break;

                case "Full Name":
                    ColumnFilter = "FullName";
                    break;
            }

            if (tbFilter.Text == ""||cbFilterBy.Text=="None")
            {
                _dtDetaindInfo.DefaultView.RowFilter = "";
                return;
            }

            if (cbFilterBy.Text == "National No" || cbFilterBy.Text == "Full Name")
                _dtDetaindInfo.DefaultView.RowFilter = $"{ColumnFilter} like '{tbFilter.Text}%'";
            else
                _dtDetaindInfo.DefaultView.RowFilter = $"{ColumnFilter} ={tbFilter.Text}";
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterBy.Text=="Release Application ID"||cbFilterBy.Text=="Detaind ID")
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cbIsRelease_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbIsRelease.Text)
            {
                case "Yes":
                    _dtDetaindInfo.DefaultView.RowFilter = "IsReleased=1";
                    break;

                case "No":
                    _dtDetaindInfo.DefaultView.RowFilter = "IsReleased=0";
                    break;

                case "All":
                    _dtDetaindInfo.DefaultView.RowFilter = "";
                    break;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddDetaind();
            frm.ShowDialog();
            frmManagementDetaind_Load(null, null);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Form frm = new frmReleaseDetianLicense();
            frm.ShowDialog();
            frmManagementDetaind_Load(null, null);

        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonDetails(clsBPeople.GetPersonIDByLicenseID((int)dgvDetaind.CurrentRow.Cells[1].Value));
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm=new frmShowLicenseInfo((int)dgvDetaind.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowLicenseHistory(clsBPeople.GetPersonIDByLicenseID((int)dgvDetaind.CurrentRow.Cells[1].Value));
            frm.ShowDialog();
        }

        private void releaseDetainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetianLicense frm = new frmReleaseDetianLicense((int)dgvDetaind.CurrentRow.Cells[1].Value);//when you define in this way Form frm=new frmRelease(). that mean as you said deal with frmReleas() as a Form in genral and show the genral method of Form.
            frm.ShowDialog();
            frmManagementDetaind_Load(null, null);
        }

        private void dgvDetaind_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (clsDetaind.IsLicenseDetaind((int)dgvDetaind.CurrentRow.Cells[1].Value))
                releaseDetainLicenseToolStripMenuItem.Enabled = true;
            else
                releaseDetainLicenseToolStripMenuItem.Enabled = false;
        }
    }
}
