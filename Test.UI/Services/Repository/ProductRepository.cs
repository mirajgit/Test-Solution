using Microsoft.EntityFrameworkCore;
using Test.Entities;
using Test.UI.Models;
using Test.UI.Services.Interface;

namespace Test.UI.Services.Repository
{
    public class ProductRepository: IProductRepository
    {
        public void AddProduct(ProductViewModel model)
        {
            try
            {
                using (var _context = new SMSanagement_DBContext())
                {
                    // Disable triggers temporarily
                    _context.Database.ExecuteSqlRaw("DISABLE TRIGGER ALL ON Product");

                    Entities.Product obj = new Entities.Product();
                    obj.Name = model.Name;
                    obj.Category = model.Category;
                    obj.Price = model.Price;
                    obj.Discount = (int)model.Discount;
                    obj.Description = model.Description;
                    _context.Product.Add(obj);
                    _context.SaveChanges();
                    _context.Database.ExecuteSqlRaw("ENABLE TRIGGER ALL ON Product");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<ProductViewModel> GetAllProduct()
        {
            try
            {
                using (var _context = new SMSanagement_DBContext())
                {
                    var result = _context.Product
                        .Select(x => new ProductViewModel
                        {
                            Id=x.Id,Name = x.Name, Category = x.Category, Price = x.Price,Discount = x.Discount,Description = x.Description
                        }).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ProductViewModel? GetProductById(int Id)
        {
            try
            {
                using (var _context = new SMSanagement_DBContext())
                {
                    return _context.Product.Where(c => c.Id == Id)
                            .Select(x => new ProductViewModel
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Category = x.Category,
                                Price = x.Price,
                                Discount = x.Discount,
                                Description = x.Description,
                            }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateProduct(ProductViewModel model)
        {
            try
            {
                using (var _context = new SMSanagement_DBContext())
                {
                    _context.Database.ExecuteSqlRaw("DISABLE TRIGGER ALL ON Product");
                    var result = _context.Product.FirstOrDefault(x => x.Id == model.Id);
                    if (result != null)
                    {
                        result.Name = model.Name;
                        result.Category = model.Category;
                        result.Price = model.Price;
                        result.Description = model.Description;
                        _context.SaveChanges();
                    }
                    _context.Database.ExecuteSqlRaw("ENABLE TRIGGER ALL ON Product");
                }
            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message);
            }
        }
    }
}
