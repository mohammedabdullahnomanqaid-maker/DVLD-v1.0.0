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
   static public class clsApplicationsData
    {
     
        static public int AddNewApplication(int ApplicantPersonID, int ApplicationTypeID,int CreatedByUserID, DateTime ApplicationDate, DateTime LastStatusDate,int ApplicationStatus,decimal PaidFees)
        {
            int ApplicationID = -1;


            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "INSERT INTO [dbo].[Applications] ([ApplicantPersonID],[ApplicationDate]," +
             "[ApplicationTypeID],[ApplicationStatus],[LastStatusDate],[PaidFees],[CreatedByUserID])" +
             " VALUES(" +
             " @ApplicantPersonID," +
             " @ApplicationDate," +
             " @ApplicationTypeID," +
             " @ApplicationStatus," +
             " @LastStatusDate," +
             " @PaidFees," +
             " @CreatedByUserID" +
             ")" +
             "select scope_identity();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("PaidFees", PaidFees);
            command.Parameters.AddWithValue("ApplicationStatus", ApplicationStatus);

            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
            // because of the  Resault = null we handle it
                if(Resault != null && int.TryParse(Resault.ToString(),out int _ApplicationID))
                {
                    ApplicationID = _ApplicationID;
                }
            }
            catch(Exception ex)
            {
                using(StreamWriter writer=new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in AddNewApplication \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }

            return ApplicationID;
        }

        static public bool GetApplicationByID(int ApplicationID,ref int ApplicantPersonID,ref int ApplicationTypeID,
            ref int CreatedByUserID, ref DateTime ApplicationDate, ref DateTime LastStatusDate,
            ref int ApplicationStatus, ref decimal PaidFees)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select *from Applications where ApplicationID=@ApplicationID; ";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    LastStatusDate = (DateTime)reader["LastStatusDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    ApplicantPersonID = (int)reader["ApplicantPersonID"];
                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    ApplicationStatus =Convert.ToInt16(reader["ApplicationStatus"]);

                }
                else
                {
                    IsFound = false;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in GetApplicationByID\n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return IsFound;
        }

        static public bool UpdatLocalApplication(int ApplicationID,int ApplicantPersonID,int ApplicationTypeID,
             int CreatedByUserID, DateTime ApplicationDate, DateTime LastStatusDate,
             int ApplicationStatus, decimal PaidFees)
        {
            int rowEffected = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "UPDATE [dbo].[Applications] SET " +
                "[ApplicantPersonID] =@ApplicantPersonID," +
                "[ApplicationDate] = @ApplicationDate," +
                "[ApplicationTypeID] =@ApplicationTypeID," +
                "[ApplicationStatus] = @ApplicationStatus," +
                "[LastStatusDate] = @LastStatusDate," +
                "[PaidFees] = @PaidFees," +
                "[CreatedByUserID] =@CreatedByUserID" +
                " WHERE ApplicationID=@ApplicationID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in UpdatLocalApplication \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return (rowEffected > 0);
        }

        static public bool DeleteApplication(int ApplicationID)
        {
            int rowEffected = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "delete Applications where ApplicationID=@ApplicationID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in DeleteApplication \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return (rowEffected > 0);
        }

        static public bool IsClassLicenseComplete(int ApplicantPersonID,int LicenseClassID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select Found=1 from LocalDrivingLicenseApplications inner join Applications" +
                " on LocalDrivingLicenseApplications.ApplicationID=Applications.ApplicationID " +
                "where ApplicantPersonID=@ApplicantPersonID and ApplicationStatus=3 and LicenseClassID=@LicenseClassID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);

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
                    writer.WriteLine("\t\t\t\tError IsClassLicenseComplete \n" + ex.Message);
                }
            }
            return IsFound;
        }

        static public bool UpdateApplicationStatus(int ApplicationID,short ApplicationStatus)
        {
            int rowEffected = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Update Applications set ApplicationStatus=@ApplicationStatus " +
                "where ApplicationID=@ApplicationID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
           

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in Update Applications \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return (rowEffected > 0);
        }


        static public int GetActiveApplicationIDForLicenseClass(int ApplicantPersonID, int ApplicationTypeID, int LicenseClassID)
        {
            int ApplicationID = -1;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select ActiveApplicationID=Applications.ApplicationID from Applications" +
                " inner join LocalDrivingLicenseApplications on LocalDrivingLicenseApplications.ApplicationID=" +
                "Applications.ApplicationID where Applications.ApplicantPersonID=@ApplicantPersonID and ApplicationTypeID=@ApplicationTypeID " +
                "and LocalDrivingLicenseApplications.LicenseClassID=@LicenseClassID and ApplicationStatus=1; ";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);


            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                // because of the  Resault = null we handle it
                if (Resault != null && int.TryParse(Resault.ToString(), out int _ApplicationID))
                {
                    ApplicationID = _ApplicationID;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in GetActiveApplicationIDForLicenseClass \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return ApplicationID;
        }


    }
}
