using Microsoft.Data.SqlClient;
using SewaPatra.Models;

namespace SewaPatra.DataAccess
{
    public class ReceiptVoucherRepository
    {
        private readonly string _connectionString;
        public ReceiptVoucherRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public bool InsertReceiptVoucher(ReceiptVoucher receiptVoucher)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO ReceiptVoucher (R_TranId, Date, Sewapatra_No, Donor, Coordinator, Amount, Next_DueDate, Remarks) 
                                 VALUES (@R_TranId,@Date,@Sewapatra_No,@Donor,@Coordinator,@Amount,@NextDueDate,@remarks)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@R_TranId", receiptVoucher.R_TranId);
                cmd.Parameters.AddWithValue("@Date", receiptVoucher.Date);
                cmd.Parameters.AddWithValue("@Sewapatra_No", receiptVoucher.Sewapatra_No);
                cmd.Parameters.AddWithValue("@Donor", receiptVoucher.Donor);
                cmd.Parameters.AddWithValue("@Coordinator", receiptVoucher.Coordinator);
                cmd.Parameters.AddWithValue("@Amount", receiptVoucher.Amount);
                cmd.Parameters.AddWithValue("@NextDueDate", receiptVoucher.Next_DueDate);
                cmd.Parameters.AddWithValue("@remarks", receiptVoucher.Remarks);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                return rowsAffected > 0;
            }
        }
        public List<ReceiptVoucher> GetAllReceiptVoucher()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"Select RV.*,DB.Box_Number As BoxName,DM.Name As DonorName,Cm.Name As CoordinatorName 
                                    from ReceiptVoucher RV
                                    INNER JOIN DonationBox DB ON DB.Id=RV.Sewapatra_No
                                    INNER JOIN Donor_master DM ON Dm.Id=RV.Donor
                                    INNER JOIN Coordinator_master CM ON CM.ID=RV.Coordinator";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<ReceiptVoucher> receiptVouchers = new List<ReceiptVoucher>();
                while (reader.Read())
                {
                    receiptVouchers.Add(new ReceiptVoucher
                    {
                        R_TranId = reader["R_TranId"].ToString(),
                        Date = Convert.ToDateTime(reader["Date"]),
                        Sewapatra_No = Convert.ToInt32(reader["Sewapatra_No"]),
                        DonationBoxName = reader["BoxName"].ToString(),
                        Donor = Convert.ToInt32(reader["Donor"]),
                        DonorName = reader["Donorname"].ToString(),
                        Coordinator = Convert.ToInt32(reader["Coordinator"]),
                        CoordinatorName = reader["CoordinatorName"].ToString(),
                        Amount = reader["Amount"].ToString(),
                        Next_DueDate = Convert.ToDateTime(reader["Next_DueDate"]),
                        Remarks = reader["Remarks"].ToString()  
                    });
                }
                conn.Close();
                return receiptVouchers;
            }
        }
        public ReceiptVoucher GetAllReceiptVoucherById(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM ReceiptVoucher WHERE R_TranId = @R_TranId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@R_TranId", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ReceiptVoucher receiptVoucher = new ReceiptVoucher();
                if (reader.Read())
                {
                    receiptVoucher.R_TranId = reader["R_TranId"].ToString();
                    receiptVoucher.Date = Convert.ToDateTime(reader["Date"]);
                    receiptVoucher.Sewapatra_No = Convert.ToInt32(reader["Sewapatra_No"]);
                    receiptVoucher.Donor = Convert.ToInt32(reader["Donor"]);
                    receiptVoucher.Coordinator = Convert.ToInt32(reader["Coordinator"]);
                    receiptVoucher.Amount = reader["Amount"].ToString();
                    receiptVoucher.Next_DueDate = Convert.ToDateTime(reader["Next_DueDate"]);
                    receiptVoucher.Remarks = reader["Remarks"].ToString();
                }
                conn.Close();
                return receiptVoucher;
            }
        }
        public bool UpdateReceiptVoucher(ReceiptVoucher receiptVoucher)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE ReceiptVoucher SET Date = @Date, SewaPatra_No = @Box_No,Donor = @Donor, Coordinator = @Coordinator, Amount = @Amount,Next_DueDate = @NextDueDate, Remarks = @Remarks 
                                WHERE R_TranId = @R_TranId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@R_TranId", receiptVoucher.R_TranId);
                cmd.Parameters.AddWithValue("@Date", receiptVoucher.Date);
                cmd.Parameters.AddWithValue("@Box_No", receiptVoucher.Sewapatra_No);
                cmd.Parameters.AddWithValue("@Donor", receiptVoucher.Donor);
                cmd.Parameters.AddWithValue("@Coordinator", receiptVoucher.Coordinator);
                cmd.Parameters.AddWithValue("@Amount", receiptVoucher.Amount);
                cmd.Parameters.AddWithValue("@NextDueDate", receiptVoucher.Next_DueDate);
                cmd.Parameters.AddWithValue("@Remarks", receiptVoucher.Remarks);
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
                string sql = "SELECT TOP 1 R_TranId FROM ReceiptVoucher ORDER BY R_TranId DESC";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            transactionId = reader.IsDBNull(0) ? string.Empty : reader["R_TranId"].ToString();
                        }
                    }
                }
            }

            return transactionId;
        }
    }
}
