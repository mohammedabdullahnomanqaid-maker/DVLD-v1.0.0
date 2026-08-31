using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DataAccessLayer
{
    static public class clsDetaindData
    {

        static public DataTable GetAllDetaind()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = " select *from DetainedLicenses_View order by ReleaseApplicationID ; ";
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
                    writer.WriteLine("\t\t\t Error in GetAllDetaind \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public int AddNewDetaindLicense(int LicenseID,int CreatedByUserID,DateTime DetainDate,bool IsReleased,decimal FineFees)
        {
            int DetainID = -1;


            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "INSERT INTO [dbo].[DetainedLicenses]([LicenseID],[DetainDate],[FineFees]," +
                "[CreatedByUserID],[IsReleased])" +
                " VALUES (@LicenseID,@DetainDate,@FineFees,@CreatedByUserID,@IsReleased )"+
                " select scope_identity();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("DetainDate", DetainDate);
            command.Parameters.AddWithValue("LicenseID", LicenseID);
            command.Parameters.AddWithValue("FineFees", FineFees);
            command.Parameters.AddWithValue("CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("IsReleased", IsReleased);

            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                // because of the  Resault = null we handle it
                if (Resault != null && int.TryParse(Resault.ToString(), out int _DetainID))
                {
                    DetainID = _DetainID;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in AddNewDetainLicense \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return DetainID;
        }
        
        static public bool IsLicenseDetaind(int LicenseID)
        {
           
                bool IsFound = false;
                SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
                string query = "select Found=1 from DetainedLicenses where LicenseID=@LicenseID and IsReleased=0;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("LicenseID", LicenseID);

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
                        writer.WriteLine("\t\t\t\tError isLicenseDetaind \n" + ex.Message);
                    }
                }
                return IsFound;
            
           
        }
        static public int GetDetainIDByLicenseID(int LicenseID)
        {
            int DetainID = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select DetainID from DetainedLicenses where LicenseID=@LicenseID and IsReleased=0;";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);


            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                if (Resault != null && int.TryParse(Resault.ToString(), out int Number))
                {
                    DetainID = Number;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in GetDetainIDByLicenseID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return DetainID;
        }
        static public bool GetDetainInfoByDetainID(int DetainID,ref int LicenseID,ref DateTime DetainDate,ref decimal FineFees,ref int CreatedByUserID,ref bool IsRelease,ref DateTime ReleaseDate,ref int ReleaseByUserID,ref int ReleaseApplicationID)
        {

                bool isFound = false;
                SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
                string query = "select * from DetainedLicenses where DetainID=@DetainID;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("DetainID", DetainID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        isFound = true;
                        LicenseID = (int)reader["LicenseID"];
                        DetainDate = (DateTime)reader["DetainDate"];
                        FineFees = (decimal)reader["FineFees"];
                        CreatedByUserID = (int)reader["CreatedByUserID"];

                        IsRelease =(bool)reader["IsReleased"];
                        ReleaseDate =( reader["ReleaseDate"]==DBNull.Value) ?DateTime.MaxValue:(DateTime)reader["ReleaseDate"];
                        ReleaseByUserID = reader["ReleasedByUserID"]==DBNull.Value? -1:(int)reader["ReleaseByUserID"];
                        ReleaseApplicationID = reader["ReleaseApplicationID"]==DBNull.Value? -1 : (int)reader["ReleaseApplicationID"];


                        

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
                        writer.WriteLine(" \t\t\t\tError in GetAllInfoByID \n" + ex.Message);
                    }
                }
                finally
                {
                    connection.Close();
                }
                return isFound;
            

        }
        static public bool ReleaseDetainLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int rowEffected = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "UPDATE [dbo].[DetainedLicenses] SET " +
                "[IsReleased] = @IsReleased," +
                "[ReleaseDate] = @ReleaseDate," +
                "[ReleasedByUserID] = @ReleasedByUserID," +
                "[ReleaseApplicationID] = @ReleaseApplicationID" +
                " WHERE DetainID=@DetainID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);
            command.Parameters.AddWithValue("@IsReleased", true);
            command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError ReleaseDetainLicense \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return (rowEffected > 0);
        }
       
    }
}
