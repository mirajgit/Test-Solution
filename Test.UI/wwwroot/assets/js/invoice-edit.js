/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
var __webpack_exports__ = {};
/*!*********************************************!*\
  !*** ./resources/assets/js/invoice-edit.js ***!
  \*********************************************/
 //disply textarea on click

function displayTextArea() {
  var textAreaElement = document.querySelector('#shipping-address'),
      addAddressBtn = document.querySelector('#addShippingAddress');
  addAddressBtn.addEventListener('click', function () {
    if (textAreaElement.classList.contains('d-none')) {
      textAreaElement.classList.remove('d-none');
    } else {
      textAreaElement.classList.add('d-none');
    }

    addAddressBtn.classList.add('d-none');
  });
}

displayTextArea(); //adding and removing invoice

function addRemoveInvoiceItem() {
  var invoiceContainer = document.querySelector('.product-description-list');
  var invoiceEach = document.querySelector('.product-description-each'); //remove product invoice

  function removeProductInvoice() {
    setInterval(function () {
      setTimeout(function () {
        var removeBtn = document.querySelectorAll('.delete-row-btn');

        for (var i = 0; i < removeBtn.length; i++) {
          removeBtn[i].addEventListener('click', removeInvoice);
        }
      }, 1);
    }, 1);

    function removeInvoice($e) {
      invoiceContainer.removeChild($e.target.parentElement);
    }
  }

  removeProductInvoice(); //add product invoice

  function addProductInvoice() {
    var invoiceEachCopy = invoiceEach.cloneNode(true);
    var addBtn = document.querySelector('.add-invoice-item-btn');
    addBtn.addEventListener('click', addInvoice);

    function addInvoice() {
      var newInvoice = invoiceEachCopy.cloneNode(true);
      var newInvoiceInputs = newInvoice.children[0].children[1].children[0].children;

      for (var i = 0; i < newInvoiceInputs.length; i++) {
        newInvoiceInputs[0].children.defaultValue = " ";
        console.log(newInvoiceInputs[0].children.value);
      }

      invoiceContainer.appendChild(newInvoice);
      select2Search(); //to render select2 options in invoice
    }
  }

  addProductInvoice();
}

addRemoveInvoiceItem(); //date pickers

$(function () {
  $("#inv-datepicker").datepicker({
    autoclose: true,
    format: 'dd-mm-yyyy',
    todayHighlight: true
  }).datepicker('update', '10-09-2021');
  $("#due-datepicker").datepicker({
    autoclose: true,
    format: 'dd-mm-yyyy',
    todayHighlight: true
  }).datepicker('update', '11-11-2021');
}); // Select2

$('.select2').select2({
  minimumResultsForSearch: Infinity,
  width: '100%'
}); // Select2 by showing the search

function select2Search() {
  $('.select2-show-search').select2({
    minimumResultsForSearch: '',
    width: '100%'
  });
}

select2Search(); //select2 client dropdown with search

function selectClient(client) {
  if (!client.id) {
    return client.text;
  }

  var $client = $('<span><img src="https://laravelui.spruko.com/noa/assets/images/users/' + client.element.value.toLowerCase() + '.jpg" class="rounded-circle avatar-sm" /> ' + client.text + '</span>');
  return $client;
}

;
$(".select2-client-search").select2({
  templateResult: selectClient,
  templateSelection: selectClient,
  escapeMarkup: function escapeMarkup(m) {
    return m;
  }
});
/******/ })()
;