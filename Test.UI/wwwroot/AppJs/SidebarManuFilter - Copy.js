// Connect to ProductHub
var productHubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/productHub")
    .build();

// Connect to DashboardHub
var dashboardHubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/dashboardHub")
    .build();



async function startConnections() {
    try {
        await productHubConnection.start();
        console.log("ProductHub connected");

        await dashboardHubConnection.start();
        console.log("DashboardHub connected");

        await orderHubConnection.start();
        console.log("OrderHub connected");

        // Optionally call methods on each hub once connected
        await productHubConnection.invoke("SendProductUpdates");
        await dashboardHubConnection.invoke("SendDashboardUpdates");
        await orderHubConnection.invoke("SendOrderUpdates");
    } catch (err) {
        console.error("Error connecting to hubs: ", err);
    }
}

// Start connections on page load
startConnections();
