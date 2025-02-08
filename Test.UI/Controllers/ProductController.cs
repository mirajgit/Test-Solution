using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Test.Entities;
using Test.UI.Hubs;
using Test.UI.Models;
using Test.UI.Services.Interface;
using Test.UI.SubscribeTableDependencies;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace Test.UI.Controllers
{
    public class ProductController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IProductRepository _iProductRepository;
        private readonly IHubContext<ProductHub> _productHubContext;
        private readonly SubscribeProductTableDependency subscribeProductTableDependency;
        private string _connectionString;
        public ProductController(IConfiguration Configuration, IProductRepository iProductRepository, IHubContext<ProductHub> productHubContext, SubscribeProductTableDependency subscribeProductTableDependency)
        {
            _iProductRepository = iProductRepository;
            _productHubContext = productHubContext;
            this.subscribeProductTableDependency = subscribeProductTableDependency;
            _connectionString= Configuration.GetConnectionString("Test_Con");
        }
        [HttpPost]
        public IActionResult ProductCreate(ProductViewModel model)
        {
            try
            {
                var result = _iProductRepository.GetAllProduct().Where(x => x.Id != model.Id && x.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault();
                if (result != null)
                {
                    return Json(new { Success = false, Message = "Product Name is already exist! " });
                }
                _iProductRepository.AddProduct(model);
                subscribeProductTableDependency.SubscribeTableDependency(_connectionString);
                // Return success response
                return Json(new { Success = true, Message = "Saved Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message });
            }
        }
        [HttpPost]
        public IActionResult ProductUpdate(ProductViewModel model)
        {
            try
            {
                var result = _iProductRepository.GetAllProduct().Where(x => x.Id != model.Id && x.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault();
                if (result != null)
                {
                    return Json(new { Success = false, Message = "Product Name is already exist! " });
                }
                _iProductRepository.UpdateProduct(model);
                return Json(new { Success = true, Message = "Update Successfuly" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message });
            }
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult EditProduct(int id)
        {
            ProductViewModel model = new ProductViewModel();
            try
            {
                var ProductInfo = _iProductRepository.GetProductById(id);
                if (ProductInfo == null)
                {
                    return Json(new { Success = false, Message = "Product Info not Found" });
                }
                model.Id = ProductInfo.Id;
                model.Name = ProductInfo.Name;
                model.Category = ProductInfo.Category;
                model.Price = ProductInfo.Price;
                model.Discount = ProductInfo.Discount;
                model.Description = ProductInfo.Description;
                return Json(new { Success = true, Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message });
            }
        }
    }
}
