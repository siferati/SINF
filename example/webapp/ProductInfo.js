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
            '<div class="product-item">'
                + '<div class="product-item-text">'
                + '<h4 class="product-name">' + product.DescArtigo + '</h4>'
                        +  '<div class="product-information">'
                            + '<span class="product-size"> Size: % </span>'
                            + '<span class="product-stock"> Stock: ' + product.STKAtual + ' </span>'
                            + '<span class="product-price"> Price: % </span>'
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