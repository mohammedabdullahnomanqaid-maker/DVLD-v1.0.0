using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Data;

namespace BussinseLayer
{
   public class clsLocalDrivingLicenseApplication :clsApplications
    {
        public int LocalDrivingLicenseApplicationID { set; get; }
        public int LicenseClassID { set; get; }
      
        private enum enMode { AddMode,UpdateMode};
        private enMode _Mode = enMode.AddMode;
        public clsLicenseClass LicenseClass
        {
            get { return clsLicenseClass.Find(this.LicenseClassID); }
        }
    
       public clsLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = -1;
            this.LicenseClassID = -1;
            _Mode = enMode.AddMode;
        }

         clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID,
             int LicenseClassID, int ApplicationPersonID, int ApplicationTypeID, int CreatedByUser,
             DateTime ApplicationDate, DateTime ApplicationStatusDate, int ApplicationStatus,
             decimal PaidFees)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.ApplicationID = ApplicationID;
            this.LicenseClassID = LicenseClassID;
            this.ApplicationPersonID = ApplicationPersonID;
            this.ApplicationTypeID = ApplicationTypeID;
            this.CreatedByUser = CreatedByUser;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationStatusDate = ApplicationStatusDate;
            this.ApplicationStatus =(enApplicationStatus) ApplicationStatus;
            this.PaidFees = PaidFees;
            _Mode = enMode.UpdateMode;
        }
        public bool DoesAttendBefore(int TestType)
        {
            return clsTest.DoesAttendBefore(this.LocalDrivingLicenseApplicationID, TestType);
        }
        public int GetTrailNumber(int TestTypeID)
        {
            return clsTestAppointment.GetTrailNumber(this.LocalDrivingLicenseApplicationID, TestTypeID);
        }
        private bool _AddNewLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(this.ApplicationID,this.LicenseClassID);
            return (this.LocalDrivingLicenseApplicationID != -1);
        }
        static public DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications();
        }
        public int GetActiveLicenseID()
        {
            return clsLicense.GetActiveLicenseIDByPersonID(this.ApplicationPersonID, this.LicenseClassID);
        }
        public byte GetTestPassedCount()
        {
            return clsTest.GetTestPassedCount(this.LocalDrivingLicenseApplicationID);
        }
        private bool _UpdatLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseApplicationData.UpdatLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID, this.ApplicationID, this.LicenseClassID);
        }
        static public bool IsLocalDrivingLicenseApplicationExistByPersonIDAndLicenseClassID(int PersonID,int LicenseClassID)
        {
            return clsLocalDrivingLicenseApplicationData.IsLocalDrivingLicenseApplicationExistByPersonIDAndLicenseClassID(PersonID, LicenseClassID);
        }
        static public bool FindLocalDrivingViewByLDLAppID(int LocalLicenseApplicationID, ref string ClassName, ref int PassedCountTest, ref string Status)
        {
           return clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationViewByID(LocalLicenseApplicationID,ref ClassName,ref PassedCountTest,ref Status);
        }
         public bool Delete()
        {
            bool IsLocalDrivingLicenseApplicationDeleted = false; 
            bool IsBaseApplicationDeleted = false;
            //First we delete the Local Driving License Application
            IsLocalDrivingLicenseApplicationDeleted = clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID);
            if (!IsLocalDrivingLicenseApplicationDeleted)
                return false;
            //Then we delete the base Application
            IsBaseApplicationDeleted = base.Delete(this.ApplicationID);
            return IsBaseApplicationDeleted;

        }
        static public clsLocalDrivingLicenseApplication Find(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = 0, LicenseClassID = 0;
            bool IsFound = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationByID(LocalDrivingLicenseApplicationID,ref ApplicationID,ref LicenseClassID);
            if (IsFound)
            {
                //if we find localDrivinglicensApplicationInfo so now we find Application
                clsApplications Application = clsApplications.Find(ApplicationID);

                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, ApplicationID,
                    LicenseClassID,Application.ApplicationPersonID,Application.ApplicationTypeID,
                    Application.CreatedByUser,Application.ApplicationDate,Application.ApplicationStatusDate,
                   (int) Application.ApplicationStatus,Application.PaidFees);

            }
            else
                return null;
            
        }
        public bool Save()
        {


            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.
            base.Mode = (clsApplications.enMode)_Mode;
            if (!base.Save())
                return false;


            //After we save the main application now we save the sub application.
            switch (_Mode)
            {
                case enMode.AddMode:
                    if (_AddNewLocalDrivingLicenseApplication())
                    {
                        _Mode = enMode.UpdateMode;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.UpdateMode:
                    return _UpdatLocalDrivingLicenseApplication();
            }
            return false;
        }
     
        public bool IsLicenseIssued()
        {
            return (GetActiveLicenseID() != -1);
        }
        public bool DoesPassedTest(clsTestTypes.enTestType TestType)
        {
            return clsTest.IsPassed(this.LocalDrivingLicenseApplicationID,(int)TestType);
        }
        public bool IsPassAllTest()
        {
            return clsTest.PassedAllTest(this.LocalDrivingLicenseApplicationID);
        }
        public int IssuedLicense(int UserID,string Note)
        {

          

            clsDrivers _Driver = new clsDrivers();
            _Driver.DriverPersonID = this.ApplicationPersonID;
            _Driver.CreatedByUserID = UserID;

            if (!_Driver.Save())
                return -1;


           clsLicense License = new clsLicense();
            License.CreatedByUserID = UserID;
            License.ApplicationID = this.ApplicationID;
            License.IsActive = true;
            License.IssueeDate = DateTime.Now;
            License.IssueReasonID = clsLicense.enIssueReason.FirstTime;
            License.LicenseClassID = this.LicenseClassID;
            License.Notes = Note;
            License.PaidFees = this.PaidFees;
            License.ExpirationDate = DateTime.Now.AddYears(clsLicenseClass.Find(this.LicenseClassID).DefaultValidityLength);
            License.DriverID = _Driver.DriverID;

            if (!this.SetCompelete())
                return -1;

            if (!License.Save())
                return -1;

            return License.LicenseID;

        }
    }

}
