using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccessLayer;

namespace BussinseLayer
{
   public class clsApplications
    {
        public int ApplicationID { get; set; }
        public int ApplicationPersonID { get; set; }
        public int ApplicationTypeID { get; set; }
        public int CreatedByUser { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime ApplicationStatusDate { get; set; }
        public enApplicationStatus ApplicationStatus { get; set; }
        public decimal PaidFees { get; set; }
        public clsUsers CreatedByUserInfo;
        public clsApplicationTypes ApplicationTypeInfo;
        public string FullName
        {
            get { return clsBPeople.Find(ApplicationPersonID).FullName; }
        }
        public string StatusText
        {
            get
            {
                switch (ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancel:
                        return "Canceled";
                    case enApplicationStatus.Complete:
                        return "Compeleted";
                    default:
                        return "unknown";
                }
            }
        }
        public enum enMode { AddMode,UpdateMode};
        public enMode Mode = enMode.AddMode;

        public enum enApplicationType { NewApplication=1,RenewApplication=2,ReplaceForLost=3,ReplaceForDamage=4,ReleaseDetaind=5,NewInternational=6,RetakeTest=7};
        public enApplicationType ClassTestType;

        public enum enApplicationStatus { New=1,Cancel=2,Complete=3};

      public clsApplications()
        {
            
            this.ApplicationDate = DateTime.Now;
            this.ApplicationStatusDate = DateTime.Now;

            this.ApplicationID = -1;
            this.ApplicationPersonID = -1;
            this.CreatedByUser = -1;
            this.ApplicationStatus = enApplicationStatus.New;
            this.PaidFees = 0;
            Mode = enMode.AddMode;
        }
       protected clsApplications(int ApplicationID,int ApplicationPersonID,int ApplicationTypeID,int CreatedByUser,
           DateTime ApplicationDate,DateTime ApplicationStatusDate,enApplicationStatus ApplicationStatus,decimal PaidFees)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicationPersonID = ApplicationPersonID;
            this.ApplicationTypeID = ApplicationTypeID;
            this.CreatedByUser = CreatedByUser;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationStatusDate = ApplicationStatusDate;
            this.ApplicationStatus = ApplicationStatus;
            this.PaidFees = PaidFees;
            this.CreatedByUserInfo = clsUsers.FindUserByUserID(CreatedByUser);
            this.ApplicationTypeInfo = clsApplicationTypes.Find(ApplicationTypeID);
            Mode = enMode.UpdateMode;
        }
       private bool _UpdatLocalApplication()
        {
            return clsApplicationsData.UpdatLocalApplication(this.ApplicationID,this.ApplicationPersonID,this.ApplicationTypeID,this.CreatedByUser,this.ApplicationDate,this.ApplicationStatusDate,(int) this.ApplicationStatus,this.PaidFees);
        }
        private bool _AddNewLocalDrivingLicenseApplication()
        {
            this.ApplicationID = clsApplicationsData.AddNewApplication(this.ApplicationPersonID,this.ApplicationTypeID,this.CreatedByUser,this.ApplicationDate,this.ApplicationStatusDate,(int)this.ApplicationStatus,this.PaidFees);
            return (this.ApplicationID != -1);
        }
      
        static public int GetActiveApplicationIDForLicenseClass(int PersonID,int ApplicationType,int LicenseClass)
        {
            return clsApplicationsData.GetActiveApplicationIDForLicenseClass(PersonID, ApplicationType, LicenseClass);
        }
        public bool SetCompelete()
        {
            return clsApplicationsData.UpdateApplicationStatus(this.ApplicationID, (int)clsApplications.enApplicationStatus.Complete);
        }
        public bool Cancel()
        {
            return clsApplicationsData.UpdateApplicationStatus(this.ApplicationID,(int)clsApplications.enApplicationStatus.Cancel);
        }

        public bool Delete(int ApplicationID)
        {
            return clsApplicationsData.DeleteApplication(ApplicationID);
        }
        static public clsApplications Find(int ApplicationID)
        {
            int  ApplicantPersonID = -1, ApplicationTypeID = -1,
            CreatedByUserID = -1;DateTime ApplicationDate = DateTime.Now, LastStatusDate = DateTime.Now;
            int ApplicationStatus =1; decimal PaidFees = 0;


            bool IsFound = clsApplicationsData.GetApplicationByID( ApplicationID,ref ApplicantPersonID, ref ApplicationTypeID,
           ref CreatedByUserID, ref ApplicationDate, ref LastStatusDate,
           ref  ApplicationStatus, ref PaidFees);

            if (IsFound)
                return new clsApplications(ApplicationID,ApplicantPersonID, ApplicationTypeID, CreatedByUserID, ApplicationDate, LastStatusDate,(enApplicationStatus) ApplicationStatus, PaidFees);
            else
                return null;
        }
        static public bool IsClassLicenseComplete(int ApplicationID,int LicenseClassID)
        {
            return clsApplicationsData.IsClassLicenseComplete(ApplicationID, LicenseClassID);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddMode:
            if (_AddNewLocalDrivingLicenseApplication())
            {
                Mode = enMode.UpdateMode;
                return true;
            }
            else
            {
                return false; 
            }
                case enMode.UpdateMode:
                    return _UpdatLocalApplication();
            }
            return false;
    }
      
    }
}
