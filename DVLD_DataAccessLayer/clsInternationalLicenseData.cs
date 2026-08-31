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
   static public class clsInternationalLicenseData
    {
        static public DataTable GetAllInternationalLicense()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select *from InternationalLicenses;";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t Error in GetAllInternationalLicense \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public DataTable GetAllInternationalLicenseByDriverID(int DriverID)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select InternationalLicenseID,ApplicationID,IssuedUsingLocalLicenseID,IssueDate,ExpirationDate,IsActive from InternationalLicenses where DriverID=@DriverID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("DriverID", DriverID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t Error in GetAllInternationalLicenseByDriverID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public int AddNewInterNationalLicense(int ApplicationID, int DriverID, int CreatedByUserID,
                 int IssuedUsingLocalLicenseID,DateTime IssueDate, DateTime ExpirationDate,bool IsActive)
        {
            int InterNationalLicenseID = -1;


            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Update InternationalLicenses set IsActive=0 where DriverID=@DriverID ;" +
                " INSERT INTO [dbo].[InternationalLicenses]([ApplicationID],[DriverID]," +
                "[IssuedUsingLocalLicenseID],[IssueDate],[ExpirationDate],[IsActive],[CreatedByUserID])" +
                "VALUES(@ApplicationID,@DriverID,@IssuedUsingLocalLicenseID,@IssueDate,@ExpirationDate," +
                "@IsActive,@CreatedByUserID) " +
                " select scope_identity();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("DriverID", DriverID);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("IssueDate", IssueDate);
            command.Parameters.AddWithValue("ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("IsActive", IsActive);

            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                // because of the  Resault = null we handle it
                if (Resault != null && int.TryParse(Resault.ToString(), out int _InterNationalLicenseID))
                {
                    InterNationalLicenseID = _InterNationalLicenseID;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in AddNewInterNationalLicense \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return InterNationalLicenseID;
        }

        public static bool GetInternationalLicenseInfoByInternationalID(int InternationalLicenseID,ref int ApplicationID,ref int DriverID,ref int CreatedByUserID,
                ref int IssuedUsingLocalLicenseID,ref DateTime IssueDate,ref DateTime ExpirationDate,ref bool IsActive)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select *from InternationalLicenses where InternationalLicenseID=@InternationalLicenseID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("InternationalLicenseID", InternationalLicenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    DriverID = (int)reader["DriverID"];
                    ApplicationID = (int)reader["ApplicationID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    if (Convert.ToInt32(reader["IsActive"]) == 1)
                        IsActive = true;
                    else
                        IsActive = false;


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
                    writer.WriteLine(" \t\t\t\tError in GetInternationalLicenseInfoByInternationalID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
        //still...
        static public bool GetInternationalAndPersonInfo(int InternationalLicenseID, ref int ApplicationID, ref int DriverID, ref int CreatedByUserID,
             ref int IssuedUsingLocalLicenseID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive)
        {

                bool isFound = false;
                SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
                string query = "select People.FirstName+' '+People.SecondName+' '+isnull(People.ThirdName,'')" +
                "+' '+People.LastName as FullName,People.ImagePath, People.NationalNo,People.Gendor," +
                "People.DateOfBirth,Drivers.DriverID,InternationalLicenses.InternationalLicenseID," +
                "InternationalLicenses.ApplicationID,InternationalLicenses.ExpirationDate," +
                "InternationalLicenses.IsActive,InternationalLicenses.IssueDate," +
                "InternationalLicenses.IssuedUsingLocalLicenseID from InternationalLicenses " +
                "inner join Drivers on InternationalLicenses.DriverID=Drivers.DriverID " +
                "inner join People On People.PersonID=Drivers.PersonID " +
                "where InternationalLicenses.InternationalLicenseID=@InternationalLicenseID;";


                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("InternationalLicenseID", InternationalLicenseID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        isFound = true;
                        DriverID = (int)reader["DriverID"];
                        ApplicationID = (int)reader["ApplicationID"];
                        CreatedByUserID = (int)reader["CreatedByUserID"];
                        IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                        IssueDate = (DateTime)reader["IssueDate"];
                        ExpirationDate = (DateTime)reader["ExpirationDate"];
                        if (Convert.ToInt32(reader["IsActive"]) == 1)
                            IsActive = true;
                        else
                            IsActive = false;


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
                        writer.WriteLine(" \t\t\t\tError in GetInternationalLicenseInfoByInternationalID \n" + ex.Message);
                    }
                }
                finally
                {
                    connection.Close();
                }
                return isFound;
            

        }


        static public int GetInternationalLicenseIDIfExist(int LicenseID)
        {
           int InternationalLicenseID = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select InternationalLicenseID from InternationalLicenses " +
                "where InternationalLicenses.IssuedUsingLocalLicenseID=@IssuedUsingLocalLicenseID " +
                "and IsActive=1 and GetDate() between IssueDate and ExpirationDate; ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("IssuedUsingLocalLicenseID", LicenseID);

            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();

             if(Resault!=null&&int.TryParse(Resault.ToString(),out int ID))
                {
                    InternationalLicenseID = ID;
                }
            }
            catch (Exception ex)
            {
             
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError DoesHaveInternationalLicense \n" + ex.Message);
                }
            }
            return InternationalLicenseID;
        }

    }
}
