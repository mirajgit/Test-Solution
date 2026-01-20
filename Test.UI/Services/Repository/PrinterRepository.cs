using Microsoft.EntityFrameworkCore;
using Test.Entities;
using Test.UI.Models;
using Test.UI.Services.Interface;
using WebAPI.Data;

namespace Test.UI.Services.Repository
{
    public class PrinterRepository : IPrinterRepository
    {
		private readonly SMSanagement_DBContext _context;

        public PrinterRepository(SMSanagement_DBContext context)
        {
            _context = context;
        }

        public void AddPrinter(PrinterSelectViewModel model)
        {
            try
            {
                var PrinterInfos = _context.PrinterSelection.FirstOrDefault(x => x.PrinterId == model.PrinterId);
                if (model.PrinterType == "P")
                {
                    if (model.PrinterId !=null)
                    {
                        PrinterInfos.CNPrinter = model.CNSelectedPrinter;
                        PrinterInfos.SelectedDate = DateTime.Now;
                    }
                    else
                    {
                        PrinterSelection obj = new PrinterSelection();
                        obj.CNPrinter = model.CNSelectedPrinter;
                        obj.AccessUser = "DHK00001";
                        obj.SelectedDate = DateTime.Now;
                        _context.PrinterSelection.Add(obj);
                    }
                    _context.SaveChanges();
                }
                else if (model.PrinterType == "B")
                {
                    if (model.PrinterId > 0)
                    {
                        PrinterInfos.BarcodePrinter = model.BarcodeSelectedPrinter;
                        PrinterInfos.HV = model.HV;
                        PrinterInfos.SelectedDate = DateTime.Now;
                    }
                    else
                    {
                        PrinterSelection obj = new PrinterSelection();
                        obj.BarcodePrinter = model.BarcodeSelectedPrinter;
                        obj.AccessUser = "DHK00001";
                        obj.HV = model.HV;
                        obj.SelectedDate = DateTime.Now;
                        _context.PrinterSelection.Add(obj);
                    }
                    _context.SaveChanges();
                }  
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public PrinterSelectViewModel GetCNSelectedPrinter(string AccessUser)
        {
			try
			{
                var result= _context.PrinterSelection.Where(x=>x.AccessUser==AccessUser).Select(x=>new PrinterSelectViewModel
                {
                    PrinterId = x.PrinterId,
                    CNPrinter=x.CNPrinter,
                    BarcodePrinter=x.BarcodePrinter,
                    AccessUser=AccessUser,
                    HV=x.HV,
                    SelectedDate=x.SelectedDate
                }).FirstOrDefault();
                return result;
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
        }
    }
}
