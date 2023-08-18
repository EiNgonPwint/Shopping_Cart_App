using System;
using System.Reflection.Emit;
using Microsoft.Data.SqlClient;
using CA_Team5.Models;
using System.Net.NetworkInformation;

namespace CA_Team5.Data
{
    public class RatingData
    {

        public static bool SetRateDb(int user, string id)
        {
            int rate1 = 0;
        
            bool status = false;
            string[] splitstring = id.Split('/');
            string ProductName = splitstring[0];
            int Rate = Convert.ToInt32(splitstring[1]);

            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string qr = String.Format(@"Select Rate from Rating where ProductName='{0}' and UserId={1}", ProductName,user);
               
                    SqlCommand cmd = new SqlCommand(qr, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rate1 = (int)reader["Rate"];
              
                }
                conn.Close();
                conn.Open();
                if (rate1 != 0)
                {
                    string q = String.Format(@"UPDATE Rating 
                SET Rate = '{0}' 
                    WHERE UserId = '{1}' AND ProductName = '{2}'",
                                Rate, user, ProductName);
                    using (SqlCommand cmd1 = new SqlCommand(q, conn))
                    {
                        if (cmd1.ExecuteNonQuery() == 1)
                        {
                            status = true;
                        }
                    }
                }
                else
                {
                    
                    string q = String.Format(@"Insert into Rating Values('{0}',{1},{2})", ProductName,Rate, user);
                             
                    using (SqlCommand cmd1 = new SqlCommand(q, conn))
                    {
                        if (cmd1.ExecuteNonQuery() == 1)
                        {
                            status = true;
                        }
                    }
                }

                conn.Close();
            }

            return status;
        }
        public static Dictionary<string,int> GetAvg()
        {
            Dictionary<string, int> avgs = new Dictionary<string, int>();
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string sql = @"select sum(rate) as rate,productName,count(productName ) as count from rating where userid!=0 GROUP by ProductName";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AvgRating avg = new AvgRating()
                {

                        AvgRate = (int)reader["rate"] / (int)reader["count"],
                        ProductName = (string)reader["ProductName"]
  
                };
                    avgs.Add(avg.ProductName,avg.AvgRate);

                }
            }
            return avgs;
        }
        public static int GetRate(int user, string ProductName)
        {
            int rate = 0;
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string sql = @"SELECT Rate FROM Rating WHERE ProductName = '" + ProductName + "' AND Userid =" + user;
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rate = (int)reader["Rate"];


                }
                reader.Close();
            }
            return rate;
        }

    }
}

