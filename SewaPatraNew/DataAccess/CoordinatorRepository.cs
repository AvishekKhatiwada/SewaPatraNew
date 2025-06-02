using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SewaPatra.Models;

namespace SewaPatra.DataAccess
{
    public class CoordinatorRepository
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CoordinatorRepository(IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _httpContextAccessor = httpContextAccessor;
        }

        public bool InsertCoordinator(Coordinator coordinator)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Coordinator_master (Name, Mobile_No, Alternate_Mobile_No,Address,City,Email,Area_Under,Active,Report_to,CreatedAt) 
                                 VALUES (@Name, @Mobile_No, @Alternate_Mobile_No,@Address,@City,@Email,@Area_Under,@Active,@ReportTo,@CreatedAt);";
                if (coordinator.Password != null)
                {
                    query += "INSERT INTO Users (FullName, Number, Email, Password, Role) SELECT @Name, @Mobile_No, @Email, @Password, 1 WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Number = @Mobile_No OR Email = @Email);";
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", (object)coordinator.Name ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Mobile_No", (object)coordinator.Mobile_No ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Alternate_Mobile_No", (object)coordinator.Alternate_Mobile_No ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object)coordinator.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@City", (object)coordinator.City ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)coordinator.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Area_Under", (object)coordinator.Area_Under ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", (object)coordinator.Active ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ReportTo", (object)coordinator.Report_to ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedAt", (object)coordinator.CreatedAt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Password", (object)coordinator.Password ?? DBNull.Value);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                return rowsAffected > 0;
            }
        }
        public List<Coordinator> GetAllCoordinator()
        {
            List<Coordinator> coordinator = new List<Coordinator>();
            string UserRole = _httpContextAccessor.HttpContext?.Session?.GetString("userRole");
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT CM.ID, Name, Mobile_No, Alternate_Mobile_No, [Address], City, Email, Area_Under, AM.Area_name , Active, Report_to 
                                    from Coordinator_master CM
                                    INNER JOIN Area_Master AM ON AM.ID=Cm.Area_Under Where 1=1";
                if(UserRole == "3")
                {
                    string Username = _httpContextAccessor.HttpContext?.Session?.GetString("Username");
                    query += "AND CM.Name=@Username";
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                if (UserRole == "3" && _httpContextAccessor.HttpContext?.Session?.GetString("Username") != null)
                {
                    cmd.Parameters.AddWithValue("@Username", _httpContextAccessor.HttpContext?.Session?.GetString("Username"));
                }
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        coordinator.Add(new Coordinator
                        {
                            Id = reader.IsDBNull(0) ? 0 : Convert.ToInt32(reader["ID"]),
                            Name = reader.IsDBNull(1) ? string.Empty : reader["Name"].ToString(),
                            Mobile_No = reader.IsDBNull(2) ? string.Empty : reader["Mobile_No"].ToString(),
                            Alternate_Mobile_No = reader.IsDBNull(3) ? string.Empty : reader["Alternate_Mobile_No"].ToString(),
                            Address = reader.IsDBNull(4) ? string.Empty : reader["Address"].ToString(),
                            City = reader.IsDBNull(5) ? string.Empty : reader["City"].ToString(),
                            Email = reader.IsDBNull(6) ? string.Empty : reader["Email"].ToString(),
                            Area_Under = reader.IsDBNull(7) ? 0 : Convert.ToInt32(reader["Area_Under"]),
                            AreaName = reader.IsDBNull(8) ? string.Empty : reader["Area_name"].ToString(),
                            Report_to = reader.IsDBNull(10) ? string.Empty : reader["Report_to"].ToString(),
                            Active = reader.GetBoolean(9)
                        });
                    }
                }
            }
            return coordinator;
        }
        public Coordinator GetCoordinatorById(int id)
        {
            Coordinator coordinator = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Coordinator_master WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        coordinator = new Coordinator
                        {
                            Id = Convert.ToInt32(reader["ID"]),
                            Name = reader["Name"].ToString(),
                            Mobile_No = reader["Mobile_No"].ToString(),
                            Alternate_Mobile_No = reader["Alternate_Mobile_No"].ToString(),
                            Address = reader["Address"].ToString(),
                            City = reader["City"].ToString(),
                            Email = reader["Email"].ToString(),
                            Area_Under = Convert.ToInt32(reader["Area_Under"]),
                            Report_to = reader["Report_to"].ToString(),
                            Active = Convert.ToBoolean(reader["Active"])
                        };
                    }
                }
            }
            return coordinator;
        }
        public bool UpdateArea(Coordinator coordinator)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Coordinator_master SET Name = @Name, Mobile_No = @Mobile_No, Alternate_Mobile_No = @Alternate_Mobile_No,Address=@Address,City=@City,Email=@Email,Area_Under=@Area_Under,Active=@Active WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", coordinator.Id);
                cmd.Parameters.AddWithValue("@Name", coordinator.Name);
                cmd.Parameters.AddWithValue("@Mobile_No", coordinator.Mobile_No);
                cmd.Parameters.AddWithValue("@Alternate_Mobile_No", coordinator.Alternate_Mobile_No);
                cmd.Parameters.AddWithValue("@Email", coordinator.Email);
                cmd.Parameters.AddWithValue("@Area_Under", coordinator.Area_Under);
                cmd.Parameters.AddWithValue("@Address", coordinator.Address);
                cmd.Parameters.AddWithValue("@City", coordinator.City);
                cmd.Parameters.AddWithValue("@Active", coordinator.Active);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                return rowsAffected > 0;
            }
        }
        public bool DeleteCoordinator(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "Update Coordinator_master Set Active = 0 WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                return rowsAffected > 0;
            }
        }


    }
}













































//using Microsoft.Data.SqlClient;
//using SewaPatra.Models;

//namespace SewaPatra.DataAccess
//{
//    public class CoordinatorRepository
//    {
//        private readonly string _connectionString;

//        public CoordinatorRepository(IConfiguration configuration)
//        {
//            _connectionString = configuration.GetConnectionString("DefaultConnection");
//        }
//        public bool InsertCoordinator(Coordinator coordinator) 
//        {
//            using (SqlConnection connection = new SqlConnection(_connectionString))
//            {
//                string sql = @"INSERT INTO Coordinator (Name, Address, City, Mobile_No, Alt_Mobile_no, Email, Area, location, Active) 
//                                VALUES (@Name, @Address, @City, @Mobile_No, @Alt_Mobile_no, @Email, @Area, @location, @Active)";
//                SqlCommand cmd = new SqlCommand(sql, connection);
//                cmd.Parameters.AddWithValue("@Name", coordinator.Name);
//                cmd.Parameters.AddWithValue("@Address", coordinator.Address);
//                cmd.Parameters.AddWithValue("@City", coordinator.City);
//                cmd.Parameters.AddWithValue("@Mobile_No", coordinator.Mobile_No);
//                cmd.Parameters.AddWithValue("@Alt_Mobile_no", coordinator.Alt_Mobile_no);
//                cmd.Parameters.AddWithValue("@Email", coordinator.Email);
//                cmd.Parameters.AddWithValue("@Area", coordinator.Area);
//                cmd.Parameters.AddWithValue("@location", coordinator.location);
//                cmd.Parameters.AddWithValue("@Active", coordinator.Active);
//                connection.Open();
//                int count = cmd.ExecuteNonQuery();
//                connection.Close();
//                return count > 0;
//            }
//        }
//    }
//}
