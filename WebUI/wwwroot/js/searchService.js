function convertFormToDto(form) {
    return {
        fromCity: form.elements.item(0).value,
        toCity: form.elements.item(1).value,
        departureDate: new Date(form.elements.item(2).value),
    };
}
function getToken(dto) {
    var token;
    $.ajax({
        type: "POST",
        async: false,
        url: "https://localhost:7250/api/Auth/token",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(dto),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (result) {
            token = result;
        }
    });
    return token;
}
function searchRoutes(dto) {
    var arr;
    var token = getToken({
        clientId: "m2m.client",
        clientSecret: "SuperSecretPassword",
        scope: "railwaytickets.read"
    });
    $.ajax({
        type: "POST",
        async: false,
        url: "https://localhost:7250/api/Route/search",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(dto),
        dataType: 'json',
        headers: {
            "Authorization": "Bearer " + token
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (result) {
            arr = JSON.parse(JSON.stringify(result));
            console.log(arr);
        }
    });
    return arr;
}
function displaySearched(form) {
    var dto = convertFormToDto(form);
    var data = searchRoutes(dto);
    var list = "\n        <ul style=\"list-style-type: none; padding: 0; margin: 0;\">\n    ";
    data.map(function (route) {
        list += "<li>".concat(route.fromCity, " - ").concat(route.toCity, " | ").concat(route.travelTime, "</li>");
    });
    list += "</ul>";
    document.getElementById("resultholder").innerHTML = list;
}
//# sourceMappingURL=searchService.js.map