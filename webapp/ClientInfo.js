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

    // debug
    console.log("Request Successful!");
    console.log(data);

    var client = data;

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

    var html = 
            '<div class= "client-header">'
                + '<div class= "client-image">'
                    + '<img class= "client-profile-image" src= "' + picture + '" alt="Profile Image">'
                + '</div>'
                + '<div class= "client-general-info">'            
                    + '<h4 class= "c-name">' + client.name.substring(0, 31) + '</h4>'
                    + '<p class="c-address"> Address: ' + client.address + '</p>'
                + '</div>'
            + '</div>'
            + '<div class="client-info">'        
                + '<p class="phone"> Phone: ' + client.phone + '</p>'
                + '<p class="email"> Email: ' + client.email + '</p>'
                + '<p class="fiscal-id"> Fiscal ID: ' + client.fiscalId + '</p>'
                + '<div class="client-info-last-row">'
                    + '<p class="birth-date"> Birth Date: ' + client.birthDate + '</p>'
                    + '<button type="button" class="btn btn-default edit-button">Edit</button>'
                + '</div>'
            + '</div>'
            + '<div class="client-description">'
                + '<h3 class="client-description-title">Description</h3>'
                + '<p class="client-description-text">' + client.description + '</p>'
            + '</div>';

    $('.left-col').html(html);           
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
                + '<h4 class="client-name">' + client.customerId + '</h4>'
                        +  '<div class="client-information">'
                            + '<span class="sale-number"> Orders: ' + client.orders + '</span>'
                        + '</div>'
                    + '</div>'
                + '</div>';

        $('.client-list').append(html);
    }

}





/**
* Main
*/
$(document).ready(function () {

    getAllCustomer();
	//HARDCODED EXAMPLE
	getCustomerOrders('SOLUCAO-Z');
	getCustomerById('SOLUCAO-Z');

});

$(".customer-list").click(function(event) {
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
