using SewaPatra.DataAccess;
using SewaPatra.Models;
using SewaPatra.Models.ReportModels;


namespace SewaPatra.BusinessLayer
{
    public class ReportService
    {
        private readonly ReportRepository _reportRepository;
        public ReportService(ReportRepository repository)
        {
            _reportRepository = repository;
        }        
        public List<SewaPatraIssueRegister> GetSewaPatraIssueRegister(DateTime fromDate, DateTime toDate)
        {
            return _reportRepository.GetSewaPatraIssueRegister(fromDate,toDate);
        }
        public List<SewaPatraReceiptRegister> GetSewaPatraReceiptRegister(DateTime fromDate, DateTime toDate)
        {
            return _reportRepository.GetSewaPatraReceiptRegister(fromDate, toDate);
        }
        public List<SewaPatraDueReport> GetSewaPatraDueReport()
        {
            return _reportRepository.GetSewaPatraDueReport();
        }

        public List<CoordinatorListingReport> GetCoordinatorListingReport()
        {
            return _reportRepository.GetCoordinatorListingReport(); 
        }

        public List<DonorListingReport> GetDonorListingReport()
        {
            return _reportRepository.GetDonorListingReport();
        }

        public List<SewaPatraListingReport> GetSewaPatraListingReport()
        {
            return _reportRepository.GetSewaPatraListingReport();
        }

        public List<PaymentRegisterReport> GetPaymentRegisterReport(DateTime fromDate, DateTime toDate)
        {
            return _reportRepository.GetPaymentRegisterReport(fromDate, toDate);
        }

        public List<ReceiptRegisterReport> GetReceiptRegisterReport(DateTime fromDate, DateTime toDate)
        {
            return _reportRepository.GetReceiptRegisterReport(fromDate, toDate);
        }

        public List<CollectionAnalysisReport> GetCollectionAnalysisReports(DateTime fromDate, DateTime toDate)
        {
            return _reportRepository.GetCollectionAnalysisReports(fromDate, toDate);
        }
        
    }
}