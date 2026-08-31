using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Data;

namespace BussinseLayer
{
   public class clsLicense
    {
        public int LicenseID { set; get; }
        public int LicenseClassID { set; get; }
        public int CreatedByUserID { set; get; }
        public int ApplicationID { set; get; }
        public int DriverID { set; get; }
        public enIssueReason IssueReasonID { set; get; }
        public string Notes { set; get; }
        public DateTime IssueeDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public decimal PaidFees { set; get; }
        public bool IsActive { set; get; }
        private enum enMode { AddMode,UpdateMode};
        private enMode _Mode = enMode.AddMode;
        public clsDrivers DriverInfo
        {
            get { return clsDrivers.FindByPersonID(clsPeople.GetPersonIDByLicenseID(LicenseID)); }
        }
        public clsLicenseClass LicenseClassInfo
        {
            get { return clsLicenseClass.Find(LicenseClassID); }
        }
        public enum enIssueReason { FirstTime=1,Renew=2,LostReplacement=3,DamageReplacement=4};
        public string IssueReasonTest
        {
            get
            {
                switch ((clsApplications.enApplicationType)IssueReasonID)
                {
                    case clsApplications.enApplicationType.NewApplication:
                        return "First Time";

                    case clsApplications.enApplicationType.RenewApplication:
                        return "Renew";

                    case clsApplications.enApplicationType.ReplaceForDamage:
                        return "Replace for Damage";

                    case clsApplications.enApplicationType.ReplaceForLost:
                        return "Replace for Lost";
                  
                }
                return "First Time";
            }
        }
       public clsLicense()
        {
            this.LicenseID = -1;
            this.DriverID = -1;
            this.CreatedByUserID = -1;
            this.LicenseClassID = -1;
            this.ExpirationDate = DateTime.Now;
            this.IssueeDate = DateTime.Now;
            this.IsActive = false;
            this.PaidFees = 0;
            this.Notes = string.Empty;
            this.IssueReasonID = enIssueReason.FirstTime;
            this.ApplicationID = -1;
            _Mode = enMode.AddMode;
        }
        public clsDetaind DetainInfo
        {
            get
            {
                return clsDetaind.Find(clsDetaind.GetDetainIDByLicenseID(LicenseID));
            }
        }
        public bool Release(int UserID,ref int ApplicationID)
        {
            clsApplications _Application = new clsApplications();
            _Application.ApplicationDate = DateTime.Now;
            _Application.ApplicationStatusDate = DateTime.Now;
            _Application.ApplicationTypeID = Convert.ToInt32(clsApplications.enApplicationType.ReleaseDetaind);
            _Application.ApplicationStatus =clsApplications.enApplicationStatus.Complete;
            _Application.CreatedByUser =UserID;
            _Application.PaidFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.ReleaseDetaind).ApplicationTypesFees;
            _Application.ApplicationPersonID = this.DriverInfo.DriverPersonID;
            if (!_Application.Save())
            {
                ApplicationID = -1;
                return false;
            }

            ApplicationID = _Application.ApplicationID;

            return this.DetainInfo.ReleaseDetain(UserID,_Application.ApplicationID);
        }
        private  clsLicense(int LicenseID,int DriverID,int ApplicationID,int LicenseClassID,int CreatedByUserID,DateTime ExpirationDate,DateTime IssueeDate,bool IsActive,decimal PaidFees,string Notes,enIssueReason IssueReason)
        {
            this.LicenseID = LicenseID;
            this.DriverID = DriverID;
            this.CreatedByUserID = CreatedByUserID;
            this.LicenseClassID = LicenseClassID;
            this.ExpirationDate = ExpirationDate;
            this.IssueeDate = IssueeDate;
            this.IsActive = IsActive;
            this.PaidFees = PaidFees;
            this.Notes = Notes;
            this.IssueReasonID = IssueReason;
            this.ApplicationID = ApplicationID;
            _Mode = enMode.UpdateMode;
        }

        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1, DriverID = -1, CreatedByUserID = -1, LicenseClassID = -1;
            short IssueReason = -1;
            string Notes = string.Empty;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            decimal PaidFees = 0;
            bool IsActive = false;

            bool IsFound = clsLicenseData.GetLicenseInfoByLicenseID(LicenseID,ref ApplicationID, ref DriverID, ref CreatedByUserID, ref LicenseClassID, ref IssueReason, ref Notes, ref IssueDate, ref ExpirationDate, ref PaidFees, ref IsActive);

            if (IsFound)
                return new clsLicense(LicenseID,DriverID, ApplicationID, LicenseClassID, CreatedByUserID, ExpirationDate, IssueDate, IsActive, PaidFees, Notes,(enIssueReason) IssueReason);
            else
               return null;
        }

        public static clsLicense FindByDriverID(int DriverID)
        {
            int ApplicationID = -1, LicenseID = -1, CreatedByUserID = -1, LicenseClassID = -1;
            short IssueReason = -1;
            string Notes = string.Empty;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            decimal PaidFees = 0;
            bool IsActive = false;

            bool IsFound = clsLicenseData.GetLicenseInfoByDriverID(DriverID, ref ApplicationID, ref LicenseID, ref CreatedByUserID, ref LicenseClassID, ref IssueReason, ref Notes, ref IssueDate, ref ExpirationDate, ref PaidFees, ref IsActive);

            if (IsFound)
                return new clsLicense(LicenseID,DriverID, ApplicationID, LicenseClassID, CreatedByUserID, ExpirationDate, IssueDate, IsActive, PaidFees, Notes,(enIssueReason) IssueReason);
            else
                return null;
        }

        public static DataTable GetLicenseInfoOfHistoryByDriverID(int DriverID)
        {
            return clsLicenseData.GetLicenseInfoOfHistoryByDriverID(DriverID);
        }
        public bool IsLicenseExpired()
        {
            return (this.ExpirationDate.CompareTo(DateTime.Now) < 0);
        }
        public static int GetActiveLicenseIDByPersonID(int PersonID,int LicenseClass)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClass);
        }
        static public bool IsLicenseExistByPersonID(int PersonID,int LicenseClass)
        {
            return (GetActiveLicenseIDByPersonID(PersonID, LicenseClass) != -1);
        }
        private bool _AddNewLicense()
        {
          

            this.LicenseID = clsLicenseData.AddNewLicense(ApplicationID, DriverID, CreatedByUserID, LicenseClassID,Convert.ToInt16(IssueReasonID), Notes, IssueeDate, ExpirationDate, PaidFees, IsActive);

            return (this.LicenseID != -1);
        }
         
        private bool _UpdateLicense()
        {
            return false;//no implenetation yet for update license.
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddMode:
                    if (_AddNewLicense())
                    {
                        _Mode = enMode.UpdateMode;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.UpdateMode:
                   return _UpdateLicense();
            }
            return false;
        }

        static public bool MakeLicenseInActive(int LicenseID)
        {
            return clsLicenseData.MakeLicenseInActive(LicenseID);
        }

        static public bool IsActiveLicense(int LicenseID)
        {
            return clsLicenseData.IsActiveLicense(LicenseID);
        }
        public void DeactiveLicense()
        {
            clsLicense.MakeLicenseInActive(this.LicenseID); 
        }
        public clsLicense RenewLicense(string Note,int CreatedByUserID)
        {

            clsApplications _Application = new clsApplications();
            _Application.ApplicationDate = DateTime.Now;
            _Application.ApplicationStatusDate = DateTime.Now;
            _Application.ApplicationTypeID = Convert.ToInt32(clsApplications.enApplicationType.RenewApplication);
            _Application.ApplicationStatus = clsApplications.enApplicationStatus.Complete;
            _Application.CreatedByUser = CreatedByUserID;
            _Application.PaidFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.RenewApplication).ApplicationTypesFees;
            _Application.ApplicationPersonID = this.DriverInfo.DriverPersonID ;
            if (!_Application.Save())
            {
                return null;
            }

            clsLicense _License = new clsLicense();
            _License.ApplicationID = _Application.ApplicationID;
            _License.CreatedByUserID = CreatedByUserID;
            _License.DriverID = DriverID;
            _License.IsActive = true;

            _License.IssueeDate = DateTime.Now;
            _License.ExpirationDate = DateTime.Now.AddYears(clsLicenseClass.Find((int)clsApplications.enApplicationType.RenewApplication).DefaultValidityLength);
            _License.IssueReasonID = enIssueReason.Renew;
            _License.LicenseClassID = this.LicenseClassID;
            _License.Notes = Note;
            _License.PaidFees = this.PaidFees;

            if (!_License.Save())
            {
                return null;
            }

            DeactiveLicense();

            return _License;
        }
   
        public clsLicense Replace(enIssueReason IssueReason,int UserID)
        {

            clsApplications _Application = new clsApplications();

                _Application.ApplicationDate = DateTime.Now;
                _Application.ApplicationStatusDate = DateTime.Now;
                _Application.ApplicationTypeID = (int)IssueReason;
                _Application.PaidFees = clsApplicationTypes.Find((int)IssueReason).ApplicationTypesFees;
                _Application.ApplicationStatus = clsApplications.enApplicationStatus.Complete;
                _Application.CreatedByUser =UserID;
                _Application.ApplicationPersonID = this.DriverInfo.DriverPersonID;
           
            if (!_Application.Save())
            {
                return null;
            }

            clsLicense _License = new clsLicense();

            _License.IssueReasonID =IssueReason;
            _License.PaidFees = 0;//no fee because it is replacement.
            _License.ExpirationDate = this.ExpirationDate;
            _License.ApplicationID = _Application.ApplicationID;
            _License.DriverID = this.DriverID;
            _License.IsActive = true;
            _License.IssueeDate = DateTime.Now;
            _License.LicenseClassID = this.LicenseClassID;
            _License.Notes = this.Notes;
            _License.CreatedByUserID = UserID;

            if (!_License.Save())
            {
                return null;
            }

            DeactiveLicense();
            return _License;

        }

        public int Detain(decimal Fees,int UserID)
        {

            clsDetaind _Detaind = new clsDetaind();

            _Detaind.DetainDate = DateTime.Now;
            _Detaind.CreatedByUser = UserID;
            _Detaind.FineFees = Fees;
            _Detaind.LicenseID = this.LicenseID;

            if (!_Detaind.Save())
            {
                return -1;
            }

            return _Detaind.DetainID;
        }

        static public int GetActiveLicenseIDWhenLicenseIsOrdinaryLicenseClassByPersonID(int PersonID)
        {
            return GetActiveLicenseIDByPersonID(PersonID,3);
        }


    }
}
