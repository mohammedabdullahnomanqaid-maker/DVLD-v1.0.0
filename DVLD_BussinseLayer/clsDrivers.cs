using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Data;

namespace BussinseLayer
{
   public class clsDrivers
    {
        public int DriverID { set; get; }
        public int DriverPersonID { set; get; }
        public int CreatedByUserID { set; get; }
        public DateTime CreatedDate { set; get; }
        private enum enMode { AddMode,UpdateMode};
        private enMode _Mode = enMode.AddMode;
        public clsBPeople PersonInfo
        {
            get { return clsBPeople.Find(DriverPersonID); }
        }
        public  clsDrivers()
        {
            this.DriverID = -1;
            this.DriverPersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now;
            _Mode = enMode.AddMode;
        }
       private clsDrivers(int DriverID,int DriverPersonID,int CreatedByUserID,DateTime CreatedDate)
        {
            this.DriverID = DriverID;
            this.DriverPersonID = DriverPersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;
            _Mode = enMode.UpdateMode;
        }

        private bool _GetDriverIDIFExist()
        {
          this.DriverID= clsDriversData.IsPersonDriver(DriverPersonID);
            return (DriverID != -1);
            // incase return -1 that mean the person not driver yet. else person already driver
        }

        private bool _AddnewDriver()
        {
            if (!_GetDriverIDIFExist())//we check here before add new driver because the person become driver on time in the system.
               this.DriverID= clsDriversData.AddNewDriver(DriverPersonID, CreatedByUserID);

            return (this.DriverID != -1);
        }
        static public DataTable GetAllDrivers()
        {
            return clsDriversData.GetAllDrivers();
        }

        static public clsDrivers Find(int DriverID)
        {
            int PersonID = -1, CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;
            bool IsFound = clsDriversData.GetDriverInfoByDriverID(DriverID,ref PersonID,ref CreatedByUserID,ref CreatedDate);

            if (IsFound)
                return new clsDrivers(DriverID, PersonID, CreatedByUserID, CreatedDate);
            else
                return null;
        }
        static public int GetDriverIDByApplicationID(int ApplicationID)
        {
            return clsDriversData.GetDriverIDByApplicationID(ApplicationID);
        }
        static public clsDrivers FindByPersonID(int PersonID)
        {
            int DriverID = -1, CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;
            bool IsFound = clsDriversData.GetDriverInfoByPersonID(PersonID, ref DriverID, ref CreatedByUserID, ref CreatedDate);

            if (IsFound)
                return new clsDrivers(DriverID, PersonID, CreatedByUserID, CreatedDate);
            else
                return null;
        }
        private bool _UpdateDriver()
        {
            return false;//no implementation because there is no update yet
        }
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddMode:
                    if (_AddnewDriver())
                    {
                        _Mode = enMode.UpdateMode;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.UpdateMode:
                    return _UpdateDriver();
            }
            return false;
        }
    }
}
