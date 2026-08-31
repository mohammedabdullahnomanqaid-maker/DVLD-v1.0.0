using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
   static public class clsCountryData
    {
        static public DataTable GetAllCountries()
        {
            DataTable dataTable = new DataTable();
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select *from Countries;";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dataTable.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in Countries \n " + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return dataTable;
        }

        static public bool GetCountryInfoByCountryID(int CountryID,ref string CountryName)
        {
            bool isFound = false ;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select *from Countries where CountryID=@CountryID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("CountryID", CountryID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    CountryName = (string)reader["CountryName"];

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
                using(StreamWriter writer=new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError in GetCountryInfoByCountryID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
    }


}
