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
    public partial class frmLocalDrivingLicenseApplications : Form
    {
        private enum enTypeOfTest { eVision=0, eWritten=1 , eStreet=2,DoneTest=3 };

          private const string New = "New";
          private const string Cancel = "Cancel";
          private const string Completed = "Completed";

        static public DataTable _dtAllLocalDrivingLicenseApplications;
        public frmLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private void frmLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;

            _dtAllLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();
            dgvLocalDrivingApplications.DataSource = _dtAllLocalDrivingLicenseApplications;
            lbRecord.Text = dgvLocalDrivingApplications.Rows.Count.ToString();

            if (dgvLocalDrivingApplications.Rows.Count > 0)
            {
                dgvLocalDrivingApplications.Columns[0].HeaderText = "L.D.L.AppID";
                dgvLocalDrivingApplications.Columns[0].Width = 100;

                dgvLocalDrivingApplications.Columns[1].HeaderText = "Driving Class";
                dgvLocalDrivingApplications.Columns[1].Width = 270;

                dgvLocalDrivingApplications.Columns[2].HeaderText = "National No";
                dgvLocalDrivingApplications.Columns[2].Width = 150;

                dgvLocalDrivingApplications.Columns[3].HeaderText = "Full Name";
                dgvLocalDrivingApplications.Columns[3].Width = 360;

                dgvLocalDrivingApplications.Columns[4].HeaderText = "Application Date";
                dgvLocalDrivingApplications.Columns[4].Width = 180;

                dgvLocalDrivingApplications.Columns[5].HeaderText = "Passed Test";
                dgvLocalDrivingApplications.Columns[5].Width = 150;

                dgvLocalDrivingApplications.Columns[6].HeaderText = "Status";
                dgvLocalDrivingApplications.Columns[6].Width = 110;

            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Visible = (cbFilterBy.Text != "None"&&cbFilterBy.Text!="Status");
            cbStatus.Visible = (cbFilterBy.Text=="Status");
            tbFilter.Clear();
            tbFilter.Focus();
          //  _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            string ColumnFilter = string.Empty;

            switch (cbFilterBy.Text.Trim())
            {
                case "L.D.L.AppID":
                    ColumnFilter = "LocalDrivingLicenseApplicationID";
                    break;

                case "National No":
                    ColumnFilter = "NationalNo";
                    break;

                case "Full Name":
                    ColumnFilter = "FullName";
                    break;
            }

            if (cbFilterBy.Text == "None" || string.IsNullOrEmpty(tbFilter.Text.Trim()))
            {
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
                tbFilter.Focus();
                return;
            }

            if (cbFilterBy.Text == "L.D.L.AppID")
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format($"{ColumnFilter}={tbFilter.Text.Trim()}");
            else
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format($"{ColumnFilter} like '{tbFilter.Text.Trim()}%'");
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format($"{cbFilterBy.Text.Trim()} like '{cbStatus.Text.Trim()}%'");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditNewDrivingLicenseApplication();
            frm.ShowDialog();
            frmLocalDrivingLicenseApplications_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditNewDrivingLicenseApplication((int)dgvLocalDrivingApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmLocalDrivingLicenseApplications_Load(null, null);
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowLocalDrivingLicenseApplicationInfo((int)dgvLocalDrivingApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmLocalDrivingLicenseApplications_Load(null, null);
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLAppID=Convert.ToInt32(dgvLocalDrivingApplications.CurrentRow.Cells[0].Value);
            if (MessageBox.Show("Are you sure you want to delete this Application ?", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.Cancel)
                return;
            
               clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find((int)dgvLocalDrivingApplications.CurrentRow.Cells[0].Value);
            if (_LocalDrivingLicenseApplication != null)
            {
                if (_LocalDrivingLicenseApplication.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   frmLocalDrivingLicenseApplications_Load(null, null);
                }
                else
                    MessageBox.Show("Could not delete Application, other data depend on it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure, do you want to cancel this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find((int)dgvLocalDrivingApplications.CurrentRow.Cells[0].Value);
            if(LocalDrivingLicenseApplication!=null)
            {
                if (LocalDrivingLicenseApplication.Cancel())
                {
                    MessageBox.Show("Application canceled Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmLocalDrivingLicenseApplications_Load(null, null);
                }
                else
                    MessageBox.Show("Application did not canceled,because there is data depend on it.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void _SechedulTest(clsTestTypes.enTestType TestType)
        {
            Form frm = new frmTestAppointment((int)dgvLocalDrivingApplications.CurrentRow.Cells[0].Value,TestType);
            frm.ShowDialog();
            frmLocalDrivingLicenseApplications_Load(null, null);
        }
        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _SechedulTest(clsTestTypes.enTestType.VisionTest);
        }
         
        private void writtenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _SechedulTest(clsTestTypes.enTestType.WrittenTest);
        }

        private void streetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _SechedulTest(clsTestTypes.enTestType.StreetTest);
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = -1;
            LicenseID =clsLocalDrivingLicenseApplication.Find((int)dgvLocalDrivingApplications.CurrentRow.Cells[0].Value).GetActiveLicenseID();
            if (LicenseID != -1)
            {
                Form frm = new frmShowLicenseInfo(LicenseID);
                frm.ShowDialog();
            }
            else
                MessageBox.Show("No License Found ", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);
           
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmIssueDrivingLicenseForFirstTime((int)dgvLocalDrivingApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmLocalDrivingLicenseApplications_Load(null, null);

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = clsBPeople.GetPersonIDByLocalDrivingLicenseApplicationID((int)dgvLocalDrivingApplications.CurrentRow.Cells[0].Value);
           
            Form frm = new frmShowLicenseHistory(PersonID);
            frm.ShowDialog();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicensApplication = clsLocalDrivingLicenseApplication.Find(LocalDrivingLicenseApplicationID);
            if (LocalDrivingLicensApplication == null)
                return;

            int TotalPassedTests = (int)dgvLocalDrivingApplications.CurrentRow.Cells[5].Value;
            bool IsLicenseExist = LocalDrivingLicensApplication.IsLicenseIssued();

            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = !IsLicenseExist && LocalDrivingLicensApplication.ApplicationStatus== clsApplications.enApplicationStatus.New;//to avoid cancel status.
            showLicenseToolStripMenuItem.Enabled = IsLicenseExist;
            sechedulMenueItemStrip.Enabled = !IsLicenseExist;
            editToolStripMenuItem.Enabled = !IsLicenseExist && (LocalDrivingLicensApplication.ApplicationStatus == clsApplications.enApplicationStatus.New);
            cancelApplicationToolStripMenuItem.Enabled = LocalDrivingLicensApplication.ApplicationStatus == clsApplications.enApplicationStatus.New;
            deleteApplicationToolStripMenuItem.Enabled = LocalDrivingLicensApplication.ApplicationStatus == clsApplications.enApplicationStatus.New;

            bool IsPassedVisionTest = LocalDrivingLicensApplication.DoesPassedTest(clsTestTypes.enTestType.VisionTest);
            bool IsPassedWrittenTest = LocalDrivingLicensApplication.DoesPassedTest(clsTestTypes.enTestType.WrittenTest);
            bool IsPassedStreetTest = LocalDrivingLicensApplication.DoesPassedTest(clsTestTypes.enTestType.StreetTest);

            sechedulMenueItemStrip.Enabled = (!IsPassedVisionTest || !IsPassedWrittenTest || !IsPassedStreetTest) && LocalDrivingLicensApplication.ApplicationStatus == clsApplications.enApplicationStatus.New;//to ensure is still have tes and not cancel.

            if (sechedulMenueItemStrip.Enabled)
            {
                visionTestToolStripMenuItem.Enabled = !IsPassedVisionTest;
                writtenTestToolStripMenuItem.Enabled = IsPassedVisionTest && !IsPassedWrittenTest;
                streetTestToolStripMenuItem.Enabled = IsPassedVisionTest && IsPassedWrittenTest && !IsPassedStreetTest;
            }
        }
    }
}
