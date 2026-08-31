using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccessLayer;
using BussinseLayer;

namespace BussinseLayer
{
   public class clsBPeople
    {
        public int PersonID { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }
        }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Address { set; get; }
        public string NationalNo { set; get; }
        public string ImagePath { set; get; }
        public int  Gendor { set; get; }
        public int  CountryID { set; get; }
        public DateTime DateOfBirth { set; get; }
        public clsCountry CountryInfo; 
        enum enMode { AddMode,UpdateMode};
        enMode Mode = enMode.AddMode;

        //done
      public clsBPeople()
        {
            this.PersonID = -1;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.Email = "";
            this.Phone = "";
            this.Address = "";
            this.Gendor = -1;
            this.CountryID = -1;
            this.DateOfBirth = DateTime.Now;
            Mode = enMode.AddMode;
        }

        //done
        public clsBPeople(int PersonID, string FirstName, string SecondName, string ThirdName, string LastName,
                          string Phone, string Email, string ImagePath,string NationalNo,string Address,int Gendor,
                          int CountryID,DateTime dateTime)
        {
            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.Phone = Phone;
            this.Email = Email;
            this.ImagePath = ImagePath;
            this.NationalNo = NationalNo;
            this.Address = Address;
            this.Gendor = Gendor;
            this.CountryID = CountryID;
            this.DateOfBirth = dateTime;
            this.CountryInfo = clsCountry.Find(CountryID);
            Mode = enMode.UpdateMode;
        }
        //done
        private bool _AddNewPerson()
        {
            this.PersonID= clsPeople.AddNewPerson(NationalNo, FirstName, SecondName,
                                         ThirdName, LastName, Email, Phone, Gendor,
                                         Address, DateOfBirth, ImagePath, CountryID);
            return (this.PersonID != -1);
        }
        //done
        private bool _UpdatePerson()
        {
            return clsPeople.UpdatePerson(PersonID,NationalNo, FirstName, SecondName,
                                         ThirdName, LastName, Email, Phone, Gendor,
                                         Address, DateOfBirth, ImagePath, CountryID);
        }
        //done
       static public bool DeletePerson(int PersonID)
        {
           return clsPeople.DeletePerson(PersonID);
        }
        //done
        static public DataTable GetAllInfoOfPeople()
        {
            return clsPeople.GetAllInfoOfPeople();
        }
        //done
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddMode:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.UpdateMode:
                    return _UpdatePerson();
            }
         
            return false ;
        }

        static public bool IsPersonExist(string NationalNo)
        {
            return clsPeople.IsPersonExist(NationalNo);
        }

       static public clsBPeople Find(int PersonID)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Email = "", Phone = "", 
                NationalNo = "", Address="",ImagePath="";
            DateTime dateOfBirth=DateTime.Now;
            int NationalCountryID = -1;
            short Gendor=-1;

            bool IsFound = clsPeople.GetPersonInfoByID(PersonID, ref FirstName, ref SecondName, ref ThirdName,
                ref LastName, ref NationalNo, ref dateOfBirth, ref Gendor, ref Address, ref Phone,
                ref Email, ref NationalCountryID, ref ImagePath);
            if (IsFound)
            {
                return new clsBPeople(PersonID, FirstName, SecondName, ThirdName, LastName, Phone, Email,
                    ImagePath, NationalNo, Address, Gendor, NationalCountryID, dateOfBirth);
            }
            else
            {
                return null;
            }

        }

        static public clsBPeople Find(string NationalNo)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Email = "", Phone = "",
             Address = "", ImagePath = "";
            DateTime dateOfBirth = DateTime.Now;
            int NationalCountryID = -1,PersonID=-1;
            short Gendor = -1;

            bool IsFound = clsPeople.GetPersonInfoByNationalNo(NationalNo, ref FirstName, ref SecondName, ref ThirdName,
                ref LastName, ref PersonID, ref dateOfBirth, ref Gendor, ref Address, ref Phone,
                ref Email, ref NationalCountryID, ref ImagePath);
            if (IsFound)
            {
                return new clsBPeople(PersonID, FirstName, SecondName, ThirdName, LastName, Phone, Email,
                    ImagePath, NationalNo, Address, Gendor, NationalCountryID, dateOfBirth);
            }
            else
            {
                return null;
            }
        }

        static public int GetPersonIDByLocalDrivingLicenseApplicationID(int LocalDrivingLiceseApplicationID)
        {
            return clsPeople.GetPersonIDByLocalDrivingLicenseApplicationID(LocalDrivingLiceseApplicationID);
        }

        static public int GetPersonIDByLicenseID(int LicenseID)
        {
            return clsPeople.GetPersonIDByLicenseID(LicenseID);
        }
    }
}
