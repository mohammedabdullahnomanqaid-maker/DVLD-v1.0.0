
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
    static public class clsPeople
    {
        //done
        static public DataTable GetAllInfoOfPeople()
        {
            DataTable dataTable = new DataTable();
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select *from myFullPeopleTableScreen;";
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
                    writer.WriteLine("\t\t\tError in clsPeople \n " + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return dataTable;
        }


        //done
        static public int AddNewPerson(string NationalNo, string FirstName, string SecondName,
                                        string ThirdName, string LastName, string Email, string Phone, int Gendor,
                                        string Address, DateTime dateTime, string ImagePath, int CountryID)
        {
            int PersonID = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "INSERT INTO [dbo].[People] ([NationalNo],[FirstName],[SecondName],[ThirdName]," +
                          "[LastName],[DateOfBirth],[Gendor],[Address],[Phone],[Email],[NationalityCountryID]," +
                           "[ImagePath])VALUES(@NationalNo,@FirstName,@SecondName,@ThirdName,@LastName," +
                           "@DateOfBirth,@Gendor,@Address,@Phone,@Email,@NationalityCountryID,@ImagePath) " +
                           "select scope_identity();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName != "" && ThirdName != null)
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", dateTime);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);

            if (Email != "" && Email != null)
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@NationalityCountryID", CountryID);

            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                if (int.TryParse(Resault.ToString(), out int _PersonID))
                {
                    PersonID = _PersonID;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in Add New Person \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return PersonID;
        }

        //done
        static public bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName,
                                        string ThirdName, string LastName, string Email, string Phone, int Gendor,
                                        string Address, DateTime dateTime, string ImagePath, int CountryID)
        {
            int rowEffected = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Update [dbo].[People] set [NationalNo]=@NationalNo,[FirstName]=@FirstName," +
                "[SecondName]=@SecondName,[ThirdName]=@ThirdName," +
                          "[LastName]=@LastName,[DateOfBirth]=@DateOfBirth,[Gendor]=@Gendor," +
                          "[Address]=@Address,[Phone]=@Phone,[Email]=@Email," +
                          "[NationalityCountryID]=@NationalityCountryID," +
                           "[ImagePath]=@ImagePath where PersonID=@PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName != "" && ThirdName != null)
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", dateTime);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);

            if (Email != "" && Email != null)
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@NationalityCountryID", CountryID);

            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in Update Person \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return (rowEffected > 0);
        }
        //done
        public static bool GetPersonInfoByID(int PersonID, ref string FirstName, ref string SecondName,
  ref string ThirdName, ref string LastName, ref string NationalNo, ref DateTime DateOfBirth,
   ref short Gendor, ref string Address, ref string Phone, ref string Email,
   ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select *from People where PersonID=@PersonID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];
                    Gendor = (byte)reader["Gendor"];
                    NationalityCountryID = (int)reader["NationalityCountryID"];
                    LastName = (string)reader["LastName"];
                    Phone = (string)reader["Phone"];
                    Address = (string)reader["Address"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    NationalNo = (string)reader["NationalNo"];


                    if (reader["ThirdName"] != DBNull.Value)
                        ThirdName = (string)reader["ThirdName"];
                    else
                        ThirdName = "";

                    if (reader["Email"] != DBNull.Value)
                        Email = (string)reader["Email"];
                    else
                        Email = "";

                    if (reader["ImagePath"] !=DBNull.Value)
                        ImagePath = (string)reader["ImagePath"];
                    else
                        ImagePath = "";

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


        public static bool GetPersonInfoByNationalNo(string NationalNo, ref string FirstName, ref string SecondName,
ref string ThirdName, ref string LastName, ref int PersonID, ref DateTime DateOfBirth,
 ref short Gendor, ref string Address, ref string Phone, ref string Email,
 ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select *from People where NationalNo=@NationalNo;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("NationalNo", NationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];
                    Gendor = (byte)reader["Gendor"];
                    NationalityCountryID = (int)reader["NationalityCountryID"];
                    LastName = (string)reader["LastName"];
                    Phone = (string)reader["Phone"];
                    Address = (string)reader["Address"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    PersonID = (int)reader["PersonID"];


                    if (reader["ThirdName"] != DBNull.Value)
                        ThirdName = (string)reader["ThirdName"];
                    else
                        ThirdName = "";

                    if (reader["Email"] != DBNull.Value)
                        Email = (string)reader["Email"];
                    else
                        Email = "";

                    if (reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)reader["ImagePath"];
                    else
                        ImagePath = "";

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
                    writer.WriteLine(" \t\t\t\tError in GetAllInfoByNationalNo \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        //done
        static public bool DeletePerson(int PersonID)
        {
            int rowEffected = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "delete [dbo].[People] where PersonID=@PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in Delete Person \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return (rowEffected > 0);
        }

//done
        static public bool IsPersonExist(string NationalNo)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "Select Found=1 From People where NationalNo=@NationalNo";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("NationalNo", NationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                
                IsFound = reader.HasRows;
                reader.Close();
            }
            catch(Exception ex)
            {
                IsFound = false;
                using(StreamWriter writer=new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\t\tError is Person exist \n" + ex.Message);
                }
            }
            return IsFound;
        }

        static public int GetPersonIDByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            int PersonID = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select People.PersonID from Applications inner join LocalDrivingLicenseApplications " +
                "on LocalDrivingLicenseApplications.ApplicationID=Applications.ApplicationID inner join People" +
                " on Applications.ApplicantPersonID=People.PersonID " +
                "where LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID;";

            
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                if (Resault != null && int.TryParse(Resault.ToString(), out int Number))
                {
                    PersonID = Number;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in GetPersonIDByLocalDrivingLicenseApplicationID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return PersonID;
        }

        static public int GetPersonIDByLicenseID(int LicenseID)
        {
            int PersonID = -1;
            SqlConnection connection = new SqlConnection(clsConnection.ConnectionString);
            string query = "select People.PersonID from Licenses inner join Applications " +
                "on Applications.ApplicationID=Licenses.ApplicationID inner join People " +
                "on Applications.ApplicantPersonID=People.PersonID where Licenses.LicenseID=@LicenseID;";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);


            try
            {
                connection.Open();
                object Resault = command.ExecuteScalar();
                if (Resault != null && int.TryParse(Resault.ToString(), out int Number))
                {
                    PersonID = Number;
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(clsConnection.FileName))
                {
                    writer.WriteLine("\t\t\tError in GetPersonIDByLicenseID \n" + ex.Message);
                }
            }
            finally
            {
                connection.Close();
            }
            return PersonID;
        }

    }

}



