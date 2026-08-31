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
   static public class clsDriversData
    {

        static public DataTable GetAllDrivers()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select Drivers.DriverID,Drivers.PersonID,People.NationalNo,People.FirstName+' '+" +
                " People.SecondName+' '+ISNULL(People.ThirdName,'')+' '+People.LastName as FullName, " +
                "Drivers.CreatedDate,(select count(*) from Licenses where Licenses.DriverID=Drivers.DriverID) " +
                " as ActiveLicense from Drivers inner join People on Drivers.PersonID=People.PersonID;";
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
                    writer.WriteLine("\t\t\t Error in GetAllDrivers \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public int GetDriverIDByApplicationID(int ApplicationID)
        {
            int LicenseID = -1;


            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select InternationalLicenses.DriverID from InternationalLicenses inner join Applications on InternationalLicenses.ApplicationID=Applications.ApplicationID where InternationalLicenses.ApplicationID=@ApplicationID;";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("ApplicationID", ApplicationID);


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
                    writer.WriteLine("\t\t\t\tError in GetDriverIDByApplicationID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return LicenseID;
        }

        public static bool GetDriverInfoByDriverID(int DriverID, ref int PersonID, ref int CreatedUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select *from Drivers where DriverID=@DriverID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("DriverID", DriverID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    PersonID = (int)reader["PersonID"];
                    CreatedUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];
                   

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
                    writer.WriteLine(" \t\t\t\tError in GetDriverInfoByDriverID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool GetDriverInfoByPersonID(int PersonID, ref int DriverID, ref int CreatedUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select *from Drivers where PersonID=@PersonID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    DriverID = (int)reader["DriverID"];
                    CreatedUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];


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
                    writer.WriteLine(" \t\t\t\tError in GetDriverInfoByPersonID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        static public int AddNewDriver(int PersonID, int CreatedByUserID)
        {
            int DriverID = -1;


            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "INSERT INTO [dbo].[Drivers]([PersonID],[CreatedByUserID],[CreatedDate])VALUES" +
                " (@PersonID,@CreatedByUserID,@CreatedDate); " +
                " select scope_identity();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("PersonID", PersonID);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("CreatedDate", DateTime.Now);

            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                // because of the  Resault = null we handle it
                if (Resault != null && int.TryParse(Resault.ToString(), out int _DriverID))
                {
                    DriverID = _DriverID;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in AddNewDriver \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return DriverID;
        }

        static public int IsPersonDriver(int PersonID)
        {
            int DriverID = -1;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select DriverID from Drivers where PersonID=@PersonID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("PersonID", PersonID);

            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                if(Resault!=null&&int.TryParse(Resault.ToString(),out int Number))
                DriverID = Number;
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in IsPersonDriver \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return DriverID;
        }
       
    }
}
