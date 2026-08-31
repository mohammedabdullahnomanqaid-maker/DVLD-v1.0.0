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
    public partial class frmAddEditNewDrivingLicenseApplication : Form
    {
        private int _SelectedPersonID = -1;
        private int _LocalDrivingLicenseApplicationID;
        private static DataTable _dtLicenseClass= clsLicenseClass.GetAllLicenseClass();//to check age and fill combobox and to achieve it every time from here not to go to DB.
        private enum enMode { AddMode,UpdateMode};
        private enMode _Mode = enMode.AddMode;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        public frmAddEditNewDrivingLicenseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddMode;
        }

        public frmAddEditNewDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _Mode = enMode.UpdateMode;
        }

        private void _LoadData()
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(_LocalDrivingLicenseApplicationID);
            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("LDLAppID = " + _LocalDrivingLicenseApplicationID.ToString() + " Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lbDLAppID.Text = _LocalDrivingLicenseApplicationID.ToString();
            lbApplicationDate.Text =clsFormat.DateToShort(_LocalDrivingLicenseApplication.ApplicationDate);
            lbCreatedBy.Text =clsUsers.FindUserByUserID(_LocalDrivingLicenseApplication.CreatedByUser).UserName;
            lbFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
            cbLicenseClass.SelectedIndex =cbLicenseClass.FindString(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName);
            uctSearchAndShowPersonCard1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicationPersonID);

        }
       private void _ResetDefaultValues()
        {

            cbLicenseClass.DataSource = _dtLicenseClass;
            cbLicenseClass.DisplayMember = "ClassName";
            cbLicenseClass.ValueMember = "LicenseClassID";

            if (_Mode == enMode.AddMode)
            {
                lbAddEditLocalDrivingLicenseApplication.Text = "Add Local Driving License Application";
                this.Text = "Add Local Driving License Application";
                uctSearchAndShowPersonCard1.FilterEnable = true;
                tpApplicationInfo.Enabled = false;
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
                uctSearchAndShowPersonCard1.FilterFocus();
                lbApplicationDate.Text = DateTime.Now.ToShortDateString();
                lbCreatedBy.Text = clsGlobal.CurrentUser.UserName;
                cbLicenseClass.SelectedIndex = 2;
                lbFees.Text = clsApplicationTypes.GetApplicationFees((int)clsApplications.enApplicationType.NewApplication).ToString();

            }
            else
            {

                lbAddEditLocalDrivingLicenseApplication.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";
                uctSearchAndShowPersonCard1.FilterEnable = false;
                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }


         

        }
        private void frmAddNewDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.UpdateMode)
                _LoadData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            
            if (uctSearchAndShowPersonCard1.PersonID == -1)
            {
                MessageBox.Show("Please Selected Person !", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                uctSearchAndShowPersonCard1.FilterFocus();
            }
            else
            {

                tabControl1.SelectedTab = tabControl1.TabPages["tpApplicationInfo"];
                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }
        }

        private bool _IsAgeMachAllowedAge()
        {
            //here to check is the Applicant's age is allowed .
            var Resault = _dtLicenseClass.Rows[Convert.ToInt32(cbLicenseClass.SelectedValue)-1]["MinimumAllowedAge"];
            int AllowedAge = Convert.ToInt32(Resault);
            int PersonAge = DateTime.Today.Year - uctSearchAndShowPersonCard1.SelectedPersonInfo.DateOfBirth.Year;

            if (PersonAge>=AllowedAge)
                return true;
            else
                return false;

        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            if (!_IsAgeMachAllowedAge())
            {
                MessageBox.Show("Age is not match the Allowed age", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int LicenseClass =Convert.ToInt32(cbLicenseClass.SelectedValue);
            int ActiveApplication = clsApplications.GetActiveApplicationIDForLicenseClass(_SelectedPersonID,(int) clsApplications.enApplicationType.NewApplication, LicenseClass);
           
            if (ActiveApplication != -1)
            {
                MessageBox.Show("Choose another license class ,the selected Person Already have an active application for the selected class with id=" + uctSearchAndShowPersonCard1.PersonID, "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return;
            }

            if (clsLicense.IsLicenseExistByPersonID(_SelectedPersonID,LicenseClass))
            {
                MessageBox.Show("Person Already have a License with the same applied driving class,choose diffrent driving class.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return;
            }
               
            _LocalDrivingLicenseApplication.ApplicationID = _LocalDrivingLicenseApplication.ApplicationID;
            _LocalDrivingLicenseApplication.ApplicationPersonID = uctSearchAndShowPersonCard1.PersonID;
            _LocalDrivingLicenseApplication.ApplicationTypeID =(int) clsApplications.enApplicationType.NewApplication;
            _LocalDrivingLicenseApplication.CreatedByUser = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseApplication.PaidFees = Convert.ToDecimal(lbFees.Text.Trim());
            _LocalDrivingLicenseApplication.ApplicationDate = Convert.ToDateTime(lbApplicationDate.Text.Trim());
            _LocalDrivingLicenseApplication.ApplicationStatusDate = Convert.ToDateTime(lbApplicationDate.Text.Trim());
            _LocalDrivingLicenseApplication.ApplicationStatus =clsApplications.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.LicenseClassID = (int)cbLicenseClass.SelectedValue;

            if (_LocalDrivingLicenseApplication.Save())
            {
                   
                        lbDLAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                        _Mode = enMode.UpdateMode;
                        lbAddEditLocalDrivingLicenseApplication.Text = "Update Local Driving License Application";
                        this.Text = "Update Local Driving License Application";
                        uctSearchAndShowPersonCard1.FilterEnable = false;
                        MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                    MessageBox.Show("Error : Data did not Save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
           
        }
        private void uctSearchAndShowPersonCard1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }
    }
}
