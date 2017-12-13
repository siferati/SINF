var curName;
var curAddress;
var curPhone;
var curEmail;
var curFiscal;
var curBirth;
var curHired;
var curDesc;
var curID;
var repNumber;
var adding = false;


function addSalesRep(name, address, phone, email, fiscal, hired, birth, desc) {

var jsonName = name.toString();
var jsonAddress = address.toString();
var jsonPhone = phone.toString();
var jsonEmail = email.toString();
var jsonFiscal = fiscal.toString();
var jsonHired = hired.toString();
var jsonBirth = birth.toString();
var jsonDesc = desc.toString();

$.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        url: 'http://localhost:49822/api/vendedores/',
        
        data: JSON.stringify ({
                "fiscalID": jsonFiscal,
                "name": jsonName,
                "address": jsonAddress,
                "phone": jsonPhone,
                "email": jsonEmail,
                "birthDate": jsonBirth,
                "hiredDate": jsonHired,
                "description": jsonDesc
            }),
        success: function (data) {
            console.log("Request succeded!");
            alert("Rep Added");
            window.location.reload(true);
        },
        error: function (data, textStatus) {
            console.log("Request failed!");
            alert(textStatus);
        }
    });
}

function editSalesRep(id, name, address, phone, email, hired, birth, desc) {

var jsonID = id.toString();
var jsonName = name.toString();
var jsonAddress = address.toString();
var jsonPhone = phone.toString();
var jsonEmail = email.toString();
var jsonHired = hired.toString();
var jsonBirth = birth.toString();
var jsonDesc = desc.toString();

$.ajax({
        type: 'PUT',
        dataType: 'json',
        contentType: 'application/json',
        url: 'http://localhost:49822/api/vendedores/' + jsonID,
        
        data: JSON.stringify ({
                "name": jsonName,
                "address": jsonAddress,
                "phone": jsonPhone,
                "email": jsonEmail,
                "birthDate": jsonBirth,
                "hiredDate": jsonHired,
                "description": jsonDesc
            }),
        success: function (data) {
            console.log("Request succeded!");
            alert("Rep Edited");
            window.location.reload(true);
        },
        error: function (data, textStatus) {
            console.log("Request failed!");
            alert(textStatus);
        }
    });
}

function getSaleById(id){


    $.ajax({
        type: 'GET',
        url: 'http://localhost:49822/api/orders/' + id.toString(),
        success: getSaleByIdHandler,
        error: function () {
            console.log("Request failed!");
        }
    });
}

function getSaleByIdHandler(data) {
    console.log(data);

    var sale = data;


    var html =
            '<div class="title">'
                + '<h2 class="title-text"><span class="glyphicon glyphicon-inbox"></span> Order Info </h2>'
            +' </div>'
            + '<div class="order-info">'
                + '<p class="customer">Client: ' + sale.customerId + '</p>'
                + '<p class="delivery-address">Delivery Address: ' + sale.deliveryAddress + '</p>'
                + '<p class="delivery-date">Delivery Date: ' + sale.deliveryDate + '</p>'
                + '<p class="order-date">Order Date: ' + sale.orderDate + '</p>'
                + '<p class="products">Products:</p>'
                + '<ul class="products-list">';
                    + '<li> Product: 1 | Quantity: 100</li>'
                    + '<li> Product: 2 | Quantity: 200</li>';

    var products = sale.products;

    for(var i = 0; i < products.length; i++)
        html += '<li> Product: ' + products[i].productId + ' | Quantity: ' + products[i].quantity + '</li>';

    html += 
                '</ul>'          
            + '</div>';

    $('.left-col').html(html);
}

function getSalesRepSalesOrders(repId) {

    $.ajax({
        type: 'GET',
        url: 'http://localhost:49822/api/vendedores/' + repId.toString() + '/orders',
        success: getSalesRepSalesOrdersHandler,
        error: function () {
            console.log("Request failed!");
        }
    });
}

function getSalesRepSalesOrdersHandler(data) {

    // debug
    console.log("Request Successful!");
    console.log(data);

    repNumber = data.length;

    var html = '';
    for(var i = 0; i < data.length; i++)
    {
        var order = data[i];

        html +=
            '<div class="sale-item" id="' + order.salesOrderId + '">'
                + '<div class="sale-item-text">'
                    + '<h4 class="sale-item-name">' + order.customerId + '</h4>'
                        + '<div class="sale-information">'
                            + '<span class="sale-quantity">Delivered: ' + order.deliveryDate.toString() + ' </span>'
                            + '<span class="sale-date">Placed: ' + order.orderDate.toString() + '</span>'
                        + '</div>'
                + '</div>'
            + '</div>';
    }


    $('.sale-products-list').html(html);

}

function getSalesRepById(id) {
        // ajax request to the RESTful web service
    $.ajax({
        type: 'GET',
        url: 'http://localhost:49822/api/vendedores/' + id.toString(),
        success: getSalesRepByIdHandler,
        error: function () {
            console.log("Request failed!");
        }
    });
}



function getSalesRepByIdHandler(data) {

    $(".edit").removeClass('hidden');

    // debug
    console.log("Request Successful!");
    console.log(data);

    var rep = data;

    curID = rep.repId;

    var picture = rep.picture;
    var defaultPath = 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSf2u0RWmYALKJ431XNoTKjzu77ERLBIvXKlOEA-Q3DPo2h2rCB';

    //Checks if the source path is valid before setting the image
    if(picture != '')
    {
        var obj = new Image();
        obj.src = picture;
        if(!obj.complete){
            picture = defaultPath;
        }
    }
    else{
        picture = defaultPath;
    }

    $('.sale-rep-profile-image').attr('src', picture);

    curName = rep.name;
    $('.s-name').html(curName);   

    curAddress = rep.address;
    $('.s-address').html(curAddress);

    curPhone = rep.phone;
    var phone = 'Phone: ' + curPhone;
    $('.phone').html(phone);

    curEmail = rep.email;
    var email = 'Email: ' + curEmail;
    $('.email').html(email);

    curFiscal =  rep.fiscalId;
    var fiscal = 'Fiscal ID: ' + curFiscal;
    $('.fiscal-id').html(fiscal);

    curHired = rep.hiredDate.substring(0, 10);
    var hired = 'Hired: ' + curHired;
    $('.hired-date').html(hired);

    curBirth =  rep.birthDate.substring(0, 10);
    var birth = 'Birth: ' + curBirth;
    $('.birth-date').html(birth);

    curDesc = rep.description;
    $('.sale-rep-description-text').html(curDesc);
} 

/**
* Gets all reps and update html to show them
*/
function getAllSalesReps() {

    // ajax request to the RESTful web service
    $.ajax({
        type: 'GET',
        url: 'http://localhost:49822/api/vendedores/',
        success: getAllSalesRepsHandler,
        error: function () {
            console.log("Request failed!");
        }
    });
}


/**
* Updates html to show sales reps list
*
* @param data JSON response sent by the RESTful web service
*/
function getAllSalesRepsHandler(data) {

    // debug
    console.log("Request Successful!");
    console.log(data);
    
    for (i = 0; i < data.length; i++) {

        var rep = data[i];
        var html =
            '<div class="sale-rep-item" id="' + rep.repId +'">'
                + '<div class="sale-rep-item-text">'
                + '<h4 class="sale-rep-name">' + rep.name + '</h4>'
                        +  '<div class="sale-rep-information">'
                            + '<span class="sale-number"> Sales: ' + rep.sales + '</span>'
                        + '</div>'
                    + '</div>'
                + '</div>';

        $('.sale-rep-list').append(html);
    }

}





/**
* Main
*/
$(document).ready(function () {

    getAllSalesReps();

});

$(".sale-rep-list").click(function(event) {
    var rep = $(event.target);
	

    //Finds root element
    while(!rep.hasClass('sale-rep-item'))
        rep = rep.parent();

    //Resets previous clicked elements
    var active = $(".sale-rep-list, .sale-products-list").find('.active');
    for(var i = 0; i < active.length; i++)
        $(active[i]).removeClass('active');

    //Sets current element as active
    rep.addClass('active');

    var id = rep.attr('id')
    getSalesRepById(id);
    getSalesRepSalesOrders(id);

});


$(".sale-products-list").click(function(event) {
    var sale = $(event.target);
    

    //Finds root element
    while(!sale.hasClass('sale-item'))
        sale = sale.parent();

    //Resets previous clicked elements
    var active = $(".sale-products-list").find('.active');
    for(var i = 0; i < active.length; i++)
        $(active[i]).removeClass('active');

    //Sets current element as active
    sale.addClass('active');

    var id = sale.attr('id')

    getSaleById(id);

});

$(".edit").click(function(event) {

    adding = false;

    $('.edit-name').val(curName);
    $('.edit-address').val(curAddress);
    $('.edit-phone').val(curPhone);
    $('.edit-email').val(curEmail);
    $('.edit-fiscal').val(curFiscal);
    $('.edit-hired').val(curHired);
    $('.edit-birth').val(curBirth);
    $('.edit-desc').val(curDesc);

});

$(".add").click(function(event) {

    adding = true;

    $('.edit-name').val('');
    $('.edit-address').val('');
    $('.edit-phone').val('');
    $('.edit-email').val('');
    $('.edit-fiscal').val('');
    $('.edit-hired').val('');
    $('.edit-birth').val('');
    $('.edit-desc').val('');

    $('#myModal').modal('show'); 

});

$(".confirm").click(function(event) {

    var name = $('.edit-name').val();
    var address = $('.edit-address').val();
    var phone = $('.edit-phone').val();
    var email = $('.edit-email').val();
    var fiscal = $('.edit-fiscal').val();
    var hired = $('.edit-hired').val();
    var birth = $('.edit-birth').val();
    var desc = $('.edit-desc').val();


    if(adding)
        addSalesRep(name, address, phone, email, fiscal, hired, birth, desc);
    else
        editSalesRep(curID, name, address, phone, email, hired, birth, desc);

});