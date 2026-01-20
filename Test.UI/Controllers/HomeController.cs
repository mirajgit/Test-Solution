using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using PdfiumViewer;
using SkiaSharp;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Net.NetworkInformation;
using System.Web;
using Test.Entities;
using Test.UI.Models;
using WebAPI.Data;
using ZXing;
using ZXing.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
        [HttpGet]
        public IActionResult Decrypt(string encryptedPath)
        {
            if (!string.IsNullOrEmpty(encryptedPath))
            {
                // URL-decode the encrypted path before decrypting
                var decodedPath = HttpUtility.UrlDecode(encryptedPath);
                var decryptedPath = DecryptUrl(decodedPath);  // Decrypt the URL path back

                if (!string.IsNullOrEmpty(decryptedPath))
                {
                    // Now, handle the decrypted path: assuming it's something like "Controller/Action"
                    var pathParts = decryptedPath.Split('/');

                    // Assuming the first part is the controller, and the second is the action
                    if (pathParts.Length == 2)
                    {
                        // Redirect to the controller and action
                        return RedirectToAction(pathParts[1], pathParts[0]);
                    }
                }
            }

            // In case of failure, return a 404 or another appropriate result
            return NotFound();
        }

        private string DecryptUrl(string encryptedPath)
        {
            try
            {
                // Revert the URL-safe Base64 encoding
                var urlSafeBase64 = encryptedPath.Replace('-', '+').Replace('_', '/');
                var padding = urlSafeBase64.Length % 4;
                if (padding > 0)
                {
                    urlSafeBase64 = urlSafeBase64.PadRight(urlSafeBase64.Length + (4 - padding), '=');
                }

                // Decode the Base64 string back to the original URL
                var pathBytes = Convert.FromBase64String(urlSafeBase64);
                return System.Text.Encoding.UTF8.GetString(pathBytes);  // Return the decrypted URL
            }
            catch (Exception)
            {
                // Handle decryption failure
                return string.Empty;
            }
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
                SqlParameter param1 = new SqlParameter("@DateOfBirth", new DateOnly(2023, 03, 01));
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
            if (!System.IO.File.Exists(reportPath))
                throw new FileNotFoundException($"RDLC file not found at {reportPath}");

            Microsoft.Reporting.NETCore.LocalReport localReport = new Microsoft.Reporting.NETCore.LocalReport { ReportPath = reportPath };

            // Step 1: Fetch data from database
            var users = _Context.Tbl_HRM_UserInformation
                .Select(x => new
                {
                    x.UserId,
                    x.UserName,
                    x.LoginName,
                    x.Salary,
                    x.UserEid,
                    x.Password,
                    x.Status
                }).ToList(); // ✅ Execute query, data is in memory

            // Step 2: Generate barcode images in memory
            var reportData = users.Select(x => new
            {
                x.UserId,
                x.UserName,
                x.LoginName,
                x.Salary,
                x.Password,
                x.Status,
                UserEid = GenerateBarcodeBase64(x.UserEid.ToString()) // ✅ Now safe
            }).ToList();

            // 3️⃣ Set data source
            var dataSource = new ReportDataSource { Name = "DataSet1", Value = reportData };
            localReport.DataSources.Add(dataSource);

            // 4️⃣ Render PDF
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

            return renderedBytes;
        }

        // ✅ Helper method to generate barcode Base64 string
        private string GenerateBarcodeBase64(string data)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_39,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 80,
                    Width = 300,
                    Margin = 0
                }
            };

            var pixelData = writer.Write(data);

            using (var bitmap = new SKBitmap(pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul))
            {
                // Copy pixels
                unsafe
                {
                    fixed (byte* src = pixelData.Pixels)
                    {
                        IntPtr dst = bitmap.GetPixels();
                        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, dst, pixelData.Pixels.Length);
                    }
                }

                using (var image = SKImage.FromBitmap(bitmap))
                using (var ms = new MemoryStream())
                {
                    image.Encode(SKEncodedImageFormat.Png, 100).SaveTo(ms);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        public static string GenerateQRCodeBase64(string data)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = 150,
                    Height = 150,
                    Margin = 0
                }
            };

            var pixelData = writer.Write(data);

            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
            {
                for (int y = 0; y < pixelData.Height; y++)
                {
                    for (int x = 0; x < pixelData.Width; x++)
                    {
                        int index = (y * pixelData.Width + x) * 4;
                        byte r = pixelData.Pixels[index];
                        byte g = pixelData.Pixels[index + 1];
                        byte b = pixelData.Pixels[index + 2];
                        bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }

                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        public IActionResult GetBarcode()
        {
            string reportPath = Path.Combine(_environment.WebRootPath, "Reports", "Barcode.rdlc");
            if (!System.IO.File.Exists(reportPath))
                throw new FileNotFoundException($"RDLC file not found at {reportPath}");

            Microsoft.Reporting.NETCore.LocalReport localReport = new Microsoft.Reporting.NETCore.LocalReport { ReportPath = reportPath };

            // Step 1: Fetch data from database
            var users = _Context.Tbl_HRM_UserInformation
                .Select(x => new
                {
                    x.UserId,
                    x.UserName,
                    x.LoginName,
                    x.Salary,
                    x.UserEid,
                    x.Password,
                    x.Status,
                    x.BarcodeImage,
                   x.QRCodeImage
                }).ToList(); // ✅ Execute query, data is in memory

            // Step 2: Generate barcode images in memory
            var reportData = users.Select(x => new
            {
                x.UserId,
                x.UserName,
                x.LoginName,
                x.Salary,
                x.Password,
                x.Status,
                BarcodeImage = GenerateBarcodeBase64(x.BarcodeImage),
                QRCodeImage = GenerateQRCodeBase64(x.QRCodeImage),
            }).ToList();

            // 3️⃣ Set data source
            var dataSource = new ReportDataSource { Name = "DataSet1", Value = reportData };
            localReport.DataSources.Add(dataSource);

            // 4️⃣ Render PDF
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
            return File(renderedBytes, "application/pdf");
        }
        public IActionResult GetQRBarcode()
        {
            string reportPath = Path.Combine(_environment.WebRootPath, "Reports", "Invoice.rdlc");
            if (!System.IO.File.Exists(reportPath)) throw new FileNotFoundException($"RDLC file not found at {reportPath}");

            Microsoft.Reporting.NETCore.LocalReport localReport = new Microsoft.Reporting.NETCore.LocalReport { ReportPath = reportPath };
            var users = _Context.View_SalesInvoice.Where(x=>x.SalesId==1)
                .Select(x => new
                {
                    x.SaleId,
                    x.InvoiceNo,
                    x.CustomarName,
                    x.SaleDate,
                    x.Tax,
                    x.Discount,
                    x.GrandTotal,
                    x.TotalQuantity,
                    x.PaidStatus,
                    x.BarcodeImage,

                    x.SalesDetailsId,
                    x.SalesId,
                    x.Item,
                    x.Quantity,
                    x.UnitPrice,
                    x.Total
                }).ToList();
            var reportData = users.Select(x => new
            {
                x.SaleId,
                x.InvoiceNo,
                x.CustomarName,
                x.SaleDate,
                x.Tax,
                x.Discount,
                x.GrandTotal,
                x.TotalQuantity,
                x.PaidStatus,
                BarcodeImage = GenerateQRCodeBase64(x.BarcodeImage),
                x.SalesDetailsId,
                x.SalesId,
                x.Item,
                x.Quantity,
                x.UnitPrice,
                x.Total

            }).ToList();

            // 3️⃣ Set data source
            var dataSource = new ReportDataSource { Name = "DataSet1", Value = reportData };
            localReport.DataSources.Add(dataSource);

            // 4️⃣ Render PDF
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
            return File(renderedBytes, "application/pdf");
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
