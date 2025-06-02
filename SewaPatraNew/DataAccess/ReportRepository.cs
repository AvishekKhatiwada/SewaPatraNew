using Microsoft.Data.SqlClient;
using SewaPatra.Models;
using SewaPatra.Models.ReportModels;
using SewaPatra.Models.ReportModels;

namespace SewaPatra.DataAccess
{
    public class ReportRepository
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReportRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _httpContextAccessor = httpContextAccessor;
        }
        public List<SewaPatraIssueRegister> GetSewaPatraIssueRegister(DateTime fromDate, DateTime toDate)
        {
            List<SewaPatraIssueRegister> sewaPatraIssueRegister = new List<SewaPatraIssueRegister>(); 
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT TranId,Entered_Date As [Date], SPI.Donor As DonorId,DM.Name As DonorName,DMA.Area_name As DonorArea, SPI.Coordinator As CoordinatorId,CM.Name As CoordinatorName,CMA.Area_name As CoordinatorArea, 
                                    DonationBox As BoxId,Db.Box_Number As DonationBox,Issue_Date As IssueDate, Recurring, Due_Date As DueDate, Remarks 
                                    FROM SewaPatraIssue SPI
                                    INNER JOIN Donor_master DM ON DM.Id=SPI.Donor
                                    INNER Join Area_Master DMA ON DM.Area=DMA.Id
                                    INNER JOIN Coordinator_master CM ON CM.Id=SPI.Coordinator
                                    INNER JOIN Area_Master CMA ON CM.Area_Under=CMA.Id 
                                    INNER JOIN DonationBox DB ON Db.Id=SPI.DonationBox
                                    WHERE  Entered_Date BETWEEN @FromDate AND @ToDate";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sewaPatraIssueRegister.Add(new SewaPatraIssueRegister
                        {
                            TranId = reader["TranId"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]).ToString("dd/MM/yyyy"),
                            DonorId = Convert.ToInt32(reader["DonorId"]),
                            DonorName = reader["DonorName"].ToString(),
                            DonorArea = reader["DonorArea"].ToString(),
                            CoordinatorId = Convert.ToInt32(reader["CoordinatorId"]),
                            CoordinatorName = reader["CoordinatorName"].ToString(),
                            CoordinatorArea = reader["CoordinatorArea"].ToString(),
                            BoxId = reader["BoxId"].ToString(),
                            DonationBox = reader["DonationBox"].ToString(),
                            IssueDate = Convert.ToDateTime(reader["IssueDate"]).ToString("dd/MM/yyyy"),
                            Recurring = reader["Recurring"].ToString(),
                            DueDate = Convert.ToDateTime(reader["DueDate"]).ToString("dd/MM/yyyy"),
                            Remarks = reader["Remarks"].ToString()
                        });
                    }
                }
            }
            return sewaPatraIssueRegister;
        }
        public List<SewaPatraReceiptRegister> GetSewaPatraReceiptRegister(DateTime fromDate, DateTime toDate)
        {
            List<SewaPatraReceiptRegister> sewaPatraReceiptRegister = new List<SewaPatraReceiptRegister>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT SPR_TranId As TranId,Date As [Date], SPR.Donor As DonorId,DM.Name As DonorName,DMA.Area_name As DonorArea, SPR.Coordinator As CoordinatorId
                                    ,CM.Name As CoordinatorName,CMA.Area_name As CoordinatorArea,DonationBox As BoxId,Db.Box_Number As DonationBox,Receive_Date As ReceiveDate, Remarks 
                                    FROM SewaPatraReceipt SPR
                                    INNER JOIN Donor_master DM ON DM.Id=SPR.Donor
                                    INNER Join Area_Master DMA ON DM.Area=DMA.Id
                                    INNER JOIN Coordinator_master CM ON CM.Id=SPR.Coordinator
                                    INNER JOIN Area_Master CMA ON CM.Area_Under=CMA.Id 
                                    INNER JOIN DonationBox DB ON Db.Id=SPR.DonationBox
                                    WHERE 1=1 AND Date BETWEEN @fromDate AND @toDate ORDER BY [Date]";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sewaPatraReceiptRegister.Add(new SewaPatraReceiptRegister
                        {
                            TranId = reader["TranId"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]).ToString("dd/MM/yyyy"),
                            DonorId = Convert.ToInt32(reader["DonorId"]),
                            DonorName = reader["DonorName"].ToString(),
                            DonorArea = reader["DonorArea"].ToString(),
                            CoordinatorId = Convert.ToInt32(reader["CoordinatorId"]),
                            CoordinatorName = reader["CoordinatorName"].ToString(),
                            CoordinatorArea = reader["CoordinatorArea"].ToString(),
                            BoxId = reader["BoxId"].ToString(),
                            DonationBox = reader["DonationBox"].ToString(),
                            Receive_Date = Convert.ToDateTime(reader["ReceiveDate"]).ToString("dd/MM/yyyy"),
                            Remarks = reader["Remarks"].ToString()
                        });
                    }
                }
            }
            return sewaPatraReceiptRegister;
        }
        public List<SewaPatraDueReport> GetSewaPatraDueReport()
        {
            List<SewaPatraDueReport> sewaPatraDueReport = new List<SewaPatraDueReport>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"Select ROW_NUMBER() OVER (ORDER BY SPI.Issue_Date) AS SNo,Db.Box_Number,SPI.Issue_Date As IssueDate,SPI.Due_Date As DueDate,DM.Name As Donor,
                                AM.Area_name As DonorArea,CM.Name As Coordinator
                                FROM SewaPatraIssue SPI
                                INNER JOIN DonationBox DB ON Db.Id=SPI.DonationBox
                                INNER JOIN Donor_master DM ON Dm.Id=SPI.Donor
                                INNER JOIN Coordinator_master CM ON CM.ID=SPI.Coordinator
                                INNER JOIN Area_Master AM ON Am.ID=Dm.Area
                                ORDER BY Due_Date DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sewaPatraDueReport.Add(new SewaPatraDueReport
                        {
                            SNo = reader["SNo"].ToString(),
                            DonationBox = reader["Box_Number"].ToString(),
                            IssueDate = Convert.ToDateTime(reader["IssueDate"]).ToString("dd/MM/yyyy"),
                            DonorName = reader["Donor"].ToString(),
                            DonorArea = reader["DonorArea"].ToString(),
                            DueDate = Convert.ToDateTime(reader["DueDate"]).ToString("dd/MM/yyyy"),
                            Coordinator = reader["Coordinator"].ToString()
                        });
                    }
                }
            }
            return sewaPatraDueReport;
        }
        public List<CoordinatorListingReport> GetCoordinatorListingReport()
        {
            List<CoordinatorListingReport> coordinatorListingReport = new List<CoordinatorListingReport>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"Select ROW_NUMBER() OVER (ORDER BY CM.id) AS SNo,CM.id as [Coordinator ID],Name as [Name],Mobile_No As [Contact],
                                    Alternate_Mobile_No As AltContact,Address AS [Address],City As [City],Email,
                                    CASE WHEN Cm.Active=1 THEN 'Active' ELSE 'InActive' END As Status,
                                    Area_Under As [Area Code],Am.Area_name As [Area],AM.Area_type As [AreaType],AM.Under AreaUnder
                                    From Coordinator_master CM
                                    Inner Join Area_Master AM On AM.Id=CM.Area_Under
                                    Where 1=1 ORDER BY CM.Name ASC";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        coordinatorListingReport.Add(new CoordinatorListingReport
                        {
                            SNo = reader["SNo"].ToString(),
                            CoordinatorId = Convert.ToInt32(reader["Coordinator ID"]),
                            Name = reader["Name"].ToString(),
                            Contact = reader["Contact"].ToString(),
                            AltContact = reader["AltContact"].ToString(),
                            Address = reader["Address"].ToString(),
                            City = reader["City"].ToString(),
                            Email = reader["Email"].ToString(),
                            AreaCode = Convert.ToInt32(reader["Area Code"]),
                            Area = reader["Area"].ToString(),
                            AreaType = reader["AreaType"].ToString(),
                            AreaUnder = reader["AreaUnder"].ToString(),
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }
            return coordinatorListingReport;
        }
        public List<DonorListingReport> GetDonorListingReport()
        {
            List<DonorListingReport> donorListingReport = new List<DonorListingReport>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"Select ROW_NUMBER() OVER (ORDER BY DM.id) AS SNo,DM.Id As [Id],DM.Name As [Name],DM.Address As [Address],DM.Mobile_no As [Contact],
                                    AM.Area_name As [Area],DM.Email As [Email],DM.Mobile_no2 as [AltContact],Dm.City As [City],
                                    CASE WHEN DM.Active=1 THEN 'Active' ELSE 'InActive' END As Status,
                                    CM.Name AS [Coordinator Name]
                                    from Donor_master DM 
                                    LEFT OUTER JOIN Area_Master AM On Am.ID=Dm.Area
                                    LEFT OUTER JOIN Coordinator_master CM ON CM.ID=DM.Coordinator";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        donorListingReport.Add(new DonorListingReport
                        {
                            SNo = reader["SNo"].ToString(),
                            DonorId = Convert.ToInt32(reader["Id"]),
                            Contact = reader["Contact"].ToString(),
                            Name = reader["Name"].ToString(),
                            Address = reader["Address"].ToString(),                           
                            City = reader["City"].ToString(),
                            AltContact = reader["AltContact"].ToString(),
                            Email = reader["Email"].ToString(),
                            //AreaCode = Convert.ToInt32(reader["Area Code"]),
                            CoordinatorName = reader["Coordinator Name"].ToString(),
                            Area = reader["Area"].ToString(),
                            Status = reader["Status"].ToString()                           
                        });
                    }
                }
            }
            return donorListingReport;
        }
        public List<SewaPatraListingReport> GetSewaPatraListingReport()
        {
            List<SewaPatraListingReport> sewaPatraListingReport = new List<SewaPatraListingReport>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"Select ROW_NUMBER() OVER (ORDER BY ID) AS Sno,ID BoxID,Box_Number [SewaPatra No],
                                    CASE Active WHEN Active THEN 'Active' ELSE 'InActive' END AS Status FROM DonationBox";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sewaPatraListingReport.Add(new SewaPatraListingReport
                        {
                            SNo = reader.IsDBNull(0) ? string.Empty : reader["SNo"].ToString(),
                            BoxID = reader.IsDBNull(1) ? string.Empty : reader["BoxID"].ToString(),
                            SewaPatraNo = reader.IsDBNull(2) ? string.Empty : reader["SewaPatra No"].ToString(),
                            Status = reader.IsDBNull(3) ? string.Empty : reader["Status"].ToString()
                        });
                    }
                }
            }
            return sewaPatraListingReport;
        }
        public List<PaymentRegisterReport> GetPaymentRegisterReport(DateTime fromDate, DateTime toDate)
        {
            List<PaymentRegisterReport> paymentRegisterReport = new List<PaymentRegisterReport>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"Select ROW_NUMBER() OVER (ORDER BY P_TranId) AS Sno,P_TranId, CONVERT(DATE,[Date],103) As [Date],Ledger_Name As [Bank Name],
                                Amount,Cm.Name AS Coordinator,AM.Area_name Area,Remarks
                                from PaymentVoucher PV
                                Inner Join Coordinator_master CM On CM.Id=PV.Coordinator
                                INNER JOIN Area_Master AM ON AM.ID=CM.Area_Under
                                Where 1=1 AND Date BETWEEN @FromDate AND @ToDate
                                ORDER BY PV.[Date] ASC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        paymentRegisterReport.Add(new PaymentRegisterReport
                        {
                            SNo = reader["SNo"].ToString(),
                            P_TranId = Convert.ToInt32(reader["P_TranId"]),
                            Date = Convert.ToDateTime(reader["Date"]).ToString("dd/MM/yyyy"),
                            BankName = reader["Bank Name"].ToString(),
                            Amount = reader["Amount"].ToString(),
                            Name = reader["Coordinator"].ToString(),
                            AreaName = reader["Area"].ToString(),
                            Remarks = reader["Remarks"].ToString()
                        });
                    }
                }
            }
            return paymentRegisterReport;
        }
        public List<ReceiptRegisterReport> GetReceiptRegisterReport(DateTime fromDate, DateTime toDate)
        {
            List<ReceiptRegisterReport> receiptRegisterReport = new List<ReceiptRegisterReport>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"Select ROW_NUMBER() OVER (ORDER BY R_TranId) As Sno,R_TranId, [Date],db.Box_Number As [Donation Box],DM.Name As [Donor],AM1.Area_name DonorArea,
                                Amount,Cm.Name As [Coordinator],AM.Area_name CoordinatorArea,Next_DueDate As [Next Due],Remarks
                                from ReceiptVoucher RV
                                LEFT OUTER Join Coordinator_master CM On CM.Id=RV.Coordinator
                                LEFT OUTER JOIN Area_Master AM ON AM.ID=CM.Area_Under
                                LEFT OUTER join DonationBox DB ON Db.Id=RV.Sewapatra_No
                                LEFT OUTER Join Donor_master DM ON DM.Id=RV.Donor
                                LEFT OUTER JOIN Area_Master AM1 ON AM1.ID=DM.Area
                                Where 1=1 AND Date BETWEEN @FromDate AND @ToDate
                                ORDER BY RV.[Date] ASC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        receiptRegisterReport.Add(new ReceiptRegisterReport
                        {
                            SNo = reader["SNo"].ToString(),
                            R_TranId = reader["R_TranId"].ToString(),
                            Date = Convert.ToDateTime(reader["Date"]).ToString("dd/MM/yyyy"),
                            DonationBox = reader["Donation Box"].ToString(),
                            Donor = reader["Donor"].ToString(),
                            DonorArea = reader["DonorArea"].ToString(),
                            Amount = reader["Amount"].ToString(),
                            Coordinator = reader["Coordinator"].ToString(),
                            CoordinatorArea = reader["CoordinatorArea"].ToString(),
                            NextDue = Convert.ToDateTime(reader["Next Due"]).ToString("dd/MM/yyyy"),
                            Remarks = reader["Remarks"].ToString()
                        });
                    }
                }
            }
            return receiptRegisterReport;
        }
        public List<CollectionAnalysisReport> GetCollectionAnalysisReports(DateTime fromDate, DateTime toDate)
        {
            List<CollectionAnalysisReport> collectionAnalysisReports = new List<CollectionAnalysisReport>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT -- Receipt Voucher Information
                                RV.R_TranId AS [Receipt Voucher No],
                                RV.Date AS [Receipt Date],
                                RV.Sewapatra_No AS [SewaPatra No],
                                RV.Amount AS [Collection Amount],
                                RV.Next_DueDate AS [Next Due Date],
                                RV.Remarks AS [Receipt Remarks],
    
                                -- Donor Information
                                DM.Id AS [Donor ID],
                                DM.Name AS [Donor Name],
                                DM.Mobile_no AS [Donor Mobile],
                                DM.Address AS [Donor Address],
                                DM.City AS [Donor City],
                                DM.Area AS [Donor Area ID],
                                AM.Area_name AS [Donor Area],
                                AM.Area_type AS [Area Type],
    
                                -- Coordinator Information
                                CM.ID AS [Coordinator ID],
                                CM.Name AS [Coordinator Name],
                                CM.Mobile_No AS [Coordinator Mobile],
                                CM.Area_Under AS [Coordinator Area],
    
                                -- SewaPatra Issue Information
                                SPI.TranId AS [SewaPatra Issue No],
                                SPI.Entered_Date AS [Issue Entry Date],
                                SPI.Issue_Date AS [Issue Date],
                                SPI.Recurring AS [Recurring Type],
                                SPI.Due_Date AS [Original Due Date],
                                DB.ID AS [Donation Box ID],
                                DB.Box_Number AS [Donation Box No],
    
                                -- Payment Voucher Information (if exists)
                                PV.P_TranId AS [Payment Voucher No],
                                PV.Date AS [Payment Date],
                                PV.Amount AS [Deposited Amount],
                                PV.Remarks AS [Payment Remarks],
    
                                -- Calculated Fields
                                DATEDIFF(DAY, SPI.Issue_Date, RV.Date) AS [Days Between Issue and Collection],
    
                                -- Status Information
                                CASE 
                                    WHEN RV.Next_DueDate IS NULL THEN 'Final Payment'
                                    WHEN CONVERT(date, RV.Next_DueDate) < CONVERT(date, GETDATE()) THEN 'Overdue'
                                    ELSE 'Active'
                                END AS [Payment Status]    
                            FROM ReceiptVoucher RV
                            INNER JOIN SewaPatraIssue SPI ON RV.SPI_Id = SPI.TranId
                            INNER JOIN Donor_master DM ON RV.Donor = DM.Id
                            INNER JOIN Coordinator_master CM ON RV.Coordinator = CM.ID
                            LEFT JOIN Area_Master AM ON DM.Area = AM.ID
                            LEFT JOIN DonationBox DB ON SPI.DonationBox = DB.ID
                            LEFT JOIN PaymentVoucher PV ON RV.R_TranId = PV.P_TranId OR 
                            (PV.Coordinator = RV.Coordinator AND PV.Date = RV.Date)
                            WHERE RV.Date BETWEEN @FromDate AND @ToDate
                            ORDER BY RV.Date DESC, CM.Name, DM.Name";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        collectionAnalysisReports.Add(new CollectionAnalysisReport
                        {
                            // Receipt Voucher Information
                            ReceiptVoucherNo = reader["Receipt Voucher No"].ToString(),
                            ReceiptDate = Convert.ToDateTime(reader["Receipt Date"]),
                            SewaPatraNo = reader["SewaPatra No"].ToString(),
                            CollectionAmount = Convert.ToDecimal(reader["Collection Amount"]),
                            NextDueDate = reader["Next Due Date"] != DBNull.Value ? Convert.ToDateTime(reader["Next Due Date"]) : (DateTime?)null,
                            ReceiptRemarks = reader["Receipt Remarks"].ToString(),

                            // Donor Information
                            DonorID = Convert.ToInt32(reader["Donor ID"]),
                            DonorName = reader["Donor Name"].ToString(),
                            DonorMobile = reader["Donor Mobile"].ToString(),
                            DonorAddress = reader["Donor Address"].ToString(),
                            DonorCity = reader["Donor City"].ToString(),
                            DonorAreaID = reader["Donor Area ID"] != DBNull.Value ? Convert.ToInt32(reader["Donor Area ID"]) : (int?)null,
                            DonorArea = reader["Donor Area"].ToString(),
                            AreaType = reader["Area Type"].ToString(),

                            // Coordinator Information
                            CoordinatorID = Convert.ToInt32(reader["Coordinator ID"]),
                            CoordinatorName = reader["Coordinator Name"].ToString(),
                            CoordinatorMobile = reader["Coordinator Mobile"].ToString(),
                            CoordinatorArea = reader["Coordinator Area"] != DBNull.Value ? Convert.ToInt32(reader["Coordinator Area"]) : (int?)null,

                            // SewaPatra Issue Information
                            SewaPatraIssueNo = reader["SewaPatra Issue No"].ToString(),
                            IssueEntryDate = Convert.ToDateTime(reader["Issue Entry Date"]),
                            IssueDate = Convert.ToDateTime(reader["Issue Date"]),
                            RecurringType = reader["Recurring Type"].ToString(),
                            OriginalDueDate = Convert.ToDateTime(reader["Original Due Date"]),
                            DonationBoxID = reader["Donation Box ID"] != DBNull.Value ? Convert.ToInt32(reader["Donation Box ID"]) : (int?)null,
                            DonationBoxNo = reader["Donation Box No"].ToString(),

                            // Payment Voucher Information
                            PaymentVoucherNo = reader["Payment Voucher No"] != DBNull.Value ? reader["Payment Voucher No"].ToString() : null,
                            PaymentDate = reader["Payment Date"] != DBNull.Value ? Convert.ToDateTime(reader["Payment Date"]) : (DateTime?)null,
                            DepositedAmount = reader["Deposited Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Deposited Amount"]) : (decimal?)null,
                            PaymentRemarks = reader["Payment Remarks"] != DBNull.Value ? reader["Payment Remarks"].ToString() : null,

                            // Calculated Fields
                            DaysBetweenIssueAndCollection = Convert.ToInt32(reader["Days Between Issue and Collection"]),

                            // Status Information
                            PaymentStatus = reader["Payment Status"].ToString()
                        });
                    }
                }
            }
            return collectionAnalysisReports; 
        }
    }
}