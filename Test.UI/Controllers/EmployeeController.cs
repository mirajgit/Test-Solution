using Microsoft.AspNetCore.Mvc;

namespace Test.UI.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult EmployeeInformation()
        {
            return View();
        }
    }
}
