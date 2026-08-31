using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Data;

namespace BussinseLayer
{
   public class clsInternationalLicense:clsApplications
    {
        private int _InternationalLicenseID=-1;
        public int InternationalLicenseID { get { return _InternationalLicenseID; } }
        public int DriverID { set; get; }
        public int IssuedUsingLocalLicense { set; get; }
        public int CreatedByUserID { set; get; }
        public DateTime IssuedDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public bool IsActive { set; get; }
        public enum enMode { AddMode,Updatemode};
        private enMode _Mode = enMode.AddMode;
        public clsInternationalLicense()
        {
            this.ApplicationTypeID = (int)enApplicationType.NewInternational;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationStatusDate = DateTime.Now;
            this.ApplicationStatus = enApplicationStatus.Complete;
            this.IssuedDate = DateTime.Now;

            this._InternationalLicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicense = -1;
            this.CreatedByUserID = -1;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = false;
            _Mode = enMode.AddMode;
        }

        public clsInternationalLicense(int InternationalLicenseID,int ApplicationID,int DriverID,
            int IssuedUsingLocalLicense,int CreatedByUserID,DateTime ExpirationDate,DateTime IssuedDate,
            bool IsActive, int ApplicationPersonID,int ApplicationTypeID,int CreatedByUser,
            DateTime ApplicationDate,DateTime ApplicationStatusDate,enApplicationStatus ApplicationStatus,decimal FaidFees)
            : base(ApplicationID,ApplicationPersonID,ApplicationTypeID,CreatedByUser,
             ApplicationDate,ApplicationStatusDate,ApplicationStatus,FaidFees)
        {
            this._InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicense = IssuedUsingLocalLicense;
            this.CreatedByUserID = CreatedByUserID;
            this.ExpirationDate = ExpirationDate;
            this.IssuedDate = IssuedDate;
            this.IsActive = IsActive;
            _Mode = enMode.Updatemode;
        }
       static public DataTable GetAllInternationalLicense()
        {
            return clsInternationalLicenseData.GetAllInternationalLicense();
        }
        static public DataTable GetAllInternationalLicenseByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetAllInternationalLicenseByDriverID(DriverID);
        }
        private bool _AddNewInternationalLicense()
        {
            this._InternationalLicenseID = clsInternationalLicenseData.AddNewInterNationalLicense(base.ApplicationID, DriverID, CreatedByUserID, IssuedUsingLocalLicense, IssuedDate, ExpirationDate, IsActive);

            return (this._InternationalLicenseID != -1);
        }
        private bool _UpdateInternationalLicense()
        {
            return false;
        }
        static public clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1, CreatedUserID = -1, DriverID = -1, IssuedUsingLicenseID = -1;
            DateTime IssuedDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = false;


            bool IsFound = clsInternationalLicenseData.GetInternationalLicenseInfoByInternationalID(InternationalLicenseID,ref ApplicationID,ref DriverID,ref CreatedUserID,ref IssuedUsingLicenseID,ref IssuedDate,ref ExpirationDate,ref IsActive);

            if (IsFound)
            {
                clsApplications applications = clsApplications.Find(ApplicationID);
                return new clsInternationalLicense(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLicenseID, CreatedUserID, ExpirationDate, IssuedDate, IsActive,applications.ApplicationPersonID, applications.ApplicationTypeID, CreatedUserID, applications.ApplicationDate, applications.ApplicationStatusDate,applications.ApplicationStatus, applications.PaidFees);

            }
            else
                return null;
        }
        static public int GetInternationalLicenseIDIfExist(int LicenseID)
        {
            return clsInternationalLicenseData.GetInternationalLicenseIDIfExist(LicenseID);
        }
        public bool Save()
        {
            base.Mode = (clsApplications.enMode)_Mode;
            if (!base.Save())
                return false;

            switch (_Mode)
            {
                case enMode.AddMode:
                    if (_AddNewInternationalLicense())
                    {
                        _Mode = enMode.Updatemode;
                        return true;
                    }
                    else
                        return false;
                case enMode.Updatemode:
                    return _UpdateInternationalLicense();

            }
            return false;
        }
    }
}
