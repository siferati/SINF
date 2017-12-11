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

    // debug
    console.log("Request Successful!");
    console.log(data);

    var rep = data;

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
/**
    var html = 
            '<div class= "sale-rep-header">'
                + '<div class= "sale-rep-image">'
                    + '<img class= "sale-rep-profile-image" src= "' + picture + '" alt="Profile Image">'
                + '</div>'
                + '<div class= "sale-rep-general-info">'            
                    + '<h4 class= "s-name">' + rep.name.substring(0, 31) + '</h4>'
                    + '<p class="s-address">' + rep.address + '</p>'
                    + '<p class="s-zip">' + rep.zip + '</p>'
                + '</div>'
            + '</div>'
            + '<div class="sale-rep-info">'        
                + '<p class="phone"> Phone: ' + rep.phone + '</p>'
                + '<p class="email"> Email: ' + rep.email + '</p>'
                + '<p class="fiscal-id"> Fiscal ID: ' + rep.fiscalID + '</p>'
                + '<div class="sale-rep-info-last-row">'
                    + '<p class="birth-date"> Birth Date: ' + rep.birthDate + '</p>'
                    + '<button id="btn" type="button" class="btn btn-default edit-button">Edit</button>'
                + '</div>'
            + '</div>'
            + '<div class="sale-rep-description">'
                + '<h3 class="sale-rep-description-title">Description</h3>'
                + '<p class="sale-rep-description-text">' + rep.description + '</p>'
            + '</div>';
	$('.left-col').html(html);
*/

	if(rep.name.substring(0, 31)) {$('.s-name').text(rep.name.substring(0, 31));}
	else {$('.s-name').text("Name: undefined");}
	
	if(rep.address.length != 0) {$('.s-address').text(rep.address);}
	else {$('.s-address').text("undefined");}
	
	if(rep.phone.length != 0) {$('.phone').text(rep.phone);}
	else {$('.phone').text("undefined");}
	
	if(rep.email.length != 0) {$('.email').text(rep.email);}
	else {$('.email').text("undefined");}
	
	if(rep.fiscalId.length != 0) {$('.fiscal-id').text(rep.fiscalId);}
	else {$('.fiscal-id').text("undefined");}
	
	if(rep.birthDate.length != 0) {$('.birth-date').text(rep.birthDate);}
	else {$('.birth-date').text("undefined");}
	
	if(rep.hiredDate.length != 0) {$('.hired-date').text(rep.hiredDate);}
	else {$('.hired-date').text("undefined");}
	
	if(rep.description.length != 0) {$('.sale-rep-description-text').text(rep.description);}
	else {$('.sale-rep-description-text').text("No description available");}
	
	
               
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

function editSaleRep(id, name, address, phone, email, hired, birthday, description) {

    $.ajax({
        type: 'PUT',
		contentType: 'application/json',
        url: 'http://localhost:49822/api/vendedores/3',
		data: {
			"name": name.toString(),
			"address": address.toString(),
			"phone": phone.toString(),
			"email": email.toString(),
			"hiredDate": hired.toString(),
			"birthDate": birthday.toString(),
			"description": description.toString()

		},
        success: function (data) {
            console.log("Request successeded!");
			alert("OK " + data);
        },
        error: function (jqXHR, textStatus) {
            console.log("Request failed!");
			alert(textStatus + jqXHR.responseText);
        }
    });
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

//editing sale rep name
$(".s-name").click(function(event){
    var old_input = $(this).text();
    $(this).html('');
    $('<input></input>')
        .attr({
            'type': 'text',
            'id': 'txt_name',
            'value': old_input
        })
        .appendTo(".s-name");
    $('#txt_name').focus();
});

$(document).on('blur','#txt_name', function(){
    var new_input = $(this).val();
	if(new_input != ''){
		$('.s-name').text(new_input);
	}else{
		$('.s-name').text("undefined");
	}
    
});

//editing sale rep address
$(".s-address").click(function(event){
    var old_input = $(this).text();
    $(this).html('');
    $('<input></input>')
        .attr({
            'type': 'text',
            'id': 'txt_address',
            'value': old_input
        })
        .appendTo(".s-address");
    $('#txt_address').focus();
});

$(document).on('blur','#txt_address', function(){
    var new_input = $(this).val();
	if(new_input != ''){
		$('.s-address').text(new_input);
	}else{
		$('.s-address').text("undefined");
	}
    
});

//editing sale rep phone
$(".phone").click(function(event){
    var old_input = $(this).text();
    $(this).html('');
    $('<input></input>')
        .attr({
            'type': 'text',
            'id': 'txt_phone',
            'value': old_input
        })
        .appendTo(".phone");
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

//editing sale rep email
$(".email").click(function(event){
    var old_input = $(this).text();
    $(this).html('');
    $('<input></input>')
        .attr({
            'type': 'text',
            'id': 'txt_email',
            'value': old_input
        })
        .appendTo(".email");
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

//editing sale rep birthdate
$(".birth-date").click(function(event){
    var old_input = $(this).text();
    $(this).html('');
    $('<input></input>')
        .attr({
            'type': 'text',
            'id': 'txt_birthdate',
            'value': old_input
        })
        .appendTo(".birth-date");
    $('#txt_birthdate').focus();
});

$(document).on('blur','#txt_birthdate', function(){
    var new_input = $(this).val();
	if(new_input != ''){
		$('.birth-date').text(new_input);
	}else{
		$('.birth-date').text("undefined");
	}
    
});

//editing sale rep hired date
$(".hired-date").click(function(event){
    var old_input = $(this).text();
    $(this).html('');
    $('<input></input>')
        .attr({
            'type': 'text',
            'id': 'txt_hireddate',
            'value': old_input
        })
        .appendTo(".hired-date");
    $('#txt_hireddate').focus();
});

$(document).on('blur','#txt_hireddate', function(){
    var new_input = $(this).val();
	if(new_input != ''){
		$('.hired-date').text(new_input);
	}else{
		$('.hiredh-date').text("undefined");
	}
    
});

//editing sale rep description
$(".sale-rep-description-text").click(function(event){
    var old_input = $(this).text();
    $(this).html('');
    $('<input></input>')
        .attr({
            'type': 'text',
            'id': 'txt_description',
            'value': old_input
        })
        .appendTo(".sale-rep-description-text");
    $('#txt_description').focus();
});

$(document).on('blur','#txt_description', function(){
    var new_input = $(this).val();
	if(new_input != ''){
		$('.sale-rep-description-text').text(new_input);
	}else{
		$('.sale-rep-description-text').text("No description available");
	}
    
});

$('.sale-rep-info-last-row button').click(function(){
	
	var name = $('.s-name').text();
	var address = $('.s-address').text();
	var phone = $('.phone').text();
	var email = $('.email').text();
	var hired = $('.hired-date').text();
	var birthday = $('.birth-day').text();
	
    if(name != "undefined" && address != "undefined" && phone != "undefined" 
		&& email != "undefined" && hired != "undefined" && birthday != "undefined"){
				
		var description = $('.sale-rep-description-text').text();	
		editSaleRep(3, name, address, phone, email, hired, birthday, description );
	}else{
		alert("Please fill all the data!");
	}
	
});
