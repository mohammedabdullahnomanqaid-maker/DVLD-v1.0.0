using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccessLayer;

namespace BussinseLayer
{
   public class clsUsers
    {

        public int UserID { set; get; }
        public int PersonID { set; get; }
        public string UserName { set; get; }
        public clsBPeople PersonInfo;
        public string Password { set; get; }
        public bool IsActive { set; get; }

       public enum enMode { AddMode,UpdateMode};
       public enMode _Mode = enMode.AddMode;
        //done
       public clsUsers()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.PersonInfo = null;
            this.Password = "";
            this.IsActive =false;
            _Mode = enMode.AddMode;
        }
        //done
       public clsUsers(int UserID,int PersonID,string UserName,string Password,bool IsActive)
        {
            this.UserID = UserID;
            this.UserName = UserName;
            this.PersonID = PersonID;
            this.Password = Password;
            this.IsActive = IsActive;
            this.PersonInfo = clsBPeople.Find(PersonID);
            _Mode = enMode.UpdateMode;
        }
        //done
        static public DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }
        //done
        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(PersonID, UserName, Password, IsActive);
           
            return (this.UserID != -1);
        }
        //done
        private bool _UpdateUser()
        {
         return clsUserData.UpdateUser(this.UserID, this.PersonID, this.UserName, this.Password, this.IsActive);
        }
        //done
       public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddMode:
                    if (_AddNewUser())
                    {
                        _Mode = enMode.UpdateMode;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.UpdateMode:
                    return _UpdateUser();
            }
            return false;
        }
        //done
        public static bool IsUserExistByPersonID(int PerosnID)
        {
            return clsUserData.IsUserExistByPersonID(PerosnID);
        }
        //done
        public static bool IsUserNameExistByUsername(string UserName)
        {
            return clsUserData.IsUserNameExistByUsername(UserName);
        }
        //done
        public static clsUsers FindUserByPersonID(int PersonID)
        {
            int UserID = -1;
            string UserName = string.Empty, Password = string.Empty;
            bool IsActive =false;

            bool IsFound = clsUserData.GetUserInfoByPersonID(PersonID, ref UserID,
                ref UserName, ref Password, ref IsActive);

            if (IsFound)
                return new clsUsers(UserID,PersonID, UserName, Password, IsActive);
            else
                return null;


        }
        //done
        public static clsUsers FindUserByUserID(int UserID)
        {
            int PersonID = -1;
            string UserName = string.Empty, Password = string.Empty;
            bool IsActive = false;

            bool IsFound = clsUserData.GetUserInfoByUserID(UserID, ref PersonID,
                ref UserName, ref Password, ref IsActive);

            if (IsFound)
                return new clsUsers(UserID, PersonID, UserName, Password, IsActive);
            else
                return null;

        }
        //done
        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }
        //done
        static public clsUsers FindUserInfoByUsernameAndPassword(string Username,string Password)
        {
            int UserID = -1, PersonID = -1;
            bool IsActive=false;

            bool IsFound = clsUserData.GetUserInfoByUsernameAndPassword(Username, Password, ref UserID, ref PersonID, ref IsActive);

            if (IsFound)
                return new clsUsers(UserID, PersonID, Username, Password, IsActive);
            else
                return null;
        }
        //still...
        public static bool UpdatePassword(int UserID,string Password)
        {
            return clsUserData.UpdatePassword(UserID, Password);
        }

    }
}
