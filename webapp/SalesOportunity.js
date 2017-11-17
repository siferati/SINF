
function getOportunityById(id) {

    // ajax request to the RESTful web service
    $.ajax({
        type: 'GET',
        url: 'http://localhost:49822/api/oportunidadeVenda/' + id.toString(),
        success: getOportunityByIdHandler,
        error: function () {
            console.log("Request failed!");
        }
    });
}

function getOportunityByIdHandler(data){
	console.log(data);

	var sale = data;

	var html =
		'<p> Name'


}


function getAllOportunities() {

    // ajax request to the RESTful web service
    $.ajax({
        type: 'GET',
        url: 'http://localhost:49822/api/oportunidadeVenda',
        success: getAllOportunitiesHandler,
        error: function () {
            console.log("Request failed!");
        }
    });
}


function getAllOportunitiesHandler(data) {

    console.log("Request Successful!");
    console.log(data);


    var html ='';
    for(var i = 0; i < data.length; i++){
    	var sale = data[i];

    	html +=
    		'<div class="sales-oportunities-item" id?"' + sale.OportunidadeID + '">'
    			+'<p> Name: ' + sale.Nome + '</p>'
    			+'<p> Phone: ' + sale.Telemovel + '</p>'
    			+'<p> Data: ' + sale.Data + '</p>'
    		+'</div>'

    }


	$('.sales-oportunities').html(html);
}

/**
* Main
*/
$(document).ready(function () {

    getAllOportunities();

});

$(".sales-oportunities").click(function(event) {
    var sale = $(event.target);
    

    //Finds root element
    while(!sale.hasClass('sales-oportunities-item'))
        sale = sale.parent();

    //Resets previous clicked elements
    var active = $(".sales-oportunities").find('.active');
    for(var i = 0; i < active.length; i++)
        $(active[i]).removeClass('active');

    //Sets current element as active
    sale.addClass('active');

    var id = sale.attr('id')

    console.log(id);
});