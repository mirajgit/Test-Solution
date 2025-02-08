using Test.Entities;
using Test.UI.Models;

namespace Test.UI.Services.Interface
{
    public interface IProductRepository
    {
        List<ProductViewModel> GetAllProduct();
        ProductViewModel? GetProductById(int Id);
        void AddProduct(ProductViewModel model);
        void UpdateProduct(ProductViewModel model);
    }
}
