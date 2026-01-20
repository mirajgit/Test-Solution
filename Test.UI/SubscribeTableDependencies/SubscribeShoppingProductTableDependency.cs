using TableDependency.Enums;
using TableDependency.EventArgs;
using TableDependency.SqlClient;
using Test.Entities;
using Test.UI.Hubs;
using WebAPI.Data;

namespace Test.UI.SubscribeTableDependencies
{
    public class SubscribeShoppingProductTableDependency: ISubscribeTableDependency
    {
        SqlTableDependency<Product> tableDependency;
        ProductHub productHub;
        DashboardHub dashboardHub;

        public SubscribeShoppingProductTableDependency(ProductHub productHub, DashboardHub dashboardHub)
        {
            this.productHub = productHub;
            this.dashboardHub = dashboardHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<Product>(connectionString);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnError(object sender, TableDependency.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Product)} SqlTableDependency error: {e.Error.Message}");
        }

        private void TableDependency_OnChanged(object sender, RecordChangedEventArgs<Product> e)
        {
           if (e.ChangeType == ChangeType.Update || e.ChangeType == ChangeType.Insert || e.ChangeType == ChangeType.Delete)
            {
                _ = Task.Run(async () => await productHub.SendShoppingProducts()); // No parameter
                _ = Task.Run(async () => await dashboardHub.SendProducts());
            }
        }
    }
}
