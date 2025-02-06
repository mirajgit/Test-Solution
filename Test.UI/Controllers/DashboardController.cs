using Microsoft.AspNetCore.Mvc;

namespace Test.UI.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
