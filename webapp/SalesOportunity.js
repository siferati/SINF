var pairsDateId = [];
var adding = false;

function fillEntityOptions(id) {

    // ajax request to the RESTful web service
    $.ajax({
        type: 'GET',
        url: 'http://localhost:49822/api/clientes/',
        success: fillEntityOptionsHandler,
        error: function () {
            console.log("Request failed!");
        }
    });
}

function fillEntityOptionsHandler(data) {

    console.log(data);

    var html = '';

    for(var i = 0; i < data.length; i++){
        html = html + '<option value="' + data[i].customerId + '">' + data[i].name + '</option>';
    }

    $(".details-entity").append(html);


}

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
            '<div class="event-item col-md-12" id="' + data.OportunidadeID + '">'
                + '<div class="col-md-6 event-item-text-left">'
                    + '<p class="op-name"> Oportunidade: ' + data.DescricaoOp + '</p>'
                    + '<p class="op-entity"> Entidade: ' + data.Entidade + '</p>'
                + '</div>'
                + '<div class="col-md-6 event-item-text-right">'
                    + '<p class="op-hour"> Hora:  ' + data.Data.substring(11,19) + '</p>'
                    + '<p class="op-location"> Local:  ' + data.Local + '</p>'
                + '</div>'
            + '</div>';

    $('.events-list').append(html);
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

$(".events-list").click(function(event) {
    var event = $(event.target);
    

    //Finds root element
    while(!event.hasClass('event-item'))
        event = event.parent();

    //Resets previous clicked elements
    adding = false;
    var active = $(".events-list").find('.active');
    for(var i = 0; i < active.length; i++)
        $(active[i]).removeClass('active');

    //Sets current element as active
    event.addClass('active');

    var id = event.attr('id')

    if(id == "add-task")
        adding = true;
    else
        displayOportunityDetails(id);

    console.log(adding);
});



function convertToForm(div){
    var divHtml = $(div).html(); 

    var editableText;

    if($(div).hasClass("details-entity")){
        editableText = $("<select class=\"col-md-12 edited details-entity\"/>");
        fillEntityOptions();
    }

    if($(div).hasClass("details-name"))
        editableText = $("<input type=\"text\" name=\"name\" class=\"col-md-12 edited details-name\"/>");


    if($(div).hasClass("details-date"))
        editableText = $(" <input type=\"date\" name=\"date\" class=\"col-md-12 edited details-date\">");

    if($(div).hasClass("details-time"))
        editableText = $(" <input id=\"time\" type=\"time\" name=\"time\" class=\"col-md-12 edited details-time\">");

    if($(div).hasClass("details-location"))
        editableText = $("<input type=\"text\" name=\"location\" class=\"col-md-12 edited details-location\"/>");


    editableText.val(divHtml);
    $(div).replaceWith(editableText);
    editableText.focus();
}

$(".edit").click(function() {

    var divs = $(document).find(".editable");

    for(var i = 0; i < divs.length; i++)
        convertToForm(divs[i]);

});

$(".confirm").click(function() {

    var name = $(document).find(".details-name").val();
    var date = $(document).find(".details-date").val();
    var time = $(document).find(".details-time").val();
    var location = $(document).find(".details-location").val();
    var entity = $(document).find(".details-entity").val();

    console.log(name);
    console.log(date);
    console.log(time);
    console.log(location);
    console.log(entity);

});

