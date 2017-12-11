var pairsDateId = [];


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

    var html = 
        '<div class="op-item" id="' + data.OportunidadeID + '">'
            + '<p class="descricao">' + data.DescricaoOp + '</p>'
            + '<p class="entidade"> Entidade: ' + data.Entidade + '</p>'
            + '<p class="hora"> Hora:' + data.Data.substring(11,19) + '</p>'
            + '<p class="local"> Local: ' + data.Local + '</p>'
        + '</div>';

    $('.events').append(html);


}

function showEventList(opIds){

    //Resets html --- the request will handle its reconstruction
    $(".events").html("");

    for(var i = 0; i < opIds.length; i++){
            getOportunityById(opIds[i]);
    }

}

function expandDate(id){
    var date = $("#" + id).data("date");
    var hasEvent = $("#" + id).data("hasEvent");

    console.log(date);

    if(hasEvent){
        var opIds = [];
        for(var i = 0; i < pairsDateId.length; i++)
        {   
            var obj = pairsDateId[i];

            if (obj.date == date)
                opIds.push(obj.id);
        }

        console.log(opIds);

        showEventList(opIds);
    }
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
    $("#my-calendar").zabuto_calendar({
        language: "en", 
        data: calendarData,
        action: function() { expandDate(this.id); }
    });
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

    //Resets data
    pairsDateId = [];

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

