using System;
using System.Net.NetworkInformation;
using CA_Team5.Models;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace CA_Team5.Data
{
    public class ProductData
    {
        public ProductData()
        {
        }
        public static List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            string connectionString = @"Server=(local); Database=ShoppingCartTeam5; Integrated Security = True ; Encrypt=False";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM Products";
                SqlCommand cmd = new SqlCommand(sql, conn);
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

                    products.Add(product);
                }
            }
            return products;
        }
        public static List<Product> GetAllMatches(string text)
        {
            List<Product> searches = new List<Product>();
            string connectionString = @"Server=(local); Database=ShoppingCartTeam5; Integrated Security = true; encrypt = false";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"select * from Products where ProductName like ('%" + text + "%') or [Description] like ('%" + text + "%')";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Product search = new Product()
                    {

                        ProductId = (Guid)reader["ProductId"],
                        ProductName = (string)reader["ProductName"],
                        UnitPrice = (int)reader["UnitPrice"],
                        Description = (string)reader["Description"],
                        Image = (string)reader["Image"]

                    };

                    searches.Add(search);
                }
            }
            return searches;
        }
        public static Product GetProductById(String text)
        {
            Product products = new Product();
            string connectionString = @"Server=(local); Database=ShoppingCartTeam5; Integrated Security = true; encrypt = false"; 

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"select * from Products where ProductId='" + text + "'";

                SqlCommand cmd = new SqlCommand(sql, conn);
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

                    products = product;
                }
            }
            return products;
        }
        //public static int ToCart(User user, Product product)
        //{
        //    bool status = false;
        //    string connectionString = @"Server=localhost; Database=ShoppingCartTeam5; user Id=sa; Password=yourStrong(!)Password ; TrustServerCertificate=True";
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
                

        //            string q = String.Format(@"INSERT INTO Cart(
        //            UserId, ProductId,ProductName,Image,Description,UnitPrice) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
        //            user.UserId, product.ProductId, product.ProductName, product.Image, product.Description, product.UnitPrice);

        //            using (SqlCommand cmd = new SqlCommand(q, conn))
        //            {
        //                if (cmd.ExecuteNonQuery() == 1)
        //                {
        //                    status = true;
        //                }
        //            }
                
        //        conn.Close();
        //    }

        //    //return status;
        //    return GetCount(user.UserId);
        //}
        public static int GetCount(int id)
        {

            string connectionString = @"Server=localhost; Database=ShoppingCartTeam5; Integrated Security = true; encrypt = false";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"select count(ProductId) as count from Cart where UserId="+id;

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                int cou=0;
                while (reader.Read())
                {
                    cou = (int)reader["count"];
                }
                return cou;

            }

        }
        public static List<Cart> GetUserCart(int id)
        {
            List<Cart> carts = new List<Cart>();
            string connectionString = @"Server=localhost; Database=ShoppingCartTeam5; Integrated Security = true; encrypt = false";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"select productid, description, unitprice, productname,image, count(productid) as qty from cart where userid='" + id + "' group by productid, description, unitprice, productname,image";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Cart cart = new Cart()
                    {

                        ProductId = (Guid)reader["ProductId"],
                        ProductName = (string)reader["ProductName"],
                        UnitPrice = (int)reader["UnitPrice"],
                        Description = (string)reader["Description"],
                        Image = (string)reader["Image"],
                        Qty = (int)reader["Qty"]
                        
                    };

                    carts.Add(cart);
                }
            }
            return carts;

        }
        public static bool RemoveFromCart(int id)
        {
            bool status = false;
            string connectionString = @"Server=localhost; Database=ShoppingCartTeam5; Integrated Security = true; encrypt = false";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"Delete from cart where UserId="+id;

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        status = true;
                    }
                }

                conn.Close();
            }

            return status;
        
    
        }

        public static bool AddToPurchase(List<Cart> cartLists,int id)
        {
            bool status = false;
            string connectionString = @"Server=localhost; Database=ShoppingCartTeam5; Integrated Security = true; encrypt = false";
            foreach (var cart in cartLists)
            {
                Guid pId = Guid.NewGuid();
                Guid aCode = Guid.NewGuid();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string q = String.Format(@"INSERT INTO PurchaseHistory(
                    PurchaseId, UserId,ProductId,ActivationCode,PurchaseDate) VALUES('{0}','{1}','{2}','{3}','{4}')",
                    pId, id, cart.ProductId, aCode,DateTime.Now.ToString());

                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            status = true;
                        }
                    }
                    
                    conn.Close();
                }
            }

            //return status;
            return status;
        }
        public static List<Cart> GetCartById(int id)
        {
            List<Cart> carts = new List<Cart>();
            string connectionString = @"Server=localhost; Database=ShoppingCartTeam5; Integrated Security = true; encrypt = false";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"Select * from Cart where userid="+id;

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Cart cart = new Cart()
                    {

                        ProductId = (Guid)reader["ProductId"],
                        ProductName = (string)reader["ProductName"],
                        UnitPrice = (int)reader["UnitPrice"],
                        Description = (string)reader["Description"],
                        Image = (string)reader["Image"]
                      

                    };

                    carts.Add(cart);
                }
            }
            return carts;

        }
        public static int FromCart(User user, Product product)
        {
            bool status = false;
            string connectionString = @"Server=localhost; Database=ShoppingCartTeam5; Integrated Security = true; encrypt = false";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string q = String.Format(@"DELETE top(1) FROM Cart WHERE ProductId = '{0}' AND UserId = '{1}'",
                product.ProductId, user.UserId);
                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        status = true;
                    }
                }

                conn.Close();
            }

            //return status;
            return GetCount(user.UserId);
        }
        public static int GetProductQty(User user, Product product)
        {

            string connectionString = @"Server=localhost; Database=ShoppingCartTeam5; Integrated Security = true; encrypt = false";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = String.Format(@"select count(productName) as qty from Cart where ProductId = '{0}' AND UserId = '{1}'",
                product.ProductId, user.UserId);

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                int qty = 0;
                while (reader.Read())
                {
                    qty = (int)reader["qty"];
                }
                return qty;

            }

        }
        public static int ToCart(User user, Product product)
        {
            bool status = false;
        
            string connectionString = @"Server=localhost; Database=ShoppingCartTeam5; Integrated Security = true; encrypt = false";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string q = String.Format(@"INSERT INTO Cart(
                    UserId, ProductId,ProductName,Image,Description,UnitPrice) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                user.UserId, product.ProductId, product.ProductName, product.Image, product.Description, product.UnitPrice);

                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        status = true;
                    }
                }

                conn.Close();
            }

            //return status;
            return GetCount(user.UserId);
        }
    }
    
}

