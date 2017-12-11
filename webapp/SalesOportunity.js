var pairsDateId = [];
var currentDataFetched;


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
	console.log("Request Successful!");
    console.log(data);

    currentDataFetched = data;

}


function prepCalendarData(){

    var data = [];

    for(var i = 0; i < pairsDateId.length; i++)
    {   
        var obj = pairsDateId[i];

        data.push({
            date: obj.date.toString(),
            badge: true,
            title: "-",
            body: "-",
            footer:"-",
            classname:"orange-event"
        })

    }

    console.log(data);
    return data;

}

function createCalendar(){
    var calendarData = prepCalendarData();
    $("#my-calendar").zabuto_calendar({language: "en", data: calendarData});
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

    for(var i = 0; i < data.length; i++)
    {
        var date = data[i].Data;
        date = date.substring(0, 10);
        var id = data[i].OportunidadeID;
        pairsDateId.push({date, id});
    }

    console.log(pairsDateId);
    var test = data[1].Data;

    for(var i = 0; i < pairsDateId.length; i++)
    {   
        var obj = pairsDateId[i];
        console.log(obj);
    }

    createCalendar();

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