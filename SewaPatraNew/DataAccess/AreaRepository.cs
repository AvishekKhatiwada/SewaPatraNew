using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SewaPatra.Models;

namespace SewaPatra.DataAccess
{
    public class AreaRepository
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AreaRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _httpContextAccessor = httpContextAccessor;
        }

        public bool InsertArea(Area area)
        {
            string Username = _httpContextAccessor.HttpContext?.Session?.GetString("Username");
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    //string query = "INSERT INTO Area_Master (Area_name, Area_type, Under) VALUES (@Area_name, @Area_type, @Under)";
                    string query = "INSERT INTO Area_Master (Area_name, Area_type, Under, CreatedBy) VALUES (@Area_name, @Area_type, @Under, @UserName)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Area_name", area.Area_name);
                    cmd.Parameters.AddWithValue("@Area_type", area.Area_type);
                    cmd.Parameters.AddWithValue("@Under", area.Under);
                    cmd.Parameters.AddWithValue("@UserName", Username);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
        public List<Area> GetAllAreas()
        {
            List<Area> areas = new List<Area>();
            string UserRole = _httpContextAccessor.HttpContext?.Session?.GetString("userRole");
            
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Area_name, Area_type, Under FROM Area_Master WHERE 1=1";
                if (UserRole == "3")
                {
                    string Username = _httpContextAccessor.HttpContext?.Session?.GetString("Username");
                    if (Username != null)
                    {
                        query += "AND Id=(SELECT Area_Under FROM Coordinator_master where Name=@Username)";                        
                    }
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                if(UserRole == "3" && _httpContextAccessor.HttpContext?.Session?.GetString("Username") != null)
                {
                    cmd.Parameters.AddWithValue("@Username", _httpContextAccessor.HttpContext?.Session?.GetString("Username"));
                }
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        areas.Add(new Area
                        {
                            Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                            Area_name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            Area_type = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Under = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                        });
                    }
                }
            }
            return areas;
        }
        public Area GetAreaById(int id)
        {
            Area area = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                //string query = "SELECT Id, Area_name, Area_type, Under FROM Area_Master WHERE Id = @Id";
                string query = "SELECT ID,Area_name,Area_type,Under FROM Area_Master WHERE ID = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        area = new Area
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Area_name = reader["Area_name"].ToString(),
                            Area_type = reader["Area_type"].ToString(),
                            Under = reader["Under"].ToString()
                        };
                    }
                }
            }
            return area;
        }
        public bool UpdateArea(Area area)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                //string query = "UPDATE Area_Master SET Area_name = @Area_name, Area_type = @Area_type, Under = @Under WHERE Id = @Id";
                string query = "UPDATE Area_Master SET Area_name = @Area_name, Area_type = @Area_type, Under = @Under WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", area.Id);
                cmd.Parameters.AddWithValue("@Area_name", area.Area_name);
                cmd.Parameters.AddWithValue("@Area_type", area.Area_type);
                cmd.Parameters.AddWithValue("@Under", area.Under);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                return rowsAffected > 0;
            }
        }
        public bool DeleteArea(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Area_Master WHERE Id = @Id";
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
