using Test.Entities;

namespace Test.UI.Services.Interface
{
    public interface IProductRepository
    {
        List<Product> GetAllProduct();
        Product? GetProductById(int Id);
        void AddProduct(Product model);
        void UpdateProduct(Product model);
    }
}
