using System;
using CA_Team5.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CA_Team5.Data
{
    public class PurchaseData
    {
        public static List<PurchaseHistory> GetPurchaseHistoryByCustomerId(int UserId)
        {
            List<PurchaseHistory> purchasehistory = new List<PurchaseHistory>();
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM PurchaseHistory WHERE UserId = @UserId ";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PurchaseHistory purchase = new PurchaseHistory()
                    {
                        PurchaseId = (Guid)reader["PurchaseId"],
                        UserId = (int)reader["UserId"],
                        ProductId = (Guid)reader["ProductId"],
                        ActivationCode = (Guid)reader["ActivationCode"],
                        PurchaseDate = (String)reader["PurchaseDate"]
                    };
                    purchasehistory.Add(purchase);
                }
                reader.Close();
            }
            return purchasehistory;
        }

        public static void CreatePurchase(int UserId, Guid ProductId)
        {
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                int uid = UserId;
                Guid pid = ProductId;
                Guid puid = Guid.NewGuid();
                Guid ac = Guid.NewGuid();
                string sql = @"insert into PurchaseHistory(PurchaseId,UserId,ProductId,ActivationCode,PurchaseDate) values ('" + puid + "'," + uid + ",'" + pid + "','" + ac + "'," + ",GetDate())";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

            }
        }

        public static List<Product> Distinct(int UserId)
        {
            List<Product> distinctproducts = new List<Product>();
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM Products WHERE ProductId IN (SELECT DISTINCT ProductId FROM PurchaseHistory WHERE UserId =" + UserId + ")";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product()
                    {

                        ProductId = (Guid)reader["ProductId"],
                        ProductName = (string)reader["ProductName"],
                        UnitPrice = (int)reader["UnitPrice"],
                        Description = (string)reader["Description"],
                        Image = (string)reader["Image"]

                    };

                    distinctproducts.Add(product);
                }
                reader.Close();
            }
            return distinctproducts;
        }

      

        public static int CountProduct(Guid ProductId,int userid)
        {
            int count = 0;
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();

                string sql = @"select count( ProductId) from PurchaseHistory where ProductId= '" + ProductId + "' AND Userid="+userid;
                SqlCommand cmd = new SqlCommand(sql, conn);
                count = (int)cmd.ExecuteScalar();
            }

            return count;
        }


        public static List<string> GetDateofPurchase(int UserId, Guid ProductId)
        {
            List<string> dates = new List<string>();
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string sql = @"SELECT DISTINCT PurchaseDate FROM PurchaseHistory WHERE ProductId = '" + ProductId + "' AND UserId = " + UserId;
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string date = (string)reader["PurchaseDate"];


                    dates.Add(date);
                }
                reader.Close();
            }
            return dates;
        }

        public static List<Guid> GetAccountActivationCodes(int UserId, Guid ProductId)
        {
            List<Guid> codes = new List<Guid>();
            using (SqlConnection conn = new SqlConnection(Data.connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM PurchaseHistory WHERE ProductId = '" + ProductId + "' AND UserId = '" + UserId + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Guid code = (Guid)reader["ActivationCode"];

                    codes.Add(code);
                }
                reader.Close();
            }
            return codes;
        }
    }
}

