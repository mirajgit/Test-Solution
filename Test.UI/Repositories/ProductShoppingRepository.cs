using Microsoft.Data.SqlClient;
using System.Data;
using Test.Entities;

namespace Test.UI.Repositories
{
    public class ProductShoppingRepository
    {
        string connectionString;
        public ProductShoppingRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Product> GetShoppingProducts()
        {
            List<Product> products = new List<Product>();
            Product product;

            var data = GetShoppingProductDetailsFromDb();

            foreach (DataRow row in data.Rows)
            {
                product = new Product
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Category = row["Category"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    Discount = row["Discount"] != DBNull.Value ? Convert.ToInt32(row["Discount"]) : 0,
                    Image = row["Image"].ToString(),
                    Description = row["Description"].ToString(),
                };
                products.Add(product);
            }

            return products;
        }

        private DataTable GetShoppingProductDetailsFromDb()
        {
            var query = "SELECT Id,Name,Category,Price,Image,Description,Discount FROM Product";
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                    return dataTable;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
