﻿using TableDependency.Enums;
using TableDependency.EventArgs;
using TableDependency.SqlClient;
using Test.UI.Hubs;
using Test.UI.Models;

namespace Test.UI.SubscribeTableDependencies
{
    public class SubscribeProductTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<Product> tableDependency;
        DashboardHub dashboardHub;
        ProductHub productHub;

        public SubscribeProductTableDependency(DashboardHub dashboardHub, ProductHub productHub)
        {
            this.dashboardHub = dashboardHub;
            this.productHub = productHub;
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
                _ = Task.Run(async () => await dashboardHub.SendProducts()); // No parameter
                _ = Task.Run(async () => await productHub.SendShoppingProducts()); // No parameter
            }
        }
    }
}
