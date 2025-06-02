using Microsoft.AspNetCore.Mvc;
using SewaPatra.BusinessLayer;
using SewaPatra.Models.ReportModels;

namespace SewaPatra.Controllers
{
    public class ReportController : Controller
    {
        private readonly ReportService _reportService;
        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult SewaPatraIsueRegister()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SewaPatraIsueRegister(DateTime fromDate, DateTime toDate) 
        {
            List<SewaPatraIssueRegister> sewaPatraIssueRegisters = _reportService.GetSewaPatraIssueRegister(fromDate,toDate);
            return View(sewaPatraIssueRegisters);
        }
        public IActionResult SewaPatraReceiptRegister()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SewaPatraReceiptRegister(DateTime fromDate, DateTime toDate)
        {
            List<SewaPatraReceiptRegister> sewaPatraReceiptRegisters = _reportService.GetSewaPatraReceiptRegister(fromDate, toDate);
            return View(sewaPatraReceiptRegisters);
        }
        public IActionResult SewaPatraDueDetails() 
        {
            List<SewaPatraDueReport> sewaPatraDueReports = _reportService.GetSewaPatraDueReport();
            return View(sewaPatraDueReports);
        }

        public IActionResult CoordinatorListing()
        {
            List<CoordinatorListingReport> coordinatorListingReport = _reportService.GetCoordinatorListingReport();
            return View(coordinatorListingReport);
        }

        public IActionResult DonorListing()
        {
            List<DonorListingReport> donorListingReport = _reportService.GetDonorListingReport();
            return View(donorListingReport);
        }

        public IActionResult SewaPatraListing()
        {
            List<SewaPatraListingReport> sewaPatraListingReport = _reportService.GetSewaPatraListingReport();
            return View(sewaPatraListingReport);
        }

        public IActionResult PaymentRegister()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PaymentRegister(DateTime fromDate, DateTime toDate)
        {
            List<PaymentRegisterReport> paymentRegisterReport = _reportService.GetPaymentRegisterReport(fromDate, toDate);
            return View(paymentRegisterReport);
        }
        public IActionResult ReceiptRegister()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ReceiptRegister(DateTime fromDate, DateTime toDate)
        {
            List<ReceiptRegisterReport> receiptRegisterReport = _reportService.GetReceiptRegisterReport(fromDate, toDate);
            return View(receiptRegisterReport);
        }
        public IActionResult CollectionAnalysisReport()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CollectionAnalysisReport(DateTime fromDate, DateTime toDate)
        {
            List<CollectionAnalysisReport> collectionAnalysisReports = _reportService.GetCollectionAnalysisReports(fromDate, toDate);
            return View(collectionAnalysisReports);
        }
    }
}

