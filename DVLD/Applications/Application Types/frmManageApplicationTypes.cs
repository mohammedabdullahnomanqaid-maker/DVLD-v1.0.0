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
    public partial class frmManageApplicationTypes : Form
    {
        static private DataTable _dtAllApplicationTypes = clsApplicationTypes.GetAllApplications();
        private DataTable _dtApplications = _dtAllApplicationTypes.DefaultView.ToTable(false, "ApplicationTypeID", "ApplicationTypeTitle", "ApplicationFees");
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void _RefreshData()
        {
         DataTable _dtAllApplicationTypes = clsApplicationTypes.GetAllApplications();
        _dtApplications = _dtAllApplicationTypes.DefaultView.ToTable(false, "ApplicationTypeID", "ApplicationTypeTitle", "ApplicationFees");

            dgvApplicationTypes.DataSource = _dtApplications;
            lbRecord.Text = dgvApplicationTypes.Rows.Count.ToString();

        }
        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            dgvApplicationTypes.DataSource = _dtApplications;

            if (dgvApplicationTypes.Rows.Count > 0)
            {
                dgvApplicationTypes.Columns[0].HeaderText = "ID";
                dgvApplicationTypes.Columns[0].Width = 50;

                dgvApplicationTypes.Columns[1].HeaderText = "Title";
                dgvApplicationTypes.Columns[1].Width = 325;

                dgvApplicationTypes.Columns[2].HeaderText = "Fees";
                dgvApplicationTypes.Columns[2].Width = 90;
            }
            lbRecord.Text = dgvApplicationTypes.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmUpdateApplicationsTypes((int)dgvApplicationTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshData();
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnClose, 88, 31);

        }
        
        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            clsUI.MauseEnterLeave(btnClose, 86, 29);

        }
    }
}
