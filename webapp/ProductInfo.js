
function getProductById(id) {
        // ajax request to the RESTful web service
    $.ajax({
        type: 'GET',
        url: 'http://localhost:49822/api/artigos/' + id.toString(),
        success: getProductByIdHandler,
        error: function () {
            console.log("Request failed!");
        }
    });
}

function getProductByIdHandler(data) {

    // debug
    console.log("Request Successful!");
    //console.log(data);

    var product = data;
    var productInfo =
                '<p class="p-name">Name: ' + product.name + '</p>' +
                '<p class="p-type">Type: ' + product.type + '</p>' +
                '<p class="p-size">Size: ' + product.size + '</p>' +
                '<p class="p-price">Price: ' + product.price + '</p>' +
                '<p class="p-vat">Vat: ' + product.VAT + '</p>' +
                '<p class="p-stock">Stock: ' + product.stock + '</p>' +
                '<p class="p-wight">Weight: ' + product.weight + '</p>';


    $('.product-info').html(productInfo);


    var productDescription = product.description;

    $('.product-description-text').html(productDescription);
}

/**
* Gets all products and update html to show them
*/
function getAllProducts() {

    // ajax request to the RESTful web service
    $.ajax({
        type: 'GET',
        url: 'http://localhost:49822/api/artigos/',
        success: getAllProductsHandler,
        error: function () {
            console.log("Request failed!");
        }
    });
}


/**
* Updates html to show product list
*
* @param data JSON response sent by the RESTful web service
*/
function getAllProductsHandler(data) {

    // debug
    console.log("Request Successful!");
    console.log(data);
    
    for (i = 0; i < data.length; i++) {

        var product = data[i];
        var html =
            '<div class="product-item" id="' + product.productId + '">'
                + '<div class="product-item-text">'
                + '<h4 class="product-name">' + product.name + '</h4>'
                        +  '<div class="product-information">'
                            + '<span class="product-size"> Size: ' + product.size + ' </span>'
                            + '<span class="product-stock"> Stock: ' + product.stock + ' </span>'
                            + '<span class="product-price"> Price: ' + product.price + ' </span>'
                        + '</div>'
                    + '</div>'
                + '</div>';

        $('.product-list').append(html);
    }

}


/**
* Main
*/
$(document).ready(function () {

    getAllProducts();
    

});

$(".product-list").click(function(event) {
    var product = $(event.target);

    //Finds root element
    while(!product.hasClass('product-item'))
        product = product.parent();

    //Resets previous clicked elements
    var active = $(".product-list").find('.active');
    for(var i = 0; i < active.length; i++)
        $(active[i]).removeClass('active');

    //Sets current element as active
    product.addClass('active');

    var id = product.attr('id')
    getProductById(id);

});