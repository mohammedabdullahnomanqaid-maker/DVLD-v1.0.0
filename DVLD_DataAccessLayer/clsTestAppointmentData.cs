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
   public static class clsTestAppointmentData
    {
        static public bool GetTestAppointmentByTestAppointmentID( int TestAppointmentID,ref int RetakeTestAppointmentID, ref int LocalDrivingLiceseApplicationID, ref DateTime AppointmentDate, ref decimal PaidFees,ref bool IsLocked)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select LocalDrivingLicenseApplicationID, RetakeTestApplicationID,AppointmentDate,PaidFees," +
                "IsLocked from TestAppointments where TestAppointmentID=@TestAppointmentID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;

                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    LocalDrivingLiceseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    if (reader["RetakeTestApplicationID"] == DBNull.Value)
                        RetakeTestAppointmentID = -1 ;
                    else
                    RetakeTestAppointmentID = (int)reader["RetakeTestApplicationID"];

                    IsLocked = (bool)reader["IsLocked"];
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
                    writer.WriteLine("\t\t\t Error in GetTestAppointmentByTestAppointmentID\n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        static public int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, decimal PaidFees
           ,int CreatedByUserID,bool IsLocked,int RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;


            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "INSERT INTO [dbo].[TestAppointments]([TestTypeID]," +
                "[LocalDrivingLicenseApplicationID],[AppointmentDate],[PaidFees],[CreatedByUserID]," +
                "[IsLocked],[RetakeTestApplicationID]) " +
                "VALUES " +
                "(@TestTypeID,@LocalDrivingLicenseApplicationID,@AppointmentDate,@PaidFees,@CreatedByUserID," +
                "@IsLocked,@RetakeTestApplicationID);"+
                " select scope_identity();";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("PaidFees", PaidFees);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("IsLocked", IsLocked);
            if (RetakeTestApplicationID!=-1)
                command.Parameters.AddWithValue("RetakeTestApplicationID", RetakeTestApplicationID);
            else
                command.Parameters.AddWithValue("RetakeTestApplicationID", DBNull.Value);


            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                // because of the  Resault = null we handle it
                if (Resault != null && int.TryParse(Resault.ToString(), out int _TestAppointmentID))
                {
                    TestAppointmentID = _TestAppointmentID;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in AddNewTestAppointment \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return TestAppointmentID;
        }

        static public bool UpdateTestAppointment(int TestAppointmentID, DateTime AppointmentDate)
        {
            int rowEffected = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Update TestAppointments set AppointmentDate=@AppointmentDate where TestAppointmentID=@TestAppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
           
            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in UpdateTestAppointment \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return (rowEffected > 0);
        }

        static public bool LockTestAppointment(int TestAppointmentID, bool IsLocked)
        {
            int rowEffected = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Update TestAppointments set IsLocked=@IsLocked where TestAppointmentID=@TestAppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in LockedTestAppointment \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return (rowEffected > 0);
        }

        static public int GetMaxTestTypeID(int LocalDrivingLicenseApplicationID)
        {
            int TestTypeID = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select max(TestTypeID) from Tests inner join TestAppointments " +
                "on Tests.TestAppointmentID=TestAppointments.TestAppointmentID " +
                "where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID and TestResult=1;";
                                              //TestResult=1 because maybe he has the max TestTypeID=1 and he faild 

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
        

            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                if(Resault!=null&&int.TryParse(Resault.ToString(),out int Number))
                {
                    TestTypeID = Number;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in GetMaxTestTypeID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return TestTypeID;
        }

        static public bool IsThereAnActiveAppointment(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select top 1 Found=1 from TestAppointments " +
                "where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID " +
                "and TestTypeID=@TestTypeID and IsLocked=0" +
                " order by TestAppointmentID desc;";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("TestTypeID", TestTypeID);

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
                    writer.WriteLine("\t\t\t\tError IsNotTestAppointmentLocked \n" + ex.Message);
                }
            }
            return IsFound;
        }

        static public bool IsNotTestAppointmentLockedByTestAppointmentID(int TestAppointmentID, int TestTypeID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select Found=1 from TestAppointments " +
                "where TestAppointmentID=@TestAppointmentID " +
                "and TestTypeID=@TestTypeID and IsLocked=0;";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("TestTypeID", TestTypeID);

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
                    writer.WriteLine("\t\t\t\tError IsNotTestAppointmentLockedByTestAppointmentID \n" + ex.Message);
                }
            }
            return IsFound;
        }
        static public DataTable GetTestAppointmentPerTest(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select TestAppointmentID,AppointmentDate,PaidFees,IsLocked from TestAppointments" +
                " where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID " +
                "and TestTypeID=@TestTypeID " +
                "order by TestAppointmentID desc;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("TestTypeID", TestTypeID);

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
                    writer.WriteLine("\t\t\t Error in GetTestAppointment \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public int GetTrailNumber(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            int TrailNumber = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select count(*) from TestAppointments" +
                " where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID and TestTypeID=@TestTypeID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                if (Resault != null && int.TryParse(Resault.ToString(), out int Number))
                {
                    TrailNumber = Number;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in GetTrailNumber \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return TrailNumber;
        }

    }
}
