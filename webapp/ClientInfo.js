var curName;
var curAddress;
var curPhone;
var curEmail;
var curFiscal;
var curBirth;
var curHired;
var curDesc;
var curID;
var htmlBuffer;
var adding = false;

function addClient(name, address, phone, email, fiscal, desc) {


var jsonName = name.toString();
var jsonAddress = address.toString();
var jsonPhone = phone.toString();
var jsonEmail = email.toString();
var jsonFiscal = fiscal.toString();
var jsonDesc = desc.toString();

$.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        url: 'http://localhost:49822/api/clientes/',
        
        data: JSON.stringify ({
                "fiscalID": jsonFiscal,
                "name": jsonName,
                "address": jsonAddress,
                "phone": jsonPhone,
                "email": jsonEmail,
                "description": jsonDesc
            }),
        success: function (data) {
            console.log("Request succeded!");
            alert("Client Added");
            window.location.reload(true);
        },
        error: function (data, textStatus) {
            console.log("Request failed!");
            alert(textStatus);
        }
    });
}

function editClient(id, name, address, phone, email, desc) {

var jsonID = id.toString();
var jsonName = name.toString();
var jsonAddress = address.toString();
var jsonPhone = phone.toString();
var jsonEmail = email.toString();
var jsonDesc = desc.toString();

$.ajax({
        type: 'PUT',
        dataType: 'json',
        contentType: 'application/json',
        url: 'http://localhost:49822/api/clientes/' + jsonID,
        
        data: JSON.stringify ({
                "name": jsonName,
                "address": jsonAddress,
                "phone": jsonPhone,
                "email": jsonEmail,
                "description": jsonDesc
            }),
        success: function (data) {
            console.log("Request succeded!");
            alert("Client Edited");
            window.location.reload(true);
        },
        error: function (data, textStatus) {
            console.log("Request failed!");
            alert(textStatus);
        }
    });
}

function getOrderById(id){


    $.ajax({
        type: 'GET',
        url: 'http://localhost:49822/api/orders/' + id.toString(),
        success: getOrderByIdHandler,
        error: function () {
            console.log("Request failed!");
        }
    });
}

function getOrderByIdHandler(data) {
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

function getCustomerOrders(id) {

    $.ajax({
        type: 'GET',
        url: 'http://localhost:49822/api/clientes/'+ id.toString() +'/orders',
        success: getCustomerOrdersHandler,
        error: function () {
            console.log("Request failed!");
        }
    });
}

function getCustomerOrdersHandler(data) {

    // debug
    console.log("Request Successful!");
    console.log(data);

    var html = '';
    for(var i = 0; i < data.length; i++)
    {
        var order = data[i];

        html +=
            '<div class="order-item" id="' + order.salesOrderId + '">'
                + '<div class="order-item-text">'
                    + '<h4 class="order-item-name">' + order.customerId + '</h4>'
                        + '<div class="order-information">'
                            + '<span class="order-quantity">Delivered: ' + order.deliveryDate.toString() + ' </span>'
                            + '<span class="order-date">Ordered: ' + order.orderDate.toString() + '</span>'
                        + '</div>'
                + '</div>'
            + '</div>';
    }


    $('.order-products-list').html(html);

}

function getCustomerById(id) {
        // ajax request to the RESTful web service
    $.ajax({
        type: 'GET',
        url: 'http://localhost:49822/api/clientes/' + id.toString(),
        success: getCustomerByIdHandler,
        error: function () {
            console.log("Request failed!");
        }
    });
}



function getCustomerByIdHandler(data) {

    if(htmlBuffer != null);{
        console.log("NULL");
        $('.left-col').html(htmlBuffer);
    }

        $(".edit").removeClass('hidden');


    // debug
    console.log("Request Successful!");
    console.log(data);

    var client = data;

    curID = client.customerId;

    var picture = client.picture;
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

    $('.client-profile-image').attr('src', picture);

    curName = client.name;
    $('.c-name').html(curName);   

    curAddress = client.address;
    $('.c-address').html(curAddress);

    curPhone = client.phone;
    var phone = 'Phone: ' + curPhone;
    $('.phone').html(phone);

    curEmail = client.email;
    var email = 'Email: ' + curEmail;
    $('.email').html(email);

    curFiscal =  client.fiscalId;
    var fiscal = 'Fiscal ID: ' + curFiscal;
    $('.fiscal-id').html(fiscal);

    curDesc = client.description;
    $('.client-description-text').html(curDesc);

    htmlBuffer = $('.left-col').html();

	
} 

/**
* Gets all reps and update html to show them
*/
function getAllCustomer() {

    // ajax request to the RESTful web service
    $.ajax({
        type: 'GET',
        url: 'http://localhost:49822/api/clientes',
        success: getAllCustomerHandler,
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
function getAllCustomerHandler(data) {

    // debug
    console.log("Request Successful!");
    console.log(data);
    
    for (i = 0; i < data.length; i++) {

        var client = data[i];
        var html =
            '<div class="client-item" id="' + client.customerId +'">'
                + '<div class="client-item-text">'
                + '<h4 class="client-name">' + client.name + '</h4>'
                        +  '<div class="client-information">'
                            + '<span class="sale-number"> Orders: ' + client.orders + '</span>'
                        + '</div>'
                    + '</div>'
                + '</div>';

        $('.client-list').append(html);
    }

}

function editCustomer(id, name, address, phone, email, description) {

    $.ajax({
        type: 'PUT',
		contentType: 'application/json',
        url: 'http://localhost:49822/api/clientes/' + id.toString(),
		
		data: {
				"address": address.toString(),
				"description": description.toString(),
				"email": email.toString(),
				"name": name.toString(),
				"phone": phone.toString()

			},
        success: function (data) {
            console.log("Request successeded!");
			alert("OK " + data);
        },
        error: function (data, textStatus) {
            console.log("Request failed!");
			alert(textStatus);
        }
    });
	
}


/**
* Main
*/
$(document).ready(function () {

    getAllCustomer();
	var help;
});

$(".client-list").click(function(event) {
    var rep = $(event.target);
	

    //Finds root element
    while(!rep.hasClass('client-item'))
        rep = rep.parent();

    //Resets previous clicked elements
    var active = $(".client-list, .order-products-list").find('.active');
    for(var i = 0; i < active.length; i++)
        $(active[i]).removeClass('active');

    //Sets current element as active
    rep.addClass('active');

    var id = rep.attr('id')
	help=id;
    getCustomerById(id);
    getCustomerOrders(id);

});


$(".order-products-list").click(function(event) {
    var sale = $(event.target);
    

    //Finds root element
    while(!sale.hasClass('order-item'))
        sale = sale.parent();

    //Resets previous clicked elements
    var active = $(".order-products-list").find('.active');
    for(var i = 0; i < active.length; i++)
        $(active[i]).removeClass('active');

    //Sets current element as active
    sale.addClass('active');

    var id = sale.attr('id')

    getOrderById(id);

});

//editing clients name
$('.c-name').click(function(){
    var old_input = $(this).text();
    $(this).html('');
    $('<input></input>')
        .attr({
            'type': 'text',
            'id': 'txt_name',
            'value': old_input
        })
        .appendTo('.c-name');
    $('#txt_name').focus();
});

$(document).on('blur','#txt_name', function(){
    var new_input = $(this).val();
	if(new_input != ''){
		$('.c-name').text(new_input);
	}else{
		$('.c-name').text("undefined");
	}
});

//editing clients address
$('.c-address').click(function(){
    var old_input = $(this).text();
    $(this).html('');
    $('<input></input>')
        .attr({
            'type': 'text',
            'id': 'txt_address',
            'value': old_input
        })
        .appendTo('.c-address');
    $('#txt_address').focus();
});

$(document).on('blur','#txt_address', function(){
    var new_input = $(this).val();
	if(new_input != ''){
		$('.c-address').text(new_input);
	}else{
		$('.c-address').text("undefined");
	}
});

//editing clients phone
$('.phone').click(function(){
    var old_input = $(this).text();
    $(this).html('');
    $('<input></input>')
        .attr({
            'type': 'text',
            'id': 'txt_phone',
            'value': old_input
        })
        .appendTo('.phone');
    $('#txt_phone').focus();
});

$(document).on('blur','#txt_phone', function(){
    var new_input = $(this).val();
	if(new_input != ''){
		$('.phone').text(new_input);
	}else{
		$('.phone').text("undefined");
	}
    
});

//editing clients email
$('.email').click(function(){
    var old_input = $(this).text();
    $(this).html('');
    $('<input></input>')
        .attr({
            'type': 'text',
            'id': 'txt_email',
            'value': old_input
        })
        .appendTo('.email');
    $('#txt_email').focus();
});

$(document).on('blur','#txt_email', function(){
    var new_input = $(this).val();
	if(new_input != ''){
		$('.email').text(new_input);
	}else{
		$('.email').text("undefined");
	}
    
});

//editing clients description
$('.client-description-text').click(function(){
    var old_input = $(this).text();
    $(this).html('');
    $('<input></input>')
        .attr({
            'type': 'text',
            'id': 'txt_description',
            'value': old_input
        })
        .appendTo('.client-description-text');
    $('#txt_description').focus();
});

$(document).on('blur','#txt_description', function(){
    var new_input = $(this).val();
	if(new_input != ''){
		$('.client-description-text').text(new_input);
	}else{
		$('.client-description-text').text("No description available");
	}
    
});


$('.client-info-last-row button').click(function(){
    var name = $('.c-name').text();
	var address = $('.c-address').text();
	var phone = $('.phone').text();
	var email = $('.email').text();
	
    if(name != "undefined" && address != "undefined" && phone != "undefined" 
		&& email != "undefined"){
				
		var description = $('.sale-rep-description-text').text();	
		editCustomer(help, name, address, phone, email, description );
	}else{
		alert("Please fill all the data!");
	}
});


$(".edit").click(function(event) {

    adding = false;

    $('.edit-name').val(curName);
    $('.edit-address').val(curAddress);
    $('.edit-phone').val(curPhone);
    $('.edit-email').val(curEmail);
    $('.edit-fiscal').val(curFiscal);
    $('.edit-desc').val(curDesc);

});

$(".add").click(function(event) {

    adding = true;

    $('.edit-name').val('');
    $('.edit-address').val('');
    $('.edit-phone').val('');
    $('.edit-email').val('');
    $('.edit-fiscal').val('');
    $('.edit-desc').val('');

    $('#myModal').modal('show'); 

});

$(".confirm").click(function(event) {

    var name = $('.edit-name').val();
    var address = $('.edit-address').val();
    var phone = $('.edit-phone').val();
    var email = $('.edit-email').val();
    var fiscal = $('.edit-fiscal').val();
    var desc = $('.edit-desc').val();



    if(adding)
        addClient(name, address, phone, email, fiscal, desc);
    else
        editClient(curID, name, address, phone, email, desc);

});
