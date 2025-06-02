namespace SewaPatra.Models.ReportModels
{
    public class SewaPatraReceiptRegister
    {
        public string TranId { get; set; }
        public string Date { get; set; }
        public int DonorId { get; set; } // Donor ID
        public string DonorName { get; set; } // Donor Name
        public string DonorArea { get; set; }
        public int CoordinatorId { get; set; } // Coordinator ID
        public string CoordinatorName { get; set; }
        public string CoordinatorArea { get; set; }
        public string BoxId { get; set; } // DonationBox ID
        public string DonationBox { get; set; } // DonationBox Number
        public string Receive_Date { get; set; }
        public string NextDueDate { get; set; }
        public string Remarks { get; set; }
    }
}
