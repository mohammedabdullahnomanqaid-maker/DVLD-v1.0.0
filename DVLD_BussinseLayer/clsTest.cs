using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BussinseLayer
{
   public class clsTest
    {
     public int TestID { set; get; }
     public int TestAppointmentID { set; get; }
     public string Note { set; get; }
     public int CreatedByUserID { set; get; }
     public bool TestResault { set; get; }

        private enum enMode { AddMode,UpdateMode};
        private enMode _Mode = enMode.AddMode;

       public clsTest()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.CreatedByUserID = -1;
            this.Note = "";
            this.TestResault = false;
            _Mode = enMode.AddMode;
        }

        private clsTest(int TestAppointmentID,int CreatedByUserID,string Note,bool TestResault)
        {
            this.TestAppointmentID = TestAppointmentID;
            this.CreatedByUserID = CreatedByUserID;
            this.Note = Note;
            this.TestResault = TestResault;
            _Mode = enMode.UpdateMode;
        }

        private bool _AddNewTest()
        {
            this.TestID= clsTestData.AddNewTest(TestAppointmentID, TestResault, Note, CreatedByUserID);
            return (this.TestID != -1);
        }

        private bool _UpdateNewTest()
        {
            return clsTestData.UpdateTest(TestID,TestAppointmentID, TestResault, Note, CreatedByUserID);
        }
        static public byte GetTestPassedCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }
        static public bool IsPassed(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            return clsTestData.IsPassTest(LocalDrivingLicenseApplicationID, TestTypeID);
        }
        static public int GetTestIDByAppointmentID(int TestAppointmentID)
        {
            return clsTestData.GetTestIDByAppointmentID(TestAppointmentID);
        }
        static public clsTest Find(int TestID)
        {
            int TestAppointmentID = -1,CreatedByUserID=-1;
            string Note = string.Empty;
            bool TestResault = false;

            bool IsFound = clsTestData.GetTestInfoByID(TestID,ref TestAppointmentID,ref Note,ref CreatedByUserID,ref TestResault);

            if (IsFound)
                return new clsTest(TestAppointmentID, CreatedByUserID, Note, TestResault);
            else
                return null;

        }
        static public bool DoesAttendBefore(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            return clsTestData.DoesAttendBefore(LocalDrivingLicenseApplicationID, TestTypeID);
        }
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddMode:
                    if (_AddNewTest())
                    {
                        _Mode = enMode.UpdateMode;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.UpdateMode:
                    return _UpdateNewTest();
            }
            return false;
        }
       static public bool PassedAllTest(int LocalDrivingLicenseApplicationID)
        {
            return (GetTestPassedCount(LocalDrivingLicenseApplicationID) == 3);
        }
    }
}
