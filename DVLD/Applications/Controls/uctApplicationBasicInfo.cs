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
    public partial class uctApplicationBasicInfo : UserControl
    {
        clsApplications _Application;
        private int _ApplicationID = -1;

        public clsApplications Applications
        {
            get { return _Application; }
        }
        public uctApplicationBasicInfo()
        {
            InitializeComponent();
        }
        public void ResetDefaultValue()
        {
            lbDate.Text = "[???]";
            lbStatusDate.Text = "[???]";
            lbFees.Text = "[???]";
            lbApplicant.Text = "[???]";
            lbCreatedBy.Text = "[???]";
            lbID.Text = "[???]";
            lbType.Text = "[???]";
            lbStatus.Text = "[???]";

        }
        private void _FillData()
        {
            lbDate.Text = clsFormat.DateToShort(_Application.ApplicationDate);
            lbStatusDate.Text = clsFormat.DateToShort(_Application.ApplicationStatusDate);
            lbFees.Text = _Application.PaidFees.ToString();
            lbApplicant.Text = clsBPeople.Find(_Application.ApplicationPersonID).FullName;
            lbCreatedBy.Text = _Application.CreatedByUserInfo.UserName;
            lbID.Text = _ApplicationID.ToString();
            lbType.Text = _Application.ApplicationTypeInfo.ApplicationTypesTitle;
            lbStatus.Text = _Application.StatusText;
        }
        public void LoadApplicationBasicInfoByApplicationID(int ApplicationID)
        {
            _Application = clsApplications.Find(ApplicationID);
            _ApplicationID = ApplicationID;

            if (_Application == null)
            {
                ResetDefaultValue();
                MessageBox.Show("Application ID =" + _ApplicationID.ToString() + " Not Found !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                _FillData();
        }
    }
}
