using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using BussinseLayer;

namespace Course19
{
    class clsGlobal
    {
        static public clsUsers CurrentUser;
        static public bool RemeberUsernameAndPassword(string Username,string Password)
        {
            try 
            {
                string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();
                string FilePath = CurrentDirectory + "\\Data.txt";

                if (Username == "" && File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                    return true;
                }

                string dataToSave = Username + "#" + Password;

                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    writer.WriteLine(dataToSave);
                    return true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
           
        }

        static public bool GetStoredCredential(ref string Username,ref string Password)
        {
            try
            {
                string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

                string FilePath = CurrentDirectory + "\\data.txt";

                if (File.Exists(FilePath))
                {
                    using (StreamReader reader = new StreamReader(FilePath))
                    {
                        string line;
                        if ((line = reader.ReadLine()) != null)
                        {
                            string[] data = line.Split('#');

                            Username = data[0];
                            Password = data[1];

                        }
                        return true;
                    }
                   
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}
