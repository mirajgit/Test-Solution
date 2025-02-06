using TableDependency.Enums;
using TableDependency.EventArgs;
using TableDependency.SqlClient;
using Test.UI.Hubs;
using Test.UI.Models;

namespace Test.UI.SubscribeTableDependencies
{

    public class SubscribeCustomerTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<Customer> tableDependency;
        DashboardHub dashboardHub;

        public SubscribeCustomerTableDependency(DashboardHub dashboardHub)
        {
            this.dashboardHub = dashboardHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<Customer>(connectionString);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnChanged(object sender, RecordChangedEventArgs<Customer> e)
        {
            if (e.ChangeType != ChangeType.None)
            {
                _ = Task.Run(async () => await dashboardHub.SendCustomers()); // No parameter
            }
        }
        private void TableDependency_OnError(object sender, TableDependency.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"Error: {e.Error.Message}");
        }
    }
}
