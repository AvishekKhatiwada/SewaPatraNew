using Microsoft.Data.SqlClient;
using SewaPatra.Models;

namespace SewaPatra.DataAccess
{
    public class SewaPatraReceiptRepository
    {
        private readonly string _connectionString;
        public SewaPatraReceiptRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public bool InsertSewaPatraReceipt(SewaPatraReceipt sewaPatraReceipt)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO SewaPatraReceipt (SPR_TranId, Date, Donor, Coordinator, DonationBox, Receive_Date, Remarks, SPI_Id) 
                                 VALUES (@TranId,@EnteredDate,@Donor,@Coordinator,@DonationBox,@ReceiptDate,@remarks,@Spi_ID)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TranId", sewaPatraReceipt.SPR_TranId);
                cmd.Parameters.AddWithValue("@EnteredDate", sewaPatraReceipt.Date);
                cmd.Parameters.AddWithValue("@Donor", sewaPatraReceipt.Donor);
                cmd.Parameters.AddWithValue("@Coordinator", sewaPatraReceipt.Coordinator);
                cmd.Parameters.AddWithValue("@DonationBox", sewaPatraReceipt.DonationBox);
                cmd.Parameters.AddWithValue("@ReceiptDate", sewaPatraReceipt.Receive_Date);
                cmd.Parameters.AddWithValue("@remarks", sewaPatraReceipt.Remarks);
                cmd.Parameters.AddWithValue("@Spi_ID", sewaPatraReceipt.SPI_TranId);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                return rowsAffected > 0;
            }
        }
        public List<SewaPatraReceipt> GetAllSewaPatraReceipt()
        {
            List<SewaPatraReceipt> sewaPatraReceipt = new List<SewaPatraReceipt>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT SPR.*,DM.Name As DonorName,CM.Name As [CoordinatorName],DB.Box_Number 
                                    FROM SewaPatraReceipt SPR
                                    INNER JOIN DonationBox DB ON DB.Id=SPR.DonationBox
                                    INNER JOIN Donor_master DM ON Dm.Id=SPR.Donor
                                    INNER JOIN Coordinator_master CM ON CM.ID=SPR.Coordinator
                                    ORDER BY SPR.Date";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sewaPatraReceipt.Add(new SewaPatraReceipt
                        {
                            SPR_TranId = reader["SPR_TranId"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]),
                            Donor = Convert.ToInt32(reader["Donor"]),
                            Coordinator = Convert.ToInt32(reader["Coordinator"]),
                            DonationBox = Convert.ToInt32(reader["DonationBox"]),
                            Receive_Date = Convert.ToDateTime(reader["Receive_Date"]),
                            Remarks = reader["Remarks"].ToString(),
                            DonorName = reader["DonorName"].ToString(),
                            CoordinatorName = reader["CoordinatorName"].ToString(),
                            DonationBoxName = reader["Box_Number"].ToString()
                        });
                    }
                }
                conn.Close();
                return sewaPatraReceipt;
            }
        }
        public SewaPatraReceipt GetSewaPatraReceiptById(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM SewaPatraReceipt WHERE SPR_TranId = @TranId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TranId", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SewaPatraReceipt sewaPatraReceipt = new SewaPatraReceipt();
                if (reader.Read())
                {
                    sewaPatraReceipt.SPR_TranId = reader["SPR_TranId"].ToString();
                    sewaPatraReceipt.Date = Convert.ToDateTime(reader["Date"]);
                    sewaPatraReceipt.Donor = Convert.ToInt32(reader["Donor"]);
                    sewaPatraReceipt.Coordinator = Convert.ToInt32(reader["Coordinator"]);
                    sewaPatraReceipt.DonationBox = Convert.ToInt32(reader["DonationBox"]);
                    sewaPatraReceipt.Receive_Date = Convert.ToDateTime(reader["Receive_Date"]);
                    sewaPatraReceipt.Remarks = reader["Remarks"].ToString();
                    //sewaPatraReceipt.SPI_TranId = reader["SPI_Id"].ToString();
                }
                conn.Close();
                return sewaPatraReceipt;
            }
        }
        public bool UpdateSewaPatraReceipt(SewaPatraReceipt sewaPatraReceipt)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE SewaPatraReceipt SET Date = @EnteredDate, Donor = @Donor, Coordinator = @Coordinator, DonationBox = @DonationBox, Receive_Date = @ReceiptDate, Remarks = @remarks WHERE SPR_TranId = @TranId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TranId", sewaPatraReceipt.SPR_TranId);
                cmd.Parameters.AddWithValue("@EnteredDate", sewaPatraReceipt.Date);
                cmd.Parameters.AddWithValue("@Donor", sewaPatraReceipt.Donor);
                cmd.Parameters.AddWithValue("@Coordinator", sewaPatraReceipt.Coordinator);
                cmd.Parameters.AddWithValue("@DonationBox", sewaPatraReceipt.DonationBox);
                cmd.Parameters.AddWithValue("@ReceiptDate", sewaPatraReceipt.Receive_Date);
                cmd.Parameters.AddWithValue("@remarks", sewaPatraReceipt.Remarks);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                return rowsAffected > 0;
            }
        }
        public bool DeleteSewaPatraReceipt(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM SewaPatraReceipt WHERE SPR_TranId = @TranId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TranId", id);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                return rowsAffected > 0;
            }
        }
        public string GetLastTransactionId()
        {
            string transactionId = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT TOP 1 SPR_TranId FROM SewaPatraReceipt ORDER BY SPR_TranId DESC";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            transactionId = reader.IsDBNull(0) ? string.Empty : reader["SPR_TranId"].ToString();
                        }
                    }
                }
            }

            return transactionId;
        }
    }
}
