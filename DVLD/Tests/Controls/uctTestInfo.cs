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
    public partial class uctTestInfo : UserControl
    {
        private clsTestAppointment _TestAppointment;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        public int LocalDrivingLiceseApplicationID = -1;
        private int _TestID = -1;
        public int TestID
        {
            get { return _TestID; }
        }

        private int _TestAppointmentID = -1;
        public int TestAppointmentID
        {
            get { return _TestAppointmentID; }
        }
        private clsTestTypes.enTestType _TestType;
        public clsTestTypes.enTestType TestType
        {
            get
            {
                return _TestType;
            }
            set
            { 
                _TestType = value;

                switch (_TestType)
                {
                    case clsTestTypes.enTestType.VisionTest:
                        gpTestType.Text = "Vision Test";
                        picTypeOfTest.Image = Properties.Resources.Vision_512;
                        break;

                    case clsTestTypes.enTestType.WrittenTest:
                        gpTestType.Text = "Written Test";
                        picTypeOfTest.Image = Properties.Resources.Written_Test_512;
                        break;

                    case clsTestTypes.enTestType.StreetTest:
                        gpTestType.Text = "Street Test";
                        picTypeOfTest.Image = Properties.Resources.driving_test_512;
                        break;
                }
            }
        }
        public uctTestInfo()
        {
            InitializeComponent();
            
        }
        
        private void _ResetDefaultValue()
        {
            lbLDLAppID.Text = "[???]";
            lbName.Text = "[???]";
            lbFees.Text = "[???]";
            lbTrail.Text = "[???]";
            lbDClass.Text = "[???]";

        }
        public void LoadTestAppointmentInfo(int TestAppointmentID)
        {

            _TestAppointment = clsTestAppointment.Find(TestAppointmentID);
            if (_TestAppointment == null)
            {
                _ResetDefaultValue();
                MessageBox.Show("Test Appointment ID =" + TestAppointmentID.ToString() + " Not Found !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _TestAppointment.TestAppointmentID = TestAppointmentID;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(_TestAppointment.LocalDrivingLicenseApplicationID);
            if (_LocalDrivingLicenseApplication == null)
            {
                _ResetDefaultValue();
                MessageBox.Show("L.D.L.App ID =" + _TestAppointment.LocalDrivingLicenseApplicationID.ToString() + " Not Found !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            lbDate.Text = _TestAppointment.AppointmentDate.ToString();
            lbLDLAppID.Text = _TestAppointment.LocalDrivingLicenseApplicationID.ToString();
            _TestAppointmentID = TestAppointmentID;
            LocalDrivingLiceseApplicationID = _TestAppointment.LocalDrivingLicenseApplicationID;
            _TestID =_TestAppointment.TestID;
            lbName.Text = _LocalDrivingLicenseApplication.FullName;
            lbFees.Text = clsTestTypes.Find((int)_TestType).TestTypesFees.ToString();
            lbTrail.Text = "0";
            lbDClass.Text = _LocalDrivingLicenseApplication.LicenseClass.ClassName ;
          
            lbTestID.Text = _TestID==-1 ? "Not Taken Yet.":_TestID.ToString();
        }
    
    }
}
