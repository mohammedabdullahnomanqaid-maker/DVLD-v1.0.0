using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace DataAccessLayer
{
   static public class clsLocalDrivingLicenseApplicationData
    {
        static public DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select *from LocalDrivingLicenseApplications_View order by ApplicationDate desc;";
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
                    writer.WriteLine("\t\t\t Error in GetAllApplications \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public int AddNewLocalDrivingLicenseApplication(int ApplicationID,int LicneseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;


            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "INSERT INTO [dbo].[LocalDrivingLicenseApplications] ([ApplicationID],[LicenseClassID])" +
                " VALUES (@ApplicationID,@LicenseClassID)" +
                " select scope_identity();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("LicenseClassID", LicneseClassID);

            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                // because of the  Resault = null we handle it
                if (Resault != null && int.TryParse(Resault.ToString(), out int _ID))
                {
                    LocalDrivingLicenseApplicationID = _ID;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in AddNewLocalDrivingLicenseApplication \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return LocalDrivingLicenseApplicationID;
        }

        static public bool IsLocalDrivingLicenseApplicationExistByPersonIDAndLicenseClassID(int PersonID, int LicneseClassID)
            {
                bool IsFound = false;
                SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
                string query = "select Found=1 from LocalDrivingLicenseApplications inner join Applications" +
                " on LocalDrivingLicenseApplications.ApplicationID=Applications.ApplicationID" +
                " where Applications.ApplicantPersonID=@PersonID and " +
                "LocalDrivingLicenseApplications.LicenseClassID=@LicneseClassID; ";
              
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("PersonID", PersonID);
                command.Parameters.AddWithValue("LicneseClassID", LicneseClassID);

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
                        writer.WriteLine("\t\t\t\tError IsLocalDrivingLicenseApplicationExistByPersonIDAndLicenseClassID \n" + ex.Message);
                    }
                }
                return IsFound;
            }

        static public bool GetLocalDrivingLicenseApplicationByID(int LocalDrivingLicenseApplicationID, ref int ApplicationID,ref int LicenseClassID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select *from LocalDrivingLicenseApplications " +
                "where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID; ";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
           // MessageBox.Show(command.Parameters["LocalDrivingLicenseApplicationID"].Value.ToString());

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    ApplicationID = (int)reader["ApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];
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
                using(StreamWriter writer=new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in GetLocalDrivingLicenseApplicationByID\n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return IsFound;
        }

        static public bool UpdatLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            int rowEffected = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "UPDATE[dbo].[LocalDrivingLicenseApplications] SET" +
                "[ApplicationID] = @ApplicationID," +
                "[LicenseClassID] =@LicenseClassID " +
                "WHERE LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID ; ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
           ;

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in UpdatLocalDrivingLicenseApplication \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return (rowEffected > 0);
        }

        static public bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
         
                int rowEffected = -1;
                SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
                string query = "delete LocalDrivingLicenseApplications where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                try
                {
                    connection.Open();
                    rowEffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                    {
                        writer.WriteLine("\t\t\tError in DeleteLocalDrivingLicenseApplication \n" + ex.Message);
                    }
                }
                finally
                {
                    connection.Close();
                }
                return (rowEffected > 0);
            

        }
        static public bool GetLocalDrivingLicenseApplicationViewByID(int LocalDrivingLicenseApplicationID, ref string ClassName, ref int PassedTestCount,
            ref string Status)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select *from LocalDrivingLicenseApplications_View where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID; ";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    ClassName = (string)reader["ClassName"];
                    PassedTestCount = (int)reader["PassedTestCount"];
                    Status = (string)reader["Status"];

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
                    writer.WriteLine("\t\t\tError in GetLocalDrivingLicenseApplicationByID\n" + ex.Message);
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
