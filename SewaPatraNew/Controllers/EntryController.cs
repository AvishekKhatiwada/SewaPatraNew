using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SewaPatra.BusinessLayer;
using SewaPatra.Helpers;
using SewaPatra.Models;

namespace SewaPatra.Controllers
{
    public class EntryController : BaseController
    {
        private readonly SewaPatraIssueService _sewaPatraIssueService;
        private readonly PaymentVoucherService _paymentVoucherService;
        private readonly ReceiptVoucherService _receiptVoucherService;
        private readonly SewaPatraReceiptService _sewaPatraReceiptService;
        private readonly DropDownService _dropDownService;
        private readonly DonorService _donorService;

        [ActivatorUtilitiesConstructor]
        public EntryController(SewaPatraIssueService sewaPatraIssueService, PaymentVoucherService paymentVoucherService,
            ReceiptVoucherService receiptVoucherService, DropDownService dropDownService, SewaPatraReceiptService sewaPatraReceiptService,
            DonorService donorService)
        {
            _sewaPatraIssueService = sewaPatraIssueService;
            _paymentVoucherService = paymentVoucherService;
            _receiptVoucherService = receiptVoucherService;
            _dropDownService = dropDownService;
            _sewaPatraReceiptService = sewaPatraReceiptService;
            _donorService = donorService;
        }
        public string GetVoucherNumber(string voucherName="")
        {
            string lastTransactionId = "";
            string newTransactionId = "";
            if (voucherName == "SewaPatraIssue") 
            {
                //Get Last Transaction ID
                lastTransactionId = _sewaPatraIssueService.GetNewTransactionId();
                int nextId = 1;
                // Generate a new transaction ID for SewaPatraIssue
                if (!string.IsNullOrEmpty(lastTransactionId))
                {
                    string numericPart = lastTransactionId.Substring(4); // Remove "SPI-"
                    if (int.TryParse(numericPart, out int lastId))
                    {
                        nextId = lastId + 1;
                    }
                }
                newTransactionId = $"SPI-{nextId}";
            }else if (voucherName == "ReceiptVoucher")
            {
                //Get Last Transaction ID
                lastTransactionId = _receiptVoucherService.GetNewVoucherId();
                int nextId = 001;
                // Generate a new Voucher number for Receipt Voucher
                if (!string.IsNullOrEmpty(lastTransactionId))
                {
                    string numericPart = lastTransactionId.Substring(3); // Remove "RV-"
                    if (int.TryParse(numericPart, out int lastId))
                    {
                        nextId = lastId + 1;
                    }
                }
                newTransactionId = $"RV-{nextId}";
            }else if (voucherName == "SewaPatraReceipt")
            {
                //Get Last Transaction ID
                lastTransactionId = _sewaPatraReceiptService.GetNewVoucherId();
                int nextId = 1;
                // Generate a new Voucher number for SewaPatraReceipt
                if (!string.IsNullOrEmpty(lastTransactionId))
                {
                    string numericPart = lastTransactionId.Substring(3); // Remove "SPR-"
                    if (int.TryParse(numericPart, out int lastId))
                    {
                        nextId = lastId + 1;
                    }
                }
                newTransactionId = $"SPR-{nextId}";
            }
            return newTransactionId;
        }
        #region SewaPatra Issue
        public IActionResult SewaPatraIssue()
        {
            ViewBag.TransactionId = GetVoucherNumber("SewaPatraIssue");
            ViewBag.DonationBoxes = _dropDownService.GetDonationBoxList();
            ViewBag.Coordinators = _dropDownService.GetCoordinatorList();
            ViewBag.Donors = _dropDownService.GetDonorList();
            ViewBag.Message = TempData["Message"];
            return View();
        }
        public IActionResult SewaPatraIssueList()
        {
            List<SewaPatraIssue> sewaPatraIssue = _sewaPatraIssueService.GetAllSewaPatraIssue();
            ViewBag.Message = TempData["Message"];
            return View(sewaPatraIssue);
        }
        [HttpPost]
        public IActionResult SewaPatraIssue(SewaPatraIssue sewaPatraIssue)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isInserted = _sewaPatraIssueService.InsertSewaPatraIssue(sewaPatraIssue);
                    if (isInserted)
                    {
                        TempData["Message"] = "SewaPatra Issue Added Successfully";
                    }
                    else
                    {
                        TempData["Message"] = "Failed to Add SewaPatra Issue";
                    }
                }
                else
                {
                    TempData["Message"] = "Invalid data";
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message.ToString();
            }
            return RedirectToAction("SewaPatraIssue");
        }
        public IActionResult EditSewaPatraIssue(string id)
        {
            var sewaPatraIssue = _sewaPatraIssueService.GetSewaPatraIssueById(id);
            ViewBag.DonationBoxes = _dropDownService.GetDonationBoxList();
            ViewBag.Coordinators = _dropDownService.GetCoordinatorList();
            ViewBag.Donors = _dropDownService.GetDonorList();
            if (sewaPatraIssue == null)
            {
                return NotFound();
            }
            return View(sewaPatraIssue);
        }
        [HttpPost]
        public IActionResult EditSewaPatraIssue(SewaPatraIssue sewaPatraIssue)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isUpdated = _sewaPatraIssueService.UpdateSewaPatraIssue(sewaPatraIssue);
                    if (isUpdated)
                    {
                        TempData["Message"] = "SewaPatra Issue Updated Successfully";
                    }
                    else
                    {
                        TempData["Message"] = "Failed to Update SewaPatra Issue";
                    }
                }
                else
                {
                    TempData["Message"] = "Invalid data";
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message.ToString();
            }            
            return RedirectToAction("SewaPatraIssueList");
        }
        public IActionResult DeleteSewaPatraIssue(string id)
        {
            try 
            {
                bool isDeleted = _sewaPatraIssueService.DeleteSewaPatraIssue(id);
                if (isDeleted)
                {
                    TempData["Message"] = "SewaPatra Issue Deleted Successfully!";
                }
                else 
                {
                    TempData["Message"] = "Failed To Delete SewaPatra Issue!";
                }
            }      
            catch(Exception Ex)
            {
                TempData["Message"] = Ex.Message.ToString();
            }
            return RedirectToAction("SewaPatraIssueList");
        } 
        #endregion
        #region Receipt Voucher
        public IActionResult ReceiptVoucher()
        {
            ViewBag.TransactionId = GetVoucherNumber("ReceiptVoucher");
            ViewBag.DonationBoxes = _dropDownService.GetDonationBoxList();
            ViewBag.Coordinators = _dropDownService.GetCoordinatorList();
            ViewBag.SewaPatraIssues = _dropDownService.GetSewaPatraIssueLists();
            ViewBag.Donors = _dropDownService.GetDonorList();
            ViewBag.Message = TempData["Message"];
            return View();
        }
        [HttpPost]
        public IActionResult ReceiptVoucher(ReceiptVoucher receiptVoucher)
        {
            if (ModelState.IsValid)
            {
                bool isInserted = _receiptVoucherService.InsertReceiptVoucher(receiptVoucher);
                if (isInserted)
                {
                    TempData["Message"] = "Receipt Voucher Inserted Successfully";
                }
                else
                {
                    TempData["Message"] = "Failed to Insert Receipt Voucher";
                }
            }
            else
            {
                TempData["Message"] = "Invalid data";
            }
            return RedirectToAction("ReceiptVoucher");
        }
        public IActionResult EditReceiptVoucher(string id)
        {
            var receiptVoucher = _receiptVoucherService.GetAllReceiptVoucherById(id);
            ViewBag.DonationBoxes = _dropDownService.GetDonationBoxList();
            ViewBag.Coordinators = _dropDownService.GetCoordinatorList();
            ViewBag.Donors = _dropDownService.GetDonorList();
            if (receiptVoucher == null)
            {
                return NotFound();
            }
            return View(receiptVoucher);
        }
        [HttpPost]
        public IActionResult EditReceiptVoucher(ReceiptVoucher receiptVoucher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isUpdated = _receiptVoucherService.UpdateReceiptVoucher(receiptVoucher);
                    if (isUpdated)
                    {
                        TempData["Message"] = "Receipt Voucher Updated Successfully";
                    }
                    else
                    {
                        TempData["Message"] = "Failed to Update Receipt Voucher";
                    }
                }
                else
                {
                    TempData["Message"] = "Invalid data";
                }
            }
            catch (Exception Ex)
            {
                TempData["Message"] = Ex.Message.ToString();
            }
            return RedirectToAction("ReceiptVoucherList");
        }
        public IActionResult ReceiptVoucherList()
        {
            ViewBag.Message = TempData["Message"];
            List<ReceiptVoucher> receiptVoucher = _receiptVoucherService.GetAllReceiptVoucher();
            return View(receiptVoucher);
        }
        #endregion
        #region Payment Voucher
        public IActionResult PaymentVoucher()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Coordinators = _dropDownService.GetCoordinatorList();
            return View();
        }
        public IActionResult PaymentVoucherList()
        {
            ViewBag.Message = TempData["Message"];
            List<PaymentVoucher> paymentVoucher = _paymentVoucherService.GetAllPaymentVoucher();
            return View(paymentVoucher);
        }
        [HttpPost]

        public IActionResult PaymentVoucher(PaymentVoucher paymentVoucher)
        {
            if (ModelState.IsValid)
            {
                bool isInserted = _paymentVoucherService.InsertPaymentVoucher(paymentVoucher);
                if (isInserted)
                {
                    TempData["Message"] = "Payment Voucher Added Successfully";
                }
                else
                {
                    TempData["Message"] = "Failed to Add Payment Voucher Issue";
                }
            }
            else
            {
                TempData["Message"] = "Invalid data";
            }
            return RedirectToAction("PaymentVoucher");
        }
        public IActionResult EditPaymentVoucher(string id)
        {
            var paymentVoucher = _paymentVoucherService.GetAllPaymentVoucherById(id);
            ViewBag.Coordinators = _dropDownService.GetCoordinatorList();
            if (paymentVoucher == null)
            {
                return NotFound();
            }
            return View(paymentVoucher);
        }
        [HttpPost]
        public IActionResult EditPaymentVoucher(PaymentVoucher paymentVoucher)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = _paymentVoucherService.UpdatePaymentVoucher(paymentVoucher);
                if (isUpdated)
                {
                    TempData["Message"] = "Payment Voucher Updated Successfully";
                }
                else
                {
                    TempData["Message"] = "Failed to Update Payment Voucher";
                }
            }
            else
            {
                TempData["Message"] = "Invalid data";
            }
            return View();
        }
        public IActionResult DeletePaymentVoucher(string id)
        {
            bool isDeleted = _paymentVoucherService.DeletePaymentVoucher(id);
            return RedirectToAction("PaymentVoucherList");
        }
        #endregion
        #region SewaPatra Receipt
        public IActionResult SewaPatraReceipt()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.DonationBoxes = _dropDownService.GetDonationBoxList();
            ViewBag.Coordinators = _dropDownService.GetCoordinatorList();
            ViewBag.TransactionId = GetVoucherNumber("SewaPatraReceipt");
            ViewBag.SewaPatraIssues = _dropDownService.GetSewaPatraIssueLists();
            ViewBag.Donors = _dropDownService.GetDonorList();
            return View();
        }
        public IActionResult SewaPatraReceiptList()
        {
            ViewBag.Message = TempData["Message"];
            List<SewaPatraReceipt> sewaPatraReceipt = _sewaPatraReceiptService.GetAllSewaPatraReceipt();
            return View(sewaPatraReceipt);
        }
        [HttpPost]
        public IActionResult SewaPatraReceipt(SewaPatraReceipt sewaPatraReceipt)
        {
            if (ModelState.IsValid)
            {
                bool isInserted = _sewaPatraReceiptService.InsertSewaPatraReceipt(sewaPatraReceipt);
                if (isInserted)
                {
                    TempData["Message"] = "SewaPatra Receipt Added Successfully";
                }
                else
                {
                    TempData["Message"] = "Failed to Add SewaPatra Receipt";
                }
            }
            else
            {
                TempData["Message"] = "Invalid data";
            }
            return RedirectToAction("SewaPatraReceipt");
        }
        public IActionResult EditSewaPatraReceipt(string id)
        {
            var sewaPatraReceipt = _sewaPatraReceiptService.GetSewaPatraReceiptById(id);
            ViewBag.DonationBoxes = _dropDownService.GetDonationBoxList();
            ViewBag.Coordinators = _dropDownService.GetCoordinatorList();
            ViewBag.SewaPatraIssues = _dropDownService.GetSewaPatraIssueLists();
            ViewBag.Donors = _dropDownService.GetDonorList();
            if (sewaPatraReceipt == null)
            {
                return NotFound();
            }
            return View(sewaPatraReceipt);
        }
        [HttpPost]
        public IActionResult EditSewaPatraReceipt(SewaPatraReceipt sewaPatraReceipt)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = _sewaPatraReceiptService.UpdateSewaPatraReceipt(sewaPatraReceipt);
                if (isUpdated)
                {
                    TempData["Message"] = "SewaPatra Receipt Updated Successfully";
                }
                else
                {
                    TempData["Message"] = "Failed to Update SewaPatra Receipt";
                }
            }
            else
            {
                TempData["Message"] = "Invalid data";
            }
            return View();
        }
        public IActionResult DeleteSewaPatraReceipt(string id)
        {
            bool isDeleted = _sewaPatraReceiptService.DeleteSewaPatraReceipt(id);
            return RedirectToAction("SewaPatraReceiptList");
        }
        #endregion
        #region Other
        public IActionResult GetDonorData(int id)
        {
            var donor = _donorService.GetDonorById(id);
            return Ok(donor);
        }
        public IActionResult GetsewaPatraIssueDetails(string id)
        {
            var sewaPatraIssue = _sewaPatraIssueService.PopulateSewaPatraIssue(id);
            return Ok(sewaPatraIssue);
        }
        #endregion

    }
}