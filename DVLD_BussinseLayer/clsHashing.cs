using System;
using System.Security.Cryptography;
using System.Text;


namespace BussinseLayer
{
   static public class clsHashing
    {
        static public string ComputeHash(string input)
        {
            using(SHA256 sha256 = SHA256.Create())
            {
                byte[] hashData = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashData).Replace("-", "").ToLower();
            }
        }
    }
}
