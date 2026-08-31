using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Data;

namespace BussinseLayer
{
   public class clsTestAppointment
    {
        public int TestAppointmentID { set; get; }
        public int TestTypeID { set; get; }
        public int LocalDrivingLicenseApplicationID { set; get; }
        public int CreatedByUserID { set; get; }
        public int RetakeTestAppointmentID { set; get; }
        public DateTime AppointmentDate { set; get; }
        public decimal PaidFees { set; get; }
        public bool IsLocked { set; get; }
        public clsApplications RetakeApplicationInfo
        {
            get { return clsApplications.Find(RetakeTestAppointmentID); }
        }
        //private clsLocalDrivingLicenseApplication _LocalDrivigLicenseApplication;
        ////composition.
        //public clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication
        //{
        //    get { return _LocalDrivigLicenseApplication; }
        //}
        enum enMode { AddMode,UpdateMode};
        private enMode _Mode = enMode.AddMode;

        public int TestID
        {
            get { return clsTest.GetTestIDByAppointmentID(TestAppointmentID); }
        }
        private enum enTestType { eVision = 1, eWritten = 2, eStreet = 3 };
        private enTestType _TestTypeMode = enTestType.eVision;

        public clsTestAppointment()
        {
            this.TestAppointmentID = -1;
            this.CreatedByUserID = -1;
            this.TestTypeID = -1;
            this.RetakeTestAppointmentID = -1;
            this.LocalDrivingLicenseApplicationID = -1;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.IsLocked = false;
            _Mode = enMode.AddMode;
        }

       private clsTestAppointment(int LocalDrivingLicenseApplicationID, int RetakeTestAppointmentID, DateTime AppointmentDate, decimal PaidFees,bool IsLocked)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.IsLocked = IsLocked;
            this.RetakeTestAppointmentID = RetakeTestAppointmentID;
           // this._LocalDrivigLicenseApplication = clsLocalDrivingLicenseApplication.Find(this.LocalDrivingLicenseApplicationID);
            _Mode = enMode.UpdateMode;
        }
        public static clsTestAppointment Find(int TestAppointmentID)
        {
            DateTime AppointmentDate = DateTime.Now;
            decimal PaidFees = 0;
            bool IsLocked = false;
            int LocalDrivingLicenseApplicationID = -1;
            int RetakeTestAppointmentID = -1;

            bool IsFound = clsTestAppointmentData.GetTestAppointmentByTestAppointmentID( TestAppointmentID,ref RetakeTestAppointmentID, ref LocalDrivingLicenseApplicationID, ref AppointmentDate,ref PaidFees,ref IsLocked);
            if (IsFound)
                return new clsTestAppointment(LocalDrivingLicenseApplicationID, RetakeTestAppointmentID, AppointmentDate, PaidFees, IsLocked);
            else
                return null;
        }

        public static DataTable GetTestAppointmentPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsTestAppointmentData.GetTestAppointmentPerTest(LocalDrivingLicenseApplicationID,TestTypeID);
        }

        public static bool IsThereAnActiveAppointment(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            return clsTestAppointmentData.IsThereAnActiveAppointment(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static bool IsNotTestAppointmentLockedByTestAppointmentID(int TestAppointmentID, int TestTypeID)
        {
            return clsTestAppointmentData.IsNotTestAppointmentLockedByTestAppointmentID(TestAppointmentID, TestTypeID);
        }

        public static bool LockTestAppointment(int TestAppointmentID,bool IsLocked)
        {
            return clsTestAppointmentData.LockTestAppointment(TestAppointmentID, IsLocked);
        }
        private bool _AddNewTestAppointment()
        {
            this.TestAppointmentID= clsTestAppointmentData.AddNewTestAppointment(this.TestTypeID,this.LocalDrivingLicenseApplicationID,this.AppointmentDate,this.PaidFees,this.CreatedByUserID,this.IsLocked,this.RetakeTestAppointmentID);
            return (this.TestAppointmentID != -1);
        }
        private bool _UpdateTestAppointment()
        {
            return clsTestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, this.AppointmentDate);
        }
        static public int GetTrailNumber(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            return clsTestAppointmentData.GetTrailNumber(LocalDrivingLicenseApplicationID, TestTypeID);
        }
       static public bool GetMaxTestTypeID(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            
            int LevelOfTest= clsTestAppointmentData.GetMaxTestTypeID(LocalDrivingLicenseApplicationID);

            bool IsPass = false;
            switch ((enTestType)TestTypeID)
            {
                case enTestType.eVision:
                    IsPass = (LevelOfTest == (int)enTestType.eVision);
                    break;

                case enTestType.eWritten:
                    IsPass = (LevelOfTest == (int)enTestType.eWritten);
                    break;

                case enTestType.eStreet:
                    IsPass = (LevelOfTest == (int)enTestType.eStreet);
                    break;
            }
            return IsPass;
        }
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddMode:
                    if (_AddNewTestAppointment())
                    {
                        _Mode = enMode.UpdateMode;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.UpdateMode:
                    return _UpdateTestAppointment();
            }
            return false;
        }
    }
}
