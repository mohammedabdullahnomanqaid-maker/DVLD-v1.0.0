using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Data;

namespace BussinseLayer
{
   public class clsLicenseClass
    {
        public int LicenseClassID { set; get; }
        public string ClassName { set; get; }
        public string ClassDescription { set; get; }
        public int MinimiumAllowedAge { set; get; }
        public int DefaultValidityLength { set; get; }
        public decimal ClassFees { set; get; }
        private enum enMode { AddMode,UpdateMode};
        private enMode _Mode = enMode.AddMode;
       public clsLicenseClass()
        {
            this.LicenseClassID = -1;
            this.ClassName = string.Empty;
            this.ClassDescription = string.Empty;
            this.MinimiumAllowedAge = -1;
            this.DefaultValidityLength = -1;
            this.ClassFees = 0;
            _Mode = enMode.AddMode;
        }

       private clsLicenseClass(string ClassName,string ClassDescription,short MinimiumAllowedAge,int DefaultValidityLength,decimal ClassFees)
        {
            this.ClassName = ClassName;
            this.ClassDescription = string.Empty;
            this.MinimiumAllowedAge = MinimiumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;
            _Mode = enMode.UpdateMode;
        }

        static public DataTable GetAllLicenseClass()
        {
            return clsLicenseClassData.GetAllLicenseClass();
        }

       static public clsLicenseClass Find(int LicenseClassID)
        {
            string ClassName = string.Empty, ClassDescription = string.Empty;
            short MinimiumAllowedAge = -1, DefaultValidityLength = -1;
            decimal ClassFees = 0;

            bool IsFound = clsLicenseClassData.GetLicenseInfoByID(LicenseClassID,ref ClassName,ref ClassDescription,ref MinimiumAllowedAge,ref DefaultValidityLength,ref ClassFees);

            if (IsFound)
                return new clsLicenseClass(ClassName, ClassDescription, MinimiumAllowedAge, DefaultValidityLength, ClassFees);
            else
                return null;
        }

    }
}
