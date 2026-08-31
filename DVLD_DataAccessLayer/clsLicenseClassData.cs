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
  public static class clsLicenseClassData
    {
        static public DataTable GetAllLicenseClass()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select LicenseClassID,ClassName,ClassFees,MinimumAllowedAge from LicenseClasses;";
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
                    writer.WriteLine("\t\t\t Error in GetAllLicenseClass \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public bool GetLicenseInfoByID(int LicenseClassID,ref string ClassName,ref string ClassDescription,ref short MinimumAllowedAge,ref short DefaultValidityLength,ref decimal ClassFees)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select * from LicenseClasses where LicenseClassID=@LicenseClassID;";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    ClassName = (string)reader["ClassName"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = Convert.ToInt16(reader["MinimumAllowedAge"]);
                    DefaultValidityLength = Convert.ToInt16(reader["DefaultValidityLength"]);
                    ClassFees = (decimal)reader["ClassFees"];
                
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


    }
}
