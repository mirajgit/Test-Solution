using Microsoft.AspNetCore.SignalR;
using Test.UI.Repositories;

namespace Test.UI.Hubs
{
    public class ProductHub:Hub
    {
        ProductShoppingRepository productShoppingRepository;
        public ProductHub(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Test_Con");
            productShoppingRepository = new ProductShoppingRepository(connectionString);
        }
        public async Task SendShoppingProducts()
        {
            var ShoppingProducts = productShoppingRepository.GetShoppingProducts();
            await Clients.All.SendAsync("ReceivedShoppingProducts", ShoppingProducts);
        }
    }
}
