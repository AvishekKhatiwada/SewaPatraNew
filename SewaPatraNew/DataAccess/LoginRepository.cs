using Microsoft.Data.SqlClient;
using SewaPatra.Models;

namespace SewaPatra.DataAccess
{
    public class LoginRepository
    {
        private readonly string _connectionString;
        public LoginRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public UserRegister GetUserByNumber(string number)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, FullName, Password, Email FROM Users WHERE Number = @number";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@number", number);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserRegister
                            {
                                Id = (int)reader["Id"],
                                FullName = (string)reader["FullName"],
                                Password = (string)reader["Password"],
                                Email = (string)reader["Email"]
                            };
                        }
                        return null;
                    }
                }
            }
        }
        public UserRegister GetUserByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, FullName, Password, Email FROM Users WHERE Email = @Email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserRegister
                            {
                                Id = (int)reader["UserId"],
                                FullName = (string)reader["Username"],
                                Password = (string)reader["PasswordHash"],
                                Email = (string)reader["Email"]
                            };
                        }
                        return null;
                    }
                }
            }
        }

    }
}
