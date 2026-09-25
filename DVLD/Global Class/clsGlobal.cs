using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using BussinseLayer;
using Microsoft.Win32;

namespace Course19
{
    class clsGlobal
    {
        static public clsUsers CurrentUser;
        static private bool LogInWindowsRegister(string Username, string Password)
        {
            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\USERINFO";
            string valueName = "Username";
            string valueData = Username.ToString();
            string valueName2 = "Password";
            string valueData2 = Password.ToString();

            try
            {
                Registry.SetValue(KeyPath, valueName, valueData, RegistryValueKind.String);
                Registry.SetValue(KeyPath, valueName2, valueData2, RegistryValueKind.String);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        static public bool RemeberUsernameAndPassword(string Username,string Password)
        {
            //try 
            //{
            //    string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();
            //    string FilePath = CurrentDirectory + "\\Data.txt";

            //    if (Username == "" && File.Exists(FilePath))
            //    {
            //        File.Delete(FilePath);
            //        return true;
            //    }

            //    string dataToSave = Username + "#" + Password;

            //    using (StreamWriter writer = new StreamWriter(FilePath))
            //    {
            //        writer.WriteLine(dataToSave);
            //        return true;
            //    }
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show($"An error occurred: {ex.Message}");
            //    return false;
            //}

            return LogInWindowsRegister(Username, Password);


        }

        static private bool ReadFromWindowsRegister(ref string Username,ref string Password)
        {
            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\USERINFO";
            string valueName = "Username";
            string valueName2 = "Password";

            try
            {
                Username = Registry.GetValue(KeyPath, valueName, null) as string;
                Password = Registry.GetValue(KeyPath, valueName2, null) as string;

                if (Username != null && Password != null)
                    return true;
                
            }
            catch(Exception ex)
            {

            }
            return false;
        }
        static public bool GetStoredCredential(ref string Username,ref string Password)
        {
            //try
            //{
            //    string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

            //    string FilePath = CurrentDirectory + "\\data.txt";

            //    if (File.Exists(FilePath))
            //    {
            //        using (StreamReader reader = new StreamReader(FilePath))
            //        {
            //            string line;
            //            if ((line = reader.ReadLine()) != null)
            //            {
            //                string[] data = line.Split('#');

            //                Username = data[0];
            //                Password = data[1];

            //            }
            //            return true;
            //        }

            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show($"An error occurred: {ex.Message}");
            //    return false;
            //}

           return ReadFromWindowsRegister(ref Username, ref Password);
        }
    }
}
