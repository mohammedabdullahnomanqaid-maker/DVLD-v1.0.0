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
    public partial class frmAddEditAppointment : Form
    {
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private clsApplications _Application;
        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID = -1;
        private int _LocalDrivingLicenseApplicationID = -1;
        public enum enCreationMode { NewSchedule=1,RetakeSchedule=2 };
        private enCreationMode _CreationMode = enCreationMode.NewSchedule;

        clsTestTypes.enTestType _TestTypeMode=clsTestTypes.enTestType.VisionTest;
        private enum enMode { AddMode,UpdateMode};
        enMode _Mode = enMode.AddMode;//to know the status of form add/update.

        public frmAddEditAppointment(int LocalDrivingLicenseApplicationID,clsTestTypes.enTestType TestTypeID,int TestAppointmentID=-1)
        {
            InitializeComponent();
    
            _TestTypeMode = TestTypeID;
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = TestAppointmentID;

            if (TestAppointmentID == -1)
                _Mode = enMode.AddMode;
            else
                _Mode = enMode.UpdateMode;
        }

        //done
        private void _ResetDefaultValue()
        {
            lbLDLAppID.Text = "[???]";
            lbName.Text = "[???]";
            lbFees.Text = "[???]";
            lbAppFees.Text = "[???]";
            lbTotalFees.Text = "[???]";
            lbTrail.Text = "[???]";
            lbDClass.Text = "[???]";

        }
        //done
        private void _LoadData()
        {

            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(_LocalDrivingLicenseApplicationID);
            if (_LocalDrivingLicenseApplication == null)
            {
                _ResetDefaultValue();
                btnSave.Enabled = false;
                MessageBox.Show("Error : Local Driving License Application ID =" + _TestAppointmentID.ToString() + " Not Found !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_LocalDrivingLicenseApplication.DoesAttendBefore((int)_TestTypeMode))
                _CreationMode = enCreationMode.RetakeSchedule;
            else
                _CreationMode = enCreationMode.NewSchedule;

            if (_CreationMode == enCreationMode.RetakeSchedule)
            {
                gpRetakeTestInfo.Enabled = true;
                lbTitle.Text = "Schedule Retake Test";
                lbAppFees.Text = clsApplicationTypes.Find((int)clsApplications.enApplicationType.RetakeTest).ApplicationTypesFees.ToString();
                lbRTestAppID.Text = "0";
            }
            else
            {
                gpRetakeTestInfo.Enabled = false;
                lbTitle.Text = "Schedule Test";
                lbAppFees.Text = "0";
                lbRTestAppID.Text = "N/A";
            }


            lbLDLAppID.Text = _LocalDrivingLicenseApplicationID.ToString();
            lbName.Text = _LocalDrivingLicenseApplication.FullName;
            lbTrail.Text = _LocalDrivingLicenseApplication.GetTrailNumber((int)_TestTypeMode).ToString();
            lbDClass.Text = _LocalDrivingLicenseApplication.LicenseClass.ClassName;


            if (_Mode == enMode.AddMode)
            {
                lbFees.Text = clsTestTypes.Find((int)_TestTypeMode).TestTypesFees.ToString();
                lbRTestAppID.Text = "N/A";
                dateTimePicker1.MinDate = DateTime.Now;
                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if (!_LoadTestAppointmentInfo())
                    return;
            }

            lbTotalFees.Text = (Convert.ToSingle(lbFees.Text) + Convert.ToSingle(lbAppFees.Text)).ToString();

            if (!_HandleActiveTestAppointmentConstraint())
                return;
            if (!_HandleAppointmentLockedConstraint())
                return;
            if (!_HandlePerviuseTestConstraint())
                return;
        }
        //done
        private bool _LoadTestAppointmentInfo()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);
            if (_TestAppointment == null)
            {
                MessageBox.Show("No Appointment with id = " + _TestAppointmentID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }
            _TestAppointment.TestAppointmentID = _TestAppointmentID;
            if (DateTime.Now.CompareTo(_TestAppointment.AppointmentDate) < 0)
                dateTimePicker1.MinDate = DateTime.Now;
            else
                dateTimePicker1.MinDate = _TestAppointment.AppointmentDate;

            dateTimePicker1.Value = _TestAppointment.AppointmentDate;
            lbFees.Text = _TestAppointment.PaidFees.ToString();

            if (_TestAppointment.RetakeTestAppointmentID == -1)
            {
                lbRTestAppID.Text = "N/A";
                lbAppFees.Text = "0";
            }
            else
            {
                lbRTestAppID.Text = _TestAppointment.RetakeTestAppointmentID.ToString();
                lbAppFees.Text = _TestAppointment.RetakeApplicationInfo.PaidFees.ToString();
                lbTitle.Text = "Schedul Retake Test";
                gpRetakeTestInfo.Enabled = true;
            }
            return true;
        }
        //done
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //done
        private void _PerformTypeOfTest()
        {
            switch (_TestTypeMode)
            {
                case clsTestTypes.enTestType.VisionTest:
                    picTypeOfTest.Image = Properties.Resources.Vision_512;
                    break;

                case clsTestTypes.enTestType.WrittenTest:
                    picTypeOfTest.Image = Properties.Resources.Written_Test_512;
                    break;

                case clsTestTypes.enTestType.StreetTest:
                    picTypeOfTest.Image = Properties.Resources.driving_test_512;
                    break;
            }
        }
    
        //done
        private void frmAddEditAppointment_Load(object sender, EventArgs e)
        {
      
            _PerformTypeOfTest();
            _LoadData();
        }
        private bool _HandleRetakeApplication()
        {

            //this will decide to create a seperate application for retake test or not.
            // and will create it if needed , then it will link it to the appoinment.
            if (_Mode == enMode.AddMode && _CreationMode == enCreationMode.RetakeSchedule)
            {
                //incase the mode is add new and creation mode is retake test we should create a seperate application for it.
                //then we linke it with the appointment.

                //First Create Applicaiton 
                 _Application = new clsApplications();
                _Application.ApplicationDate = DateTime.Now;
                _Application.ApplicationPersonID = _LocalDrivingLicenseApplication.ApplicationPersonID; 
                _Application.ApplicationStatusDate = DateTime.Now;
                _Application.CreatedByUser = clsGlobal.CurrentUser.UserID;
                _Application.PaidFees = Convert.ToDecimal(lbAppFees.Text.Trim());
                _Application.ApplicationTypeID = (int)clsApplications.enApplicationType.RetakeTest;//we can chieve it from class not object because the enum already static fixed variable and it can not change from object to object.
                _Application.ApplicationStatus =clsApplications.enApplicationStatus.Complete;
                if (!_Application.Save())
                {
                    _TestAppointment.RetakeTestAppointmentID = -1;

                    MessageBox.Show("Application did not save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                _TestAppointment.RetakeTestAppointmentID = _Application.ApplicationID;

            }
            return true;
        }
        //done
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeApplication())
                return;

            //we still have when save retake test make retake test application for it.
            _TestAppointment.AppointmentDate = dateTimePicker1.Value;
            _TestAppointment.IsLocked = false;  //lbfees to store the fee of the appointment just and the fee of retake store it in Application table
            _TestAppointment.PaidFees =Convert.ToDecimal(lbFees.Text.Trim());
            _TestAppointment.LocalDrivingLicenseApplicationID =Convert.ToInt32(lbLDLAppID.Text.Trim());
            _TestAppointment.TestTypeID = (int)_TestTypeMode;
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;


            if (_TestAppointment.Save())
            {
                MessageBox.Show("Data saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            
        }

        private bool _HandlePerviuseTestConstraint()
        {
            //we need to make sure that this person passed the prvious required test before apply to the new test.
            //person cannnot apply for written test unless s/he passes the vision test.
            //person cannot apply for street test unless s/he passes the written test.

            switch (_TestTypeMode)
            {
                case clsTestTypes.enTestType.VisionTest:
                    //in this case not required to check if pass perviuse test.
                    lbUserMessage.Visible = false;
                    return true;

                case clsTestTypes.enTestType.WrittenTest:
                    //Written Test, you cannot sechdule it before person passes the vision test.
                    //we check if pass visiontest 1.
                    if (!clsTest.IsPassed(_LocalDrivingLicenseApplicationID ,(int)clsTestTypes.enTestType.VisionTest))
                    {
                        dateTimePicker1.Enabled = false;
                        btnSave.Enabled = false;
                        lbUserMessage.Visible = true;
                        lbUserMessage.Text = "Cannot Sechule, Vision Test should be passed first";
                        return false;
                    }
                    else
                    {
                        dateTimePicker1.Enabled = true;
                        btnSave.Enabled = true;
                        lbUserMessage.Visible = false;

                    }
                    return true;

                case clsTestTypes.enTestType.StreetTest:
                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    if (!clsTest.IsPassed(_LocalDrivingLicenseApplicationID, (int)clsTestTypes.enTestType.WrittenTest))
                    {
                        dateTimePicker1.Enabled = false;
                        btnSave.Enabled = false;
                        lbUserMessage.Visible = true;
                        lbUserMessage.Text = "Cannot Sechule, Written Test should be passed first";
                        return false;
                    }
                    else
                    {
                        dateTimePicker1.Enabled = true;
                        btnSave.Enabled = true;
                        lbUserMessage.Visible = false;

                    }
                    return true;
            }
            return false;
        }
        private bool _HandleAppointmentLockedConstraint()
        {
            //if appointment is locked that means the person already sat for this test
            //we cannot update locked appointment
            if (_TestAppointment.IsLocked)
            {
                lbUserMessage.Visible = true;
                lbUserMessage.Text = "Person already sat the test,appointment locked !";
                dateTimePicker1.Enabled = false;
                btnSave.Enabled = false;
                return false;
            }
            else
                lbUserMessage.Visible = false;
            return true;
        }
        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.AddMode && clsTestAppointment.IsThereAnActiveAppointment(_LocalDrivingLicenseApplicationID,(int)_TestTypeMode))
            {
                lbUserMessage.Text = "Person already have an active appointment for this test";
                btnSave.Enabled = false;
                dateTimePicker1.Enabled = false;
                lbUserMessage.Visible = true;
                return false;
            }

            return true;
        }

    }
}
