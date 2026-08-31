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
   static public class clsApllicationTypesData
    {

        static public DataTable GetApplicationTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select *from ApplicationTypes;";
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
            catch(Exception ex)
            {
                using(StreamWriter writer=new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t Error in Gat All Application Types \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public decimal GetApplicationTypeFees(int ApplicationTypeID)
        {
            decimal ApplicationFees = 0;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select ApplicationFees from ApplicationTypes where ApplicationTypeID=@ApplicationTypeID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("ApplicationTypeID", ApplicationTypeID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ApplicationFees = (decimal)reader["ApplicationFees"];
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t Error in GetApplicationTypeFees \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return ApplicationFees;
        }

        static public bool GetApplicationTypeByApplicationID(int ApplicationTypeID, ref string ApplicationTypeTitle,ref decimal ApplicationTypeFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select *from ApplicationTypes where ApplicationTypeID=@ApplicationTypeID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("ApplicationTypeID", ApplicationTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    
                    ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                    ApplicationTypeFees = (decimal)reader["ApplicationFees"];
                }
                else
                {
                    isFound = false;
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                isFound = false;
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t Error in GetApplicationTypeByApplicationID\n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool UpdateApplicationTypeTiteAndFees(int ApplicationTypeID,string ApplicationTypeTitle,decimal ApplicationTypeFees)
        {
            int rowEffected = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Update ApplicationTypes set " +
                "ApplicationTypeTitle=@ApplicationTypeTitle," +
                "ApplicationFees=@ApplicationFees " +
                "where ApplicationTypeID=@ApplicationTypeID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("ApplicationFees", ApplicationTypeFees);
            command.Parameters.AddWithValue("ApplicationTypeTitle", ApplicationTypeTitle);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                using(StreamWriter writer=new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\tError in Update Application Types \n" + ex.Message);
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
