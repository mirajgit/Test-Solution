using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using PdfiumViewer;
using System.Diagnostics;
using System.Drawing.Printing;
using Test.Entities;
using Test.UI.Models;
namespace Test.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SMSanagement_DBContext _Context;
        private readonly IWebHostEnvironment _environment;
        public HomeController(ILogger<HomeController> logger, SMSanagement_DBContext Context,IWebHostEnvironment environment)
        {
            _logger = logger;
            _Context = Context;
            _environment = environment;
        }
        public IActionResult Index()
        {
            try
            {
                var serverDate = GetServerDateTime() ; 
                if (!serverDate.HasValue)
                {
                    ModelState.AddModelError("", "Unable to fetch server date.");
                    return BadRequest(ModelState);
                }
                DateTime clientDate = DateTime.Now;
                 
                TimeSpan tolerance = TimeSpan.FromMinutes(5);
                if (Math.Abs((serverDate.Value - clientDate).TotalMinutes) > tolerance.TotalMinutes)
                {
                    ModelState.AddModelError("", "Your system date and time do not match the server's date and time."); 
                } 
                SqlParameter param1 = new SqlParameter("@DateOfBirth", new DateOnly(1998, 8, 20));
                AgeResult result = _Context.Database.SqlQueryRaw<AgeResult>("Select dbo.Fn_AgeCalculator (@DateOfBirth) as Age", param1).FirstOrDefault();
                ViewBag.result = result.Age;
                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        private DateTime? GetServerDateTime()
        {
            try
            {
                var result = _Context.Database.SqlQueryRaw<DateTime?>("EXEC GetServerDateTime").ToList();
                return result.FirstOrDefault();
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Unable to connect to the database. Please try again later.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while fetching the server date and time.", ex);
            }
        }
        public IActionResult SelectPrinter()
        {
            PrinterSelectViewModel model = new PrinterSelectViewModel();
            model.PrinterList = PrinterSettings.InstalledPrinters.Cast<string>().Select(x => new SelectListItem{Text = x, Value = x }).ToList();
            model.PrinterList.Insert(0, new SelectListItem { Text = "Select Printer", Value = "" });
            return View(model);
        }
        [HttpPost]
        public IActionResult SaveCNSelectedPrinter(string CNSelectedPrinter)
        {
            // Save the selected printer to the session
            HttpContext.Session.SetString("CNSelectedPrinter", CNSelectedPrinter);
            return RedirectToAction("Index");
        }
        public void PrintPDF(byte[] pdfBytes)
        {
            using (MemoryStream ms = new MemoryStream(pdfBytes))
            {
                using (PdfDocument pdfDocument = PdfDocument.Load(ms))
                {
                    PrintDocument printDoc = new PrintDocument();
                    string CNSelectedPrinter = HttpContext.Session.GetString("CNSelectedPrinter");
                    printDoc.PrinterSettings.PrinterName = CNSelectedPrinter;

                    int currentPage = 0;

                    printDoc.PrintPage += (sender, e) =>
                    {
                        if (currentPage < pdfDocument.PageCount)
                        {
                            var image = pdfDocument.Render(currentPage, e.PageBounds.Width, e.PageBounds.Height, true);
                            e.Graphics.DrawImage(image, e.PageBounds);
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
            byte[] reportBytes = PdfGenerate();
            PrintPDF(reportBytes);
            return View("Index");
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
                Salary = x.Salary,
                UserName = x.UserName,
                UserEid = x.UserEid,
                Password = x.Password,
                Status = x.Status
            }).ToList();

            var expriencesDataSource = new ReportDataSource { Name = "DataSet1" };
            localReport.DataSources.Add(expriencesDataSource);
            expriencesDataSource.Value = reportData;
            localReport.ReportPath = reportPath;
            string mimeType, encoding, fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes = localReport.Render("PDF", null,out mimeType,out encoding,out fileNameExtension, out streams,out warnings);
            return renderedBytes;
        }
        public class AgeResult
        {
            public string Age { get; set; }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
