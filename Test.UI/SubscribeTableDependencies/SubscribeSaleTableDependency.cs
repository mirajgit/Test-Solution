using TableDependency.Enums;
using TableDependency.EventArgs;
using TableDependency.SqlClient;
using Test.UI.Hubs;
using Test.UI.Models;

namespace Test.UI.SubscribeTableDependencies
{
    public class SubscribeSaleTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<Sale> tableDependency;
        DashboardHub dashboardHub;

        public SubscribeSaleTableDependency(DashboardHub dashboardHub)
        {
            this.dashboardHub = dashboardHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<Sale>(connectionString);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnError(object sender, TableDependency.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Product)} SqlTableDependency error: {e.Error.Message}");
        }

        private void TableDependency_OnChanged(object sender, RecordChangedEventArgs<Sale> e)
        {
            if (e.ChangeType != ChangeType.None)
            {
                _ = Task.Run(async () => await dashboardHub.SendSales()); // No parameter
            }
        }
    }
}
