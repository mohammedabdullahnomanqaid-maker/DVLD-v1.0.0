using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccessLayer;

namespace BussinseLayer
{
    public class clsCountry
    {
        public int countryID { set; get; }
        public string CountryName { set; get; }
        //enum enMode { AddMode,UpdateMode };
        //enMode _Mode = enMode.UpdateMode;

        clsCountry(int CountryID,string CountryName)
        {
            this.countryID = countryID;
            this.CountryName = CountryName;
            //_Mode = enMode.UpdateMode;
        }
       static public DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }

        static public clsCountry Find(int CountryID)
        {
            string CountryName = "";
            if (clsCountryData.GetCountryInfoByCountryID(CountryID,ref CountryName))
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }
        
    }
}
