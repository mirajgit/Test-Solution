$('document').ready(function () {
	GetDataOnPageLoad();
});
"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/productHub").build();
$(function () {
    connection.start().then(function () {
		InvokeShoppingProducts();
    }).catch(function (err) {
        return console.error(err.toString());
    });
});

// Product
function InvokeShoppingProducts() {
	connection.invoke("SendShoppingProducts").catch(function (err) {
		return console.error(err.toString());
	});
}

connection.on("ReceivedShoppingProducts", function (products) {
	BindProductsToGrid(products);
});

function BindProductsToGrid(products) {
    $('#ProductList').empty(); // Clear existing products
    var row = $('<div class="row">'); // Start a new row for products

    $.each(products, function (index, product) {
        // If discount is null or undefined, set it to 0
        var discount = product.discount || 0;

        var discountCircle = '';
        if (discount > 0) {
            discountCircle = `
                <div class="discount-circle">-${discount}%</div>
            `;
        }

        var productCard = `
            <div class="col-12 col-sm-6 col-md-3 col-lg-3">
                <div class="card shadow-lg">
                    <img src="/ProductImage/${product.image}" class="card-img-top" alt="Product Image">
                    ${discountCircle}
                    <div class="card-body text-center">
                        <h5 class="card-title">${product.name}</h5>
                        <p class="price">${product.price} ৳</p>
                        <p class="card-text text-muted">${product.description || 'No description available'}</p>
                        <div class="d-flex justify-content-between">
                            <button class="btn btn-sm btn-primary btn-custom w-90" onclick="AddToCart(${product.id})">Add to Cart</button>
                            <button class="btn btn-sm btn-success btn-custom w-90" onclick="BuyNow(${product.id})">Buy Now</button>
                        </div>
                    </div>
                </div>
            </div>
        `;

        row.append(productCard); // Add each product card to the row
    });

    $('#ProductList').append(row); // Append the row to the container
}


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
			$("#Discount").val(res.record.discount);
			$("#Description").val(res.record.description);
		}
	});
}

$(document).on("click", "#btnUpdate", function () {
	var Id = $("#Id").val();
	var Name = $("#Name").val();
	var Category = $("#Category").val();
	var Price = $("#Price").val();
	var Discount = $("#Discount").val();
	var Description = $("#Description").val();
	var postData = {
		'Id': Id,
		'Name': Name,
		'Category': Category,
		'Price': Price,
		'Discount': Discount,
		'Description': Description,
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
	var Discount = $("#Discount").val();
	var Description = $("#Description").val();
	var postData = {
		'Name': Name,
		'Category': Category,
		'Price': Price,
		'Discount': Discount,
		'Description': Description,
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
	$("#Discount").val('');
	$("#Description").val('');
	$('#modal-xl').modal('hide');
	InvokeShoppingProducts();
}
function GetDataOnPageLoad() {
	$("#ShoppingList").html("");
	$.ajax({
		type: "POST",
		url: "/ShoppingProduct/ShoppingList",
		success: function (response) {
			$("#ShoppingList").html(response);
		}
	});
}