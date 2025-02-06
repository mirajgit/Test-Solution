using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.NETCore;
using System.Drawing.Printing;
using Test.Entities;
using Test.UI.Models;
using Test.UI.Services.Interface;
namespace Test.UI.Controllers
{
    public class AutoPrintController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SMSanagement_DBContext _Context;
        private readonly IWebHostEnvironment _environment;
        private readonly INotyfService _notyf;
        private readonly IPrinterRepository _iPrinterRepository;
        public AutoPrintController(ILogger<HomeController> logger, SMSanagement_DBContext Context,IWebHostEnvironment environment, INotyfService notyf, IPrinterRepository iPrinterRepository)
        {
            _logger = logger;
            _Context = Context;
            _environment = environment;
            _notyf = notyf;
            _iPrinterRepository = iPrinterRepository;
        }
        public IActionResult Pdf()
        {
            return View();
        }
        public IActionResult SelectPrinter()
        {
            var AccessUser = "DHK00001";
            var CurrentUserData= _iPrinterRepository.GetCNSelectedPrinter(AccessUser);
            PrinterSelectViewModel model = new PrinterSelectViewModel();
            model.PrinterList = PrinterSettings.InstalledPrinters.Cast<string>().Select(x => new SelectListItem{Text = x,Value = x }).ToList();
            model.PrinterList.Insert(0, new SelectListItem { Text = "Select Printer", Value = "" });
            model.CNSelectedPrinter = CurrentUserData?.CNPrinter;
            model.BarcodeSelectedPrinter = CurrentUserData?.BarcodePrinter;
            model.PrinterId = CurrentUserData?.PrinterId;
            model.HV = CurrentUserData?.HV;
            return View(model);
        }
        [HttpPost]
        public IActionResult SaveCNSelectedPrinter(PrinterSelectViewModel model)
        {
            _iPrinterRepository.AddPrinter(model);
            return RedirectToAction("SelectPrinter","AutoPrint");
        }
        [HttpPost]
        public IActionResult SaveBarcodeSelectedPrinter(PrinterSelectViewModel model)
        {
            _iPrinterRepository.AddPrinter(model);
            return RedirectToAction("SelectPrinter", "AutoPrint");
        }
        public void PrintPDF(byte[] pdfBytes,string CNPrinter)
        {
            using (MemoryStream ms = new MemoryStream(pdfBytes))
            {
                // Load the PDF using PdfiumViewer
                using (var pdfDocument = PdfiumViewer.PdfDocument.Load(ms))
                {
                    PrintDocument printDoc = new PrintDocument();
                    // Retrieve the printer name from session
                    if (!string.IsNullOrEmpty(CNPrinter)) printDoc.PrinterSettings.PrinterName = CNPrinter;
                    int currentPage = 0;
                    printDoc.PrintPage += (sender, e) =>
                    {
                        if (currentPage < pdfDocument.PageCount)
                        {
                            // Get printable area for scaling
                            var printableArea = e.PageSettings.PrintableArea;
                            // Render the page to an image with high DPI 300 DPI
                            // if you want to use more high resulation to use 600 DPI
                            const int dpi = 300;
                            int width = (int)(printableArea.Width / 100 * dpi);
                            int height = (int)(printableArea.Height / 100 * dpi);
                            var image = pdfDocument.Render(currentPage, width, height, dpi, dpi, true);
                            // Draw the image on the print page, scaled to fit
                            e.Graphics.DrawImage(image, printableArea);
                            currentPage++;
                            e.HasMorePages = currentPage < pdfDocument.PageCount;
                        }
                        else
                        {
                            e.HasMorePages = false;
                        }
                    };
                    try
                    {
                        printDoc.Print();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during printing: {ex.Message}");
                    }
                }
            }
        }
        [HttpPost]
        public IActionResult PrintReport()
        {
            var AccessUser = "DHK00001";
            var CNPrinter = _iPrinterRepository.GetCNSelectedPrinter(AccessUser).CNPrinter;
            if (string.IsNullOrEmpty(CNPrinter))
            {
                _notyf.Warning("Please select a printer from the Printer Select Option.", 4);
                return View("Pdf");
            }
            byte[] reportBytes = PdfGenerate();
            PrintPDF(reportBytes, CNPrinter);
            return RedirectToAction("Pdf", "AutoPrint");
        }
        private byte[] PdfGenerate()
        {
            string reportPath = Path.Combine(_environment.WebRootPath, "Reports", "UserInformation.rdlc");
            if (!System.IO.File.Exists(reportPath)) throw new FileNotFoundException($"RDLC file not found at {reportPath}");
            LocalReport localReport = new LocalReport { ReportPath = reportPath };
            var reportData = _Context.Tbl_HRM_UserInformation.Select(x => new Tbl_HRM_UserInformation
            {
                UserId = x.UserId,
                LoginName = x.LoginName,
                UserName = x.UserName,
                UserEid = x.UserEid,
                Password = x.Password,
                Salary = x.Salary,
                Status = x.Status
            }).ToList();
            var expriencesDataSource = new ReportDataSource { Name = "DataSet1" };
            localReport.DataSources.Add(expriencesDataSource);
            expriencesDataSource.Value = reportData;
            localReport.ReportPath = reportPath;
            string mimeType, encoding, fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes = localReport.Render("PDF",null,out mimeType,out encoding,out fileNameExtension,out streams,out warnings);
            return renderedBytes;
        }
        public IActionResult PdfGenerates()
        {
            // Path to your RDLC file
            string reportPath = Path.Combine(_environment.WebRootPath, "Reports", "UserInformation.rdlc");

            // Ensure the report file exists
            if (!System.IO.File.Exists(reportPath))
                return NotFound($"RDLC file not found at {reportPath}");

            // Initialize LocalReport
            LocalReport localReport = new LocalReport { ReportPath = reportPath };

            // Get data from the database
            var reportData = _Context.Tbl_HRM_UserInformation.Select(x => new Tbl_HRM_UserInformation
            {
                UserId = x.UserId,
                LoginName = x.LoginName,
                UserName = x.UserName,
                UserEid = x.UserEid,
                Password = x.Password,
                Salary = x.Salary,
                Status = x.Status
            }).ToList();
            var experiencesDataSource = new ReportDataSource("DataSet1", reportData);
            localReport.DataSources.Add(experiencesDataSource);
            string mimeType, encoding, fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes = localReport.Render(
                "PDF",
                null,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings
            );
            string reportName = "Bangladesh";
            Response.Headers.Append("Content-Disposition", $"inline; filename={reportName}.pdf"); 
            return File(renderedBytes, "application/pdf");
        }
    }
}
