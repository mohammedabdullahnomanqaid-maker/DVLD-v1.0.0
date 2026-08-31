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
   public static class clsLicenseData
    {
        public static bool GetLicenseInfoByLicenseID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int CreatedByUserID,
                      ref int LicenseClassID,ref short IssueReason,ref string Notes,ref DateTime IssueDate,ref DateTime ExpirationDate,ref decimal PaidFees,ref bool IsActive)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select *from Licenses where LicenseID=@LicenseID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    ApplicationID = (int)reader["ApplicationID"];
                    DriverID = (int)reader["DriverID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    LicenseClassID = (int)reader["LicenseClass"];
                    IssueReason =Convert.ToInt16(reader["IssueReason"]);//not (short) because the IssueReason type is tinyint and return object and when you use (short) he see object and can not cast it to short but convert.toint16() can cast it to short.
                    if (reader["Notes"] == DBNull.Value)
                        Notes = string.Empty;
                    else
                        Notes = (string)reader["Notes"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    PaidFees = (decimal)reader["PaidFees"];
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
                    writer.WriteLine(" \t\t\t\tError in GetLicenseInfoByLicenseID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool GetLicenseInfoByDriverID(int DriverID, ref int ApplicationID, ref int LicenseID, ref int CreatedByUserID,
              ref int LicenseClassID, ref short IssueReason, ref string Notes, ref DateTime IssueDate, ref DateTime ExpirationDate, ref decimal PaidFees, ref bool IsActive)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select *from Licenses where DriverID=@DriverID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("DriverID", DriverID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    ApplicationID = (int)reader["ApplicationID"];
                    LicenseID = (int)reader["LicenseID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    LicenseClassID = (int)reader["LicenseClass"];
                    IssueReason = Convert.ToInt16(reader["IssueReason"]);//not (short) because the IssueReason type is tinyint and return object and when you use (short) he see object and can not cast it to short but convert.toint16() can cast it to short.
                    if (reader["Notes"] == DBNull.Value)
                        Notes = string.Empty;
                    else
                        Notes = (string)reader["Notes"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    PaidFees = (decimal)reader["PaidFees"];
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
                    writer.WriteLine(" \t\t\t\tError in GetLicenseInfoByDriverID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static DataTable GetLicenseInfoOfHistoryByDriverID(int DriverID)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select LicenseID,ApplicationID," +
                "(select ClassName from LicenseClasses where LicenseClassID=Licenses.LicenseClass )" +
                " as ClassName,IssueDate,ExpirationDate,IsActive from Licenses where DriverID=@DriverID;";
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
               
                reader.Close();

            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine(" \t\t\t\tError in GetLicenseInfoOfHistoryByDriverID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public int AddNewLicense( int ApplicationID, int DriverID, int CreatedByUserID,
                       int LicenseClassID, short IssueReason, string Notes, DateTime IssueDate, DateTime ExpirationDate, decimal PaidFees, bool IsActive)
        {
            int LicenseID = -1;


            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "INSERT INTO [dbo].[Licenses]([ApplicationID],[DriverID],[LicenseClass]," +
                "[IssueDate],[ExpirationDate],[Notes],[PaidFees],[IsActive],[IssueReason],[CreatedByUserID])" +
                "VALUES (@ApplicationID,@DriverID,@LicenseClass,@IssueDate,@ExpirationDate,@Notes,@PaidFees," +
                "@IsActive,@IssueReason,@CreatedByUserID); " +
                " select scope_identity();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("DriverID", DriverID);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("LicenseClass", LicenseClassID);
            command.Parameters.AddWithValue("IssueReason", IssueReason);
            command.Parameters.AddWithValue("Notes", Notes);
            command.Parameters.AddWithValue("IssueDate", IssueDate);
            command.Parameters.AddWithValue("ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("PaidFees", PaidFees);
            command.Parameters.AddWithValue("IsActive", IsActive);

            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                // because of the  Resault = null we handle it
                if (Resault != null && int.TryParse(Resault.ToString(), out int _licenseID))
                {
                    LicenseID = _licenseID;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in AddNewLicense \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return LicenseID;
        }

        static public int GetActiveLicenseIDByPersonID(int PersonID,int ClassLicense)
        {
            int LicenseID = -1;

            //My Note : this query check if Person is driver - because if Person is not driver ,it will not appear - and has the same license class and already active prevent him.

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select Licenses.LicenseID from Licenses inner join Drivers " +
                "on Drivers.DriverID=Licenses.DriverID where Drivers.PersonID=@PersonID" +
                " and Licenses.LicenseClass=@LicenseClass and Licenses.IsActive=1; ";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("PersonID", PersonID);
            command.Parameters.AddWithValue("LicenseClass", ClassLicense);


            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                // because of the  Resault = null we handle it
                if (Resault != null && int.TryParse(Resault.ToString(), out int _licenseID))
                {
                    LicenseID = _licenseID;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in GetLicenseID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return LicenseID;
        }

        static public bool MakeLicenseInActive(int LicenseID)
        {
            int rowEffected = -1;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "update licenses set IsActive=0 where LicenseID=@LicenseID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                using (StreamWriter writer=new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("error in Make license in active "+ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }

            return (rowEffected != -1);
        }

        static public bool IsActiveLicense(int LicenseID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select Found=1 from Licenses where LicenseID=@LicenseID and IsActive=1;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("LicenseID", LicenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                IsFound = reader.HasRows;
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in IsActiveLicense \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return IsFound;
        }

      
    }
}
