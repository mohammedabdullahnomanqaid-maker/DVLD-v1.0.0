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
   static public class clsTestTypesData
    {

        static public DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select *from TestTypes;";
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
                using (StreamWriter writer=new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in Get All Test Types \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        static public bool GetTestTypeByTestTypeID(int TestTypeID,ref string TestTypeTitle,ref string TestTypeDescription,ref decimal TestTypeFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select *from TestTypes where TestTypeID =@TestTypeID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("TestTypeID", TestTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;

                    TestTypeTitle = (string)reader["TestTypeTitle"];
                    TestTypeDescription = (string)reader["TestTypeDescription"];
                    TestTypeFees = (decimal)reader["TestTypeFees"];
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
                    writer.WriteLine("\t\t\t Error in GetTestTypeByTestTypeID\n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool UpdateTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, decimal TestTypeFees)
        {
            int rowEffected = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Update TestTypes set " +
                "TestTypeTitle=@TestTypeTitle ," +
                "TestTypeDescription=@TestTypeDescription ," +
                "TestTypeFees=@TestTypeFees " +
                "where TestTypeID=@TestTypeID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("TestTypeFees", TestTypeFees);
            command.Parameters.AddWithValue("TestTypeTitle", TestTypeTitle);
            command.Parameters.AddWithValue("TestTypeDescription", TestTypeDescription);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\tError in Update Test Types \n" + ex.Message);
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
