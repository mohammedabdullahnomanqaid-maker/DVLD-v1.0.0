using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Course19
{
    public partial class frmDashboard : Form
    {
      private frmLoginScreen _frmlogin;
        public frmDashboard(frmLoginScreen frm)
        {
            InitializeComponent();
            _frmlogin = frm;
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmManageApplicationTypes();
            frm.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmAddEditNewDrivingLicenseApplication();
            frm.ShowDialog();
        }

        private void interNationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmAddInternationalLicense();
            frm.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmRenewedLicense();
            frm.ShowDialog();
        }

        private void damagedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmApplicationInfoLicenseReplacement();
            frm.ShowDialog();
        }

        private void lostDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmApplicationInfoLicenseReplacement();
            frm.ShowDialog();

        }

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmReleaseDetianLicense();
            frm.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmLocalDrivingLicenseApplications();
            frm.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmDriversList();
            frm.ShowDialog();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Form frm = new frmManagementUsers();
            frm.ShowDialog();
        }

        private void accountSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("No Impelementation yet !!", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmPeople();
            frm.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            _frmlogin.Show();
            this.Close();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowUserDetails(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmChangePassword(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void manageTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmManageTestTypes();
            frm.ShowDialog();
        }

        private void localDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmLocalDrivingLicenseApplications();
            frm.ShowDialog();
        }

        private void interNationalLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form frm = new frmManagementInternationalLicense();
            frm.ShowDialog();
        }

        private void manageDetaindLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmManagementDetaind();
            frm.ShowDialog();
        }

        private void detaindLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddDetaind();
            frm.ShowDialog();
        }

        private void releaseLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmReleaseDetianLicense();
            frm.ShowDialog();
        }
    }
}
