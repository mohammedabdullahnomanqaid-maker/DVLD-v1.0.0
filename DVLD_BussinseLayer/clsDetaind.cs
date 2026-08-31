using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccessLayer;

namespace BussinseLayer
{
   public class clsDetaind
    {
        public int DetainID { get; set; }
        public int LicenseID { set; get; }
        public int CreatedByUser { set; get; }
        public int ReleasedByUserID { set; get; }
        public int ReleaseApplicationID { set; get; }
        public DateTime ReleaseDate { set; get; }
        public DateTime DetainDate { set; get; }
        public bool IsRelease { set; get; }
        public decimal FineFees { set; get; }
        enum enMode { AddMode=1,UpdateMode=2 };
        enMode _Mode = enMode.AddMode;

        public clsDetaind()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.CreatedByUser = -1;
            this.ReleasedByUserID = -1;
            this.ReleaseApplicationID = -1;
            this.DetainDate = DateTime.Now;
            this.ReleaseDate = DateTime.Now;
            this.IsRelease = false;
            this.FineFees = 0;
            _Mode = enMode.AddMode;
        }
        clsDetaind(int DetainID,int LicenseID,int CreatedByUser,int ReleasedByUserID,int ReleaseApplicationID,DateTime DetainDate,DateTime ReleaseDate,bool IsRelease,decimal FineFees)
        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.CreatedByUser = CreatedByUser;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleaseApplicationID;
            this.DetainDate = DetainDate;
            this.ReleaseDate = ReleaseDate;
            this.IsRelease = IsRelease;
            this.FineFees = FineFees;
            _Mode = enMode.UpdateMode;
        }
        static public DataTable GetAllDetaind()
        {
            return clsDetaindData.GetAllDetaind();
        }

        private bool _AddNewDetainLecense()
        {
            this.DetainID = clsDetaindData.AddNewDetaindLicense(LicenseID, CreatedByUser, DetainDate, IsRelease, FineFees);
            return (this.DetainID != -1);
        }

        public bool ReleaseDetain(int ReleasedByUserID,int ReleaseApplicationID)
        {
            return clsDetaindData.ReleaseDetainLicense(this.DetainID,ReleasedByUserID,ReleaseApplicationID);
        }
        static public clsDetaind Find(int DetainID)
        {
            int LicenseID=-1, CreatedByUserID = -1, ReleaseByUserID = -1, ReleaseApplicationID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.Now;
            decimal FineFees = 0;
            bool IsRelease = false;

            bool IsFound = clsDetaindData.GetDetainInfoByDetainID(DetainID,ref LicenseID,ref DetainDate,ref FineFees,ref CreatedByUserID,ref IsRelease,ref ReleaseDate,ref ReleaseByUserID,ref ReleaseApplicationID);

            if (IsFound)
                return new clsDetaind(DetainID, LicenseID, CreatedByUserID, ReleaseByUserID, ReleaseApplicationID, DetainDate, ReleaseDate, IsRelease, FineFees);
            else
                return null;
        }

        static public int GetDetainIDByLicenseID(int LicenseID)
        {
            return clsDetaindData.GetDetainIDByLicenseID(LicenseID);
        }
       static public bool IsLicenseDetaind(int LicenseID)
        {
            return clsDetaindData.IsLicenseDetaind(LicenseID);
        }
        public void MakeModeUpdate()
        {
            _Mode = enMode.UpdateMode;
        }
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddMode:
                    if (_AddNewDetainLecense())
                    {
                        _Mode = enMode.UpdateMode;
                        return true;
                    }
                    else
                        return false;

                case enMode.UpdateMode:
                    return false;
            }
            return false;
        }
    }
}
