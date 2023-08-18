using System;
using System.Security.Cryptography;
using System.Text;

namespace CA_Team5.Data
{
	public class HashPassword
	{
        public static string Hashing(string password)
        {
            MD5 md5 = MD5.Create();



            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(password));



            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();



            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }



            // Return the hexadecimal string.
            return sBuilder.ToString();


        }
	}
}

