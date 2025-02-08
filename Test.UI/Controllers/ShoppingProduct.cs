using Microsoft.AspNetCore.Mvc;

namespace Test.UI.Controllers
{
    public class ShoppingProduct : Controller
    {
        public IActionResult Shopping()
        {
            return View();
        }
    }
}
