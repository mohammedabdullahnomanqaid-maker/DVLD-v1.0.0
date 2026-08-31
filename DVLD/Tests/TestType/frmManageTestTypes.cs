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
    public partial class frmManageTestTypes : Form
    {
        static private DataTable _dtAllTestTypes = clsTestTypes.GetAllTestTypes();
                                                                        //false to give all rows without distinct
                                                                        // true as you put distinct.
        public DataTable _dtTestTypes = _dtAllTestTypes.DefaultView.ToTable(false, "TestTypeID", "TestTypeTitle", "TestTypeDescription", "TestTypeFees");
        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        private void _RefreshData()
        {
           _dtAllTestTypes = clsTestTypes.GetAllTestTypes();
        _dtTestTypes = _dtAllTestTypes.DefaultView.ToTable(false, "TestTypeID", "TestTypeTitle", "TestTypeDescription", "TestTypeFees");
            dgvTestTypes.DataSource = _dtTestTypes;
            lbRecord.Text = dgvTestTypes.Rows.Count.ToString();

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            dgvTestTypes.DataSource = _dtTestTypes;
            //and should make the AutoSizeMode =allCellsExceptHeader
            dgvTestTypes.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            
            if (_dtTestTypes.Rows.Count > 0)
            {
                dgvTestTypes.Columns[0].HeaderText = "ID";
                dgvTestTypes.Columns[0].Width = 60;

                dgvTestTypes.Columns[1].HeaderText = "Title";
                dgvTestTypes.Columns[1].Width = 190;

                dgvTestTypes.Columns[2].HeaderText = "Description";
                dgvTestTypes.Columns[2].Width = 361;

                dgvTestTypes.Columns[3].HeaderText = "Fees";
                dgvTestTypes.Columns[3].Width = 100;
            }
            lbRecord.Text = dgvTestTypes.Rows.Count.ToString();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmUpdateTestTypes((clsTestTypes.enTestType)dgvTestTypes.CurrentRow.Cells[0].Value);
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
