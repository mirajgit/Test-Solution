using Test.UI.Models;

namespace Test.UI.Services.Interface
{
    public interface IPrinterRepository
    {
        PrinterSelectViewModel GetCNSelectedPrinter(string AccessUser);
       void  AddPrinter(PrinterSelectViewModel model);
    }
}
