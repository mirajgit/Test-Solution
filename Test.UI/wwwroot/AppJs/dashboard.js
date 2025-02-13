﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dashboardHub").build();
$(function () {
    connection.start().then(function () {
		InvokeProducts();
		InvokeSales();
		InvokeCustomers();

    }).catch(function (err) {
        return console.error(err.toString());
    });
});

// Product
function InvokeProducts() {
	connection.invoke("SendProducts").catch(function (err) {
		return console.error(err.toString());
	});
}

connection.on("ReceivedProducts", function (products) {
	BindProductsToGrid(products);
});

function BindProductsToGrid(products) {
	$('#tblProduct tbody').empty();
	var tr;
	$.each(products, function (index, product) {
		tr = $('<tr/>');
		tr.append(`<td>${(index + 1)}</td>`);
		tr.append(`<td>${product.name}</td>`);
		tr.append(`<td>${product.category}</td>`);
		tr.append(`<td>${product.price}</td>`);
		// Create the button and append it to the last <td>
		var button = $('<a/>', {
			href: '#modal-xl',
			class: 'btn btn-sm btn-info',
			'data-bs-toggle': 'modal', // Use 'data-bs-toggle' for Bootstrap 5
			html: '<i class="fas fa-check-square"></i>',
			click: function () {
				GetEditProduct(product.id.toString());
			}
		});


		tr.append($('<td/>').append(button));

		$('#tblProduct').append(tr);
	});
}

connection.on("ReceivedProductsForGraph", function (productsForGraph) {
	BindProductsToGraph(productsForGraph);
});

function BindProductsToGraph(productsForGraph) {
	var labels = [];
	var data = [];

	$.each(productsForGraph, function (index, item) {
		labels.push(item.category);
		data.push(item.products);
	});

	DestroyCanvasIfExists('canvasProudcts');

	const context = $('#canvasProudcts');
	const myChart = new Chart(context, {
		type: 'doughnut',
		data: {
			labels: labels,
			datasets: [{
				label: '# of Products',
				data: data,
				backgroundColor: backgroundColors,
				borderColor: borderColors,
				borderWidth: 1
			}]
		},
		options: {
			scales: {
				y: {
					beginAtZero: true
				}
			}
		}
	});
}

// Sale
function InvokeSales() {
	connection.invoke("SendSales").catch(function (err) {
		return console.error(err.toString());
	});
}

connection.on("ReceivedSales", function (sales) {
	BindSalesToGrid(sales);
});

function BindSalesToGrid(sales) {
	$('#tblSale tbody').empty();

	var tr;
	$.each(sales, function (index, sale) {
		tr = $('<tr/>');
		tr.append(`<td>${(index + 1)}</td>`);
		tr.append(`<td>${sale.customer}</td>`);
		tr.append(`<td>${sale.amount}</td>`);
		tr.append(`<td>${sale.purchasedOn}</td>`);
		$('#tblSale').append(tr);
	});
}

connection.on("ReceivedSalesForGraph", function (salesForGraph) {
	BindSalesToGraph(salesForGraph);
});

function BindSalesToGraph(salesForGraph) {
	var labels = [];
	var data = [];

	$.each(salesForGraph, function (index, item) {
		labels.push(item.purchasedOn);
		data.push(item.amount);
	});

	DestroyCanvasIfExists('canvasSales');

	const context = $('#canvasSales');
	const myChart = new Chart(context, {
		type: 'line',
		data: {
			labels: labels,
			datasets: [{
				label: '# of Sales',
				data: data,
				backgroundColor: backgroundColors,
				borderColor: borderColors,
				borderWidth: 1
			}]
		},
		options: {
			scales: {
				y: {
					beginAtZero: true
				}
			}
		}
	});
}

// Customer
function InvokeCustomers() {
	connection.invoke("SendCustomers").catch(function (err) {
		return console.error(err.toString());
	});
}

connection.on("ReceivedCustomers", function (customers) {
	BindCustomersToGrid(customers);
});

function BindCustomersToGrid(customers) {
	$('#tblCustomer tbody').empty();

	var tr;
	$.each(customers, function (index, customer) {
		tr = $('<tr/>');
		tr.append(`<td>${(index + 1)}</td>`);
		tr.append(`<td>${customer.name}</td>`);
		tr.append(`<td>${customer.gender}</td>`);
		tr.append(`<td>${customer.mobile}</td>`);
		$('#tblCustomer').append(tr);
	});
}

connection.on("ReceivedCustomersForGraph", function (customersForGraph) {
	BindCustomersToGraph(customersForGraph);
});

function BindCustomersToGraph(customersForGraph) {
	var datasets = [];
	var labels = ['Customers']
	var data = [];
	$.each(customersForGraph, function (index, item) {
		data = [];
		data.push(item.customers);
		var dataset = {
			label: item.gender,
			data: data,
			backgroundColor: backgroundColors[index],
			borderColor: borderColors[index],
			borderWidth: 1
		};

		datasets.push(dataset);
	});

	DestroyCanvasIfExists('canvasCustomers');

	const context = $('#canvasCustomers');
	const myChart = new Chart(context, {
		type: 'bar',
		data: {
			labels: labels,
			datasets: datasets,
		},
		options: {
			scales: {
				y: {
					beginAtZero: true
				}
			}
		}
	});
}

// supporting functions for Graphs
function DestroyCanvasIfExists(canvasId) {
	let chartStatus = Chart.getChart(canvasId);
	if (chartStatus != undefined) {
		chartStatus.destroy();
	}
}

var backgroundColors = [
	'rgba(255, 99, 132, 0.2)',
	'rgba(54, 162, 235, 0.2)',
	'rgba(255, 206, 86, 0.2)',
	'rgba(75, 192, 192, 0.2)',
	'rgba(153, 102, 255, 0.2)',
	'rgba(255, 159, 64, 0.2)'
];
var borderColors = [
	'rgba(255, 99, 132, 1)',
	'rgba(54, 162, 235, 1)',
	'rgba(255, 206, 86, 1)',
	'rgba(75, 192, 192, 1)',
	'rgba(153, 102, 255, 1)',
	'rgba(255, 159, 64, 1)'
];

function GetEditProduct(id) {
	var param = {};
	param.id = id;
	$.get("/Product/EditProduct", param, function (res) {
		if (!res.success) {
			toastr.warning(res.Message);
		}
		else {
			$("#Id").val(res.record.id);
			$("#Name").val(res.record.name);
			$("#Category").val(res.record.category);
			$("#Price").val(res.record.price);
		}
	});
}

$(document).on("click", "#btnUpdate", function () {
	var Id = $("#Id").val();
	var Name = $("#Name").val();
	var Category = $("#Category").val();
	var Price = $("#Price").val();
	var postData = {
		'Id': Id,
		'Name': Name,
		'Category': Category,
		'Price': Price
	};
	$.post("/Product/ProductUpdate", postData, function (res) {
		if (res.success) {
			toastr.success(res.message);
			ClearUI()
		}
		else {

			toastr.warning(res.message);
		}
	});
});
$(document).on("click", "#btnSubmit", function () {
	var Name = $("#Name").val();
	var Category = $("#Category").val();
	var Price = $("#Price").val();
	var postData = {
		'Name': Name,
		'Category': Category,
		'Price': Price
	};
	$.post("/Product/ProductCreate", postData, function (res) {
		if (res.success) {
			toastr.success(res.message);
			ClearUI()
		}
		else {

			toastr.warning(res.message);
		}
	});
});


function ClearUI() {
	$("#Id").val(0);
	$("#Name").val('');
	$("#Category").val('');
	$("#Price").val('');
	$('#modal-xl').modal('hide');
	InvokeProducts();
	InvokeSales();
	InvokeCustomers();
}