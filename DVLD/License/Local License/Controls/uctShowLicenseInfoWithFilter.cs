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
    public partial class uctShowLicenseInfoWithFilter : UserControl
    {

        public event Action<int> onLicenseSelected;
        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int> Handle = onLicenseSelected;
            if (Handle != null)
                Handle(LicenseID);
        }
        public bool FilterEnable
        {
            set { gbFilter.Enabled = value; }
            get { return gbFilter.Enabled; }
        }
        public clsLicense License
        {
            get {return uctLicenseInfo1.License; }
        }
        public uctShowLicenseInfoWithFilter()
        {
            InitializeComponent();
        }
        //done
        private void tbFilter_Validating(object sender, CancelEventArgs e)
        {
            if (tbFilter.Text.Trim() == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(tbFilter, "This Field is required!");
            }
            else
                errorProvider1.SetError(tbFilter, "");

        }
        //done
        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);

            if (e.KeyChar == (char)13)
                btnSearch.PerformClick();

        }

        //done
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid,put mouse over the red icon(s) to see the error! ", "Validating Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            uctLicenseInfo1.LoadLicenseInfo(Convert.ToInt32(tbFilter.Text.Trim()));
            if (onLicenseSelected != null && FilterEnable)
                onLicenseSelected(uctLicenseInfo1.LicenseID);
            tbFilter.Focus();
        }
        //done
       public void FocusOnTbFilter()
        {
            tbFilter.Focus();
        }
       
       public void LoadInfo(int LicenseID)
        {
            tbFilter.Text = LicenseID.ToString();
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid,put mouse over the red icon(s) to see the error! ", "Validating Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            uctLicenseInfo1.LoadLicenseInfo(Convert.ToInt32(tbFilter.Text.Trim()));
            if (onLicenseSelected != null && FilterEnable)
                onLicenseSelected(uctLicenseInfo1.LicenseID);
            tbFilter.Focus();
        }
    }
}
