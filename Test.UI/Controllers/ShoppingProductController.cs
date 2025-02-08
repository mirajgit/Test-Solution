using Microsoft.AspNetCore.Mvc;
using Test.UI.Services.Interface;

namespace Test.UI.Controllers
{
    public class ShoppingProductController : Controller
    {

        private readonly IProductRepository productRepository;

        public ShoppingProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        [HttpGet]
        public IActionResult Shopping()
        {
            return View();
        }
        public IActionResult ShoppingList()
        {
            var result = productRepository.GetAllProduct().ToList();
            return PartialView("_ShoppingList", result);
        }
    }
}
