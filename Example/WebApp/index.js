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
            '<li class="product">(' + product.CodArtigo + ') ' + product.DescArtigo +
                '<ul class="product-info">' +
                    '<li class="product-info-item">Stock: ' + product.STKAtual + '</li>' +
                '</ul>' +
            '</li>';

        $('.product-list').append(html);
    }

    $('.test').append(data[0].CodArtigo);
}


/**
* Main
*/
$(document).ready(function () {

    $(".btn").click(function () {

        getAllProducts();
    });

});