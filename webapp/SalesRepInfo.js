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
                    + '<button type="button" class="btn btn-default edit-button">Edit</button>'
                + '</div>'
            + '</div>'
            + '<div class="sale-rep-description">'
                + '<h3 class="sale-rep-description-title">' +  rep.description + '</h3>'
                + '<p class="sale-rep-description-text"> Good employee. Likes this company.</p>'
            + '</div>';

    $('.left-col').html(html);           
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


    //HARDCODED EXAMPLE
    getSalesRepSalesOrders(3);

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
