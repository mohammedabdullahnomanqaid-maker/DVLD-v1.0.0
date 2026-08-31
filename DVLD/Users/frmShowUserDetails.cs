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
    public partial class frmShowUserDetails : Form
    {
        public frmShowUserDetails(int PersonID)
        {
            InitializeComponent();
            uctShowUserInfo1.LoadData(PersonID);

        }

        private void frmShowUserDetails_Load(object sender, EventArgs e)
        {
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
    }
}
