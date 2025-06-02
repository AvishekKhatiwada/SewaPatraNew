namespace SewaPatra.Models.ReportModels
{
    public class CollectionAnalysisReport
    {
        // Receipt Voucher Information
        public string ReceiptVoucherNo { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string SewaPatraNo { get; set; }
        public decimal CollectionAmount { get; set; }
        public DateTime? NextDueDate { get; set; }
        public string ReceiptRemarks { get; set; }


        // Donor Information
        public int DonorID { get; set; }
        public string DonorName { get; set; }
        public string DonorMobile { get; set; }
        public string DonorAddress { get; set; }
        public string DonorCity { get; set; }
        public int? DonorAreaID { get; set; }
        public string DonorArea { get; set; }
        public string AreaType { get; set; }


        // Coordinator Information
        public int CoordinatorID { get; set; }
        public string CoordinatorName { get; set; }
        public string CoordinatorMobile { get; set; }
        public int? CoordinatorArea { get; set; }


        // SewaPatra Issue Information
        public string SewaPatraIssueNo { get; set; }
        public DateTime IssueEntryDate { get; set; }
        public DateTime IssueDate { get; set; }
        public string RecurringType { get; set; }
        public DateTime OriginalDueDate { get; set; }
        public int? DonationBoxID { get; set; }
        public string DonationBoxNo { get; set; }


        // Payment Voucher Information
        public string PaymentVoucherNo { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? DepositedAmount { get; set; }
        public string PaymentRemarks { get; set; }


        // Calculated Fields
        public int DaysBetweenIssueAndCollection { get; set; }


        // Status Information
        public string PaymentStatus { get; set; }
    }
}
