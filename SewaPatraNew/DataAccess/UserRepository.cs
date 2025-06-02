using Microsoft.Data.SqlClient;
using SewaPatra.Models;

namespace SewaPatra.DataAccess
{
    public class UserRepository
    {
        private readonly string _connectionString;
        public UserRepository(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public UserRegister GetUserByNumber(string number)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, FullName, Password, Email,Role,Number FROM Users WHERE Number = @number";
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
                                Email = (string)reader["Email"],
                                Role = (string)reader["Role"],
                                Number = (string)reader["Number"]
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
        public bool AddUser(UserRegister user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (FullName, Number, Email, Password, Role) VALUES (@FullName, @Number, @Email, @Password, @Role)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", user.FullName);
                    command.Parameters.AddWithValue("@Number", user.Number);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Role", user.Role);
                    int rowaffected = command.ExecuteNonQuery();
                    return rowaffected > 0;
                }
            }
        }
        public bool ChangePassword(UserRegister user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "Update Users SET Password = @Password WHERE Number=@Number;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Number", user.Number);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    int rowaffected = command.ExecuteNonQuery();
                    return rowaffected > 0;
                }
            }
        }
        public bool CreateCoordinatorUser(Coordinator coordinator) 
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (FullName, Number, Email, Password, Role) VALUES (@FullName, @Number, @Email, @Password, @Role)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", coordinator.Name);
                    command.Parameters.AddWithValue("@Number", coordinator.Mobile_No);
                    command.Parameters.AddWithValue("@Email", coordinator.Email);
                    command.Parameters.AddWithValue("@Password", coordinator.Password);
                    command.Parameters.AddWithValue("@Role", "3");
                    int rowaffected = command.ExecuteNonQuery();
                    return rowaffected > 0;
                }
            }
        }

    }
}
