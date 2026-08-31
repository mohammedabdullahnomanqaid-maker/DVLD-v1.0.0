using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccessLayer;

namespace BussinseLayer
{
    public class clsTestTypes
    {
        public enum enTestType { VisionTest=1,WrittenTest=2,StreetTest=3};
        public clsTestTypes.enTestType TestTypeID { set; get; }
        public string TestTypeTitle { set; get; }
        public string TestTypeDescription { set; get; }
        public decimal TestTypesFees { set; get; }

        clsTestTypes(string TestTypeTitle, string TestTypeDescription, decimal TestTypesFees)
        {
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;
            this.TestTypesFees = TestTypesFees;
        }
        static public DataTable GetAllTestTypes()
        {
            return clsTestTypesData.GetAllTestTypes();
        }

        static public clsTestTypes Find(int TestTypeID)
        {
            string TestTypeTitle = string.Empty,TestTypeDescription=string.Empty;
            decimal TestTypesFees = 0;
            bool IsFound = clsTestTypesData.GetTestTypeByTestTypeID(TestTypeID, ref TestTypeTitle, ref TestTypeDescription,ref TestTypesFees);

            if (IsFound)
                return new clsTestTypes(TestTypeTitle, TestTypeDescription, TestTypesFees);
            else
                return null;
        }

        private bool _UpdateTestTypes()
        {
            return clsTestTypesData.UpdateTestType((int)TestTypeID, TestTypeTitle, TestTypeDescription, TestTypesFees);
        }

        public bool Save()
        {
            if (_UpdateTestTypes())
                return true;
            else
                return false;
        }

    }
}
