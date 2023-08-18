using CA_Team5.Models;
using Microsoft.Data.SqlClient;
namespace CA_Team5.Data
{
    public class LoginData
    {
        public static User GetUserByUsername(string username1)
        {
            User userdata = new User();
            string connectionString = @"Server=(local); Database=ShoppingCartTeam5; Integrated Security = true; encrypt = false";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT UserId,FirstName,LastName,Password FROM UserAccounts where UserName='"+username1+"'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User()
                    {
                        UserId = (int)reader["UserId"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        Password = (string)reader["Password"]

                    };
                    
                    userdata = user;
                }
            }
            return userdata;
        }
        public static string AddSession(int userId)
        {
            string sessionId = null;
            Guid guid = Guid.NewGuid();
            string connectionString = @"Server=(local); Database=ShoppingCartTeam5; Integrated Security = true; encrypt = false";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string q = String.Format(@"INSERT INTO Session(
                SessionId, UserId, Timestamp) VALUES('{0}', '{1}', {2})",
                        guid, userId, DateTimeOffset.Now.ToUnixTimeSeconds());

                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        sessionId = guid.ToString();
                    }
                }

                conn.Close();
            }

            return sessionId;
        }
        public static bool RemoveSession(string sessionId)
        {
            bool status = false;
            string connectionString = @"Server=(local); Database=ShoppingCartTeam5; Integrated Security = true; encrypt = false";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string q = String.Format(@"DELETE FROM Session
                WHERE sessionId = '{0}'", sessionId);

                using (SqlCommand cmd = new SqlCommand(q, conn))
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
        public static User GetUserBySession(string sessionId)
        {
            if (sessionId == null)
            {
                return null;
            }
            User user = new User();
            string connectionString = @"Server=(local); Database=ShoppingCartTeam5; Integrated Security = true; encrypt = false";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string q = String.Format(@"SELECT u.UserId, u.Username, u.Password,u.FirstName,u.Lastname
             FROM Session s, UserAccounts u
                WHERE s.UserId = u.UserId AND 
                    s.SessionId='{0}'", sessionId);

                using (SqlCommand cmd = new SqlCommand(q, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                UserId = (int)reader["UserId"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                Password = (string)reader["Password"]
                            };
                        }
                    }
                }

                conn.Close();
            }

            return user;
        }

        public static bool CheckUserName(string username)
        {
            if (GetUserByUsername(username).UserName != username)

            { return true; }

            else
            { return false; }
        }

        public static bool CheckPassword(string username, string password)
        {
            if (GetUserByUsername(username).Password == password)

            { return true; }

            else
            { return false; }
        }
    }
}

