using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace DataAccessLayer
{
   public static class clsUserData
    {
        //done
        static public DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select UserID,Users.PersonID ,FirstName+' '+SecondName+' '+ISNULL(ThirdName,'')+" +
                "' '+LastName as FullName,UserName,Password,IsActive from Users inner join People " +
                "on Users.PersonID=People.PersonID;";
            SqlCommand command = new SqlCommand(query,connection);

            try
            {
                connection.Open();
                SqlDataReader reader =command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
            }
            catch(Exception ex)
            {
                using(StreamWriter writer =new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t Error in GetAllUsers \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }
        //done
        static public int AddNewUser(int PersonID,string UserName,string Password,bool IsActive)
        {
            int UserID = -1;


            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Insert into Users (PersonID,UserName,Password,IsActive)" +
                " values(@PersonID," +
                "@UserName," +
                "@Password," +
                "@IsActive )" +
                " select scope_identity();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("PersonID", PersonID);
            command.Parameters.AddWithValue("UserName", UserName);
            command.Parameters.AddWithValue("Password", Password);
            command.Parameters.AddWithValue("IsActive", IsActive);

            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
            // because of the  Resault = null we handle it
                if(Resault != null && int.TryParse(Resault.ToString(),out int _UserID))
                {
                    UserID = _UserID;
                }
            }
            catch(Exception ex)
            {
                using(StreamWriter writer=new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in AddNewUser \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return UserID;
        }
        //done
        static public bool IsUserExistByPersonID(int PersonID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select Found=1 From Users where PersonID=@PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                IsFound = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError is Person exist \n" + ex.Message);
                }
            }
            return IsFound;
        }
        //done
        static public bool IsUserNameExistByUsername(string UserName)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select Found=1 from Users where UserName=@UserName;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("UserName", UserName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                IsFound = reader.HasRows;
            }
            catch(Exception ex)
            {
                using (StreamWriter writer =new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in IsUserNameExist \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return IsFound;
        }
        //done
        public static bool GetUserInfoByPersonID(int PersonID,ref int UserID, ref string UserName, ref string Password,
                       ref bool IsActive)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select *from Users where PersonID=@PersonID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    UserID = (int)reader["UserID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
                        
                   
                }
                else
                {
                    isFound = false;
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                isFound = false;
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine(" \t\t\t\tError in GetUserInfoByPersonID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
        //done
        public static bool GetUserInfoByUserID(int UserID, ref int PersonID, ref string UserName, ref string Password,
                       ref bool IsActive)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select *from Users where UserID=@UserID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    PersonID = (int)reader["PersonID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];


                }
                else
                {
                    isFound = false;
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                isFound = false;
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine(" \t\t\t\tError in GetUserInfoByUserID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
        //done
        static public bool UpdateUser(int UserID,int PersonID, string UserName, string Password, bool IsActive)
        {
            int rowEffected = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Update [dbo].[Users] set [PersonID]=@PersonID,[UserName]=@UserName," +
                "[Password]=@Password,[IsActive]=@IsActive " +
                "where UserID=@UserID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in Update Users \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return (rowEffected > 0);
        }
        //done
        static public bool DeleteUser(int UserID)
        {
            int rowEffected = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "delete [dbo].[Users] where UserID=@UserID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in Delete User \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return (rowEffected > 0);
        }
        //done
        static public bool GetUserInfoByUsernameAndPassword(string Username,string Password,ref int UserID,ref int PersonID,ref bool IsActive)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select *from Users where Username=@Username and Password=@Password;";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("Username", Username);
            command.Parameters.AddWithValue("Password", Password);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
               
                if (reader.HasRows)
                {
                    if(reader.Read())
                    {
                        UserID = (int)reader["UserID"];
                        PersonID = (int)reader["PersonID"];
                        IsActive = (bool)reader["IsActive"];
                        IsFound = true;
                    }
               
                }
                else
                {
                    IsFound = false;
                }
                reader.Close();

            }
            catch(Exception ex)
            {
                IsFound = false;
                using(StreamWriter writer=new StreamWriter(clsConnection.ConnectionString))
                {
                    writer.WriteLine("\t\t\tError in GetUserInfoByUsernameAndPassword\n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return IsFound;

        }
        //still...
        static public bool UpdatePassword(int UserID,string Password)
        {
            short roweffected = -1;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Update Users set Password=@Password where UserID=@UserID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("UserID", UserID);
            command.Parameters.AddWithValue("Password", Password);

            try
            {
                connection.Open();
                roweffected =(short) command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                using(StreamWriter writer=new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\t Error in Update Password \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }

            return (roweffected > 0);

        }

    }
}
