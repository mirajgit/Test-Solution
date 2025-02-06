using Microsoft.AspNetCore.Mvc;
using Test.Entities;
using Test.UI.Services.Interface;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace Test.UI.Controllers
{
    public class ProductController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IProductRepository _iProductRepository;

        public ProductController(IProductRepository iProductRepository)
        {
            _iProductRepository = iProductRepository;
        }

        [HttpPost]
        public IActionResult ProductCreate(Product model)
        {
            try
            {
                var result = _iProductRepository.GetAllProduct().Where(x => x.Id != model.Id && x.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault();
                if (result != null)
                {
                    return Json(new { Success = false, Message = "Product Name is already exist! " });
                }
                _iProductRepository.AddProduct(model);
                return Json(new { Success = true, Message = "Saved Successfuly" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message });
            }
        }
        [HttpPost]
        public IActionResult ProductUpdate(Product model)
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
            Product model = new Product();
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
                return Json(new { Success = true, Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message });
            }
        }
    }
}
