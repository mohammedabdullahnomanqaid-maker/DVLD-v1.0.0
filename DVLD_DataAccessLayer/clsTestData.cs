using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

namespace DataAccessLayer
{
   static public class clsTestData
    {
        static public bool UpdateTest(int TestID,int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int rowEffected = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "UPDATE [dbo].[Tests] SET " +
                "[TestAppointmentID] = @TestAppointmentID," +
                "[TestResult] = @TestResult," +
                "[Notes] = @Notes," +
                "[CreatedByUserID] = @CreatedByUserID " +
                "WHERE TestID=@TestID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("TestID", TestID);
            command.Parameters.AddWithValue("TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("TestResult", TestResult);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            if (Notes == null || Notes == string.Empty)
                command.Parameters.AddWithValue("Notes", DBNull.Value);
            else
                command.Parameters.AddWithValue("Notes", Notes);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in Update Test \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return (rowEffected > 0);
        }

        static public int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int TestID = -1;


            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "INSERT INTO [dbo].[Tests] ([TestAppointmentID],[TestResult],[Notes]," +
                "[CreatedByUserID]) VALUES (@TestAppointmentID,@TestResult,@Notes,@CreatedByUserID);" +
                "Update TestAppointments set IsLocked=1 where TestAppointmentID=@TestAppointmentID;" +
                " select scope_identity();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("TestResult", TestResult);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            if(Notes==null||Notes==string.Empty)
            command.Parameters.AddWithValue("Notes", DBNull.Value);
            else
            command.Parameters.AddWithValue("Notes", Notes);

            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                // because of the  Resault = null we handle it
                if (Resault != null && int.TryParse(Resault.ToString(), out int _TestID))
                {
                    TestID = _TestID;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in AddNewTest \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return TestID;
        }

        static public int GetTestIDByAppointmentID(int TestAppointmentID)
        {
            int TestID = -1;


            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select TestID from Tests where TestAppointmentID=@TestAppointmentID;";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("TestAppointmentID", TestAppointmentID);


            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                // because of the  Resault = null we handle it
                if (Resault != null && int.TryParse(Resault.ToString(), out int _TestID))
                {
                    TestID = _TestID;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in GetTestIDByAppointmentID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return TestID;
        }
        static public bool GetTestInfoByID(int TestID, ref int TestAppointmentID, ref string Notes, ref int CreatedByUser, ref bool TestResault)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select * from Tests where TestID=@TestID;";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("TestID", TestID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    CreatedByUser = (int)reader["CreatedByUserID"];
                    TestResault = Convert.ToInt16(reader["TestResult"])==1 ? true : false;
                    Notes = reader["Notes"]==DBNull.Value ? "": (string)reader["Notes"];

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
                    writer.WriteLine("\t\t\tError in GetTestInfoByID\n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return IsFound;
        }

        static public bool IsPassTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select top 1 Found=1 from Tests inner join TestAppointments " +
                "on Tests.TestAppointmentID=TestAppointments.TestAppointmentID " +
                "where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID and TestTypeID=@TestTypeID and TestResult=1 " +
                "order by TestAppointments.TestAppointmentID desc;";


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
                    writer.WriteLine("\t\t\t\tError IsFaild \n" + ex.Message);
                }
            }
            return IsFound;
        }
        static public bool DoesAttendBefore(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool IsFound = false;
            //it does not neccessery that pass or faild the neccessry thing that attend the test before.
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select top 1 Found=1 from Tests inner join TestAppointments " +
                "on Tests.TestAppointmentID=TestAppointments.TestAppointmentID " +
                "where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID and TestTypeID=@TestTypeID " +
                "order by TestAppointments.TestAppointmentID desc;";


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
                    writer.WriteLine("\t\t\t\tError DoesAttendBefore \n" + ex.Message);
                }
            }
            return IsFound;
        }

        static public byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte TestPassedCount = 0;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select TestCount=count(tests.TestID) from Tests inner join TestAppointments" +
                " on Tests.TestAppointmentID=TestAppointments.TestAppointmentID" +
                " where TestAppointments.LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID and Tests.TestResult=1;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                if(Resault!=null&&int.TryParse(Resault.ToString(),out int Number))
                {
                    TestPassedCount =Convert.ToByte(Number);
                }
            }
            catch(Exception ex)
            {
                using(StreamWriter writer=new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("Error in GetTestPassedCount \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return TestPassedCount;
        }
    }
}
