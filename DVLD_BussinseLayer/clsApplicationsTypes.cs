using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccessLayer;

namespace BussinseLayer
{
    public class clsApplicationTypes
    {
        public int ApplicationTypeID { set; get; }
       public string ApplicationTypesTitle { set; get; }
       public decimal ApplicationTypesFees { set; get; }
        static public DataTable GetAllApplications()
        {
            return clsApllicationTypesData.GetApplicationTypes();
        }

        //no mode because we have just update .
        clsApplicationTypes(string ApplicationTypesTitle, decimal ApplicationTypesFees)
        {
            this.ApplicationTypesTitle = ApplicationTypesTitle;
            this.ApplicationTypesFees = ApplicationTypesFees;
        }

       static public clsApplicationTypes Find(int ApplicationTypesID)
        {
            string ApplicationTypesTitle = string.Empty;
            decimal ApplicationTypesFees = 0;
            bool IsFound = clsApllicationTypesData.GetApplicationTypeByApplicationID(ApplicationTypesID,ref ApplicationTypesTitle,ref ApplicationTypesFees);

            if (IsFound)
                return new clsApplicationTypes(ApplicationTypesTitle, ApplicationTypesFees);
            else
                return null;
        }

        private bool _UpdateApplicationTypes()
        {
            return clsApllicationTypesData.UpdateApplicationTypeTiteAndFees(ApplicationTypeID, ApplicationTypesTitle, ApplicationTypesFees);
        }

        static public decimal GetApplicationFees(int ApplicationID)
        {
            return clsApllicationTypesData.GetApplicationTypeFees(ApplicationID);
        }

        public bool Save()
        {
            if (_UpdateApplicationTypes())
                return true;
            else
                 return false;
        }
    }
}
