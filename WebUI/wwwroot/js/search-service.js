var placeTypes = {
    "Compartment": 0,
    "Public": 1,
    "Luxe": 2,
    "S1": 3,
    "S2": 4
};
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
        url: "https://localhost:8081/api/Auth/token",
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
        url: "https://localhost:8081/api/Route/search",
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
        }
    });
    return arr;
}
function getNumberOfPlaces(trainCarId, placeType) {
    var count;
    var token = getToken({
        clientId: "m2m.client",
        clientSecret: "SuperSecretPassword",
        scope: "railwaytickets.read"
    });
    $.ajax({
        type: "GET",
        async: false,
        url: "https://localhost:8081/api/Place/".concat(trainCarId, "/").concat(placeType, "/count"),
        contentType: 'application/json; charset=utf-8',
        headers: {
            "Authorization": "Bearer " + token
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (result) {
            count = result;
        }
    });
    return count;
}
function displaySearched(form) {
    var languageCode = window.location.pathname.split("/")[1];
    var dto = convertFormToDto(form);
    var data = searchRoutes(dto);
    var trains;
    var list = "\n        <ul style=\"list-style-type: none; padding: 0; margin: 0; width: 80%;\">\n    ";
    data.map(function (route) {
        var compartmentCount = 0;
        var publictCount = 0;
        var luxeCount = 0;
        var s1Count = 0;
        var s2Count = 0;
        trains = "";
        route.trains.map(function (train) {
            train.trainCars.map(function (trainCar) {
                compartmentCount += getNumberOfPlaces(trainCar.id, placeTypes["Compartment"]);
                publictCount += getNumberOfPlaces(trainCar.id, placeTypes["Public"]);
                luxeCount += getNumberOfPlaces(trainCar.id, placeTypes["Luxe"]);
                s1Count += getNumberOfPlaces(trainCar.id, placeTypes["S1"]);
                s2Count += getNumberOfPlaces(trainCar.id, placeTypes["S2"]);
            });
        });
        list += "\n            <li style=\"width: 110%\">\n                <div class=\"resultitem\">\n                    <span>".concat(route.fromCity, "</span>\n                    <span>").concat(route.toCity, "</span>\n                    <span>").concat(new Date(route.departureTime.toString()).toLocaleTimeString(), "</span>\n                    <span>").concat(new Date(route.arrivalTime.toString()).toLocaleTimeString(), "</span>\n                    <span>").concat(route.travelTime, "</span> \n                    <div>\n                        <ul style=\"list-style-type: none; padding: 0; margin: 0;\">\n                            <li>Compartment - ").concat(compartmentCount, "</li>\n                            <li>Public - ").concat(publictCount, "</li>\n                            <li>Luxe - ").concat(luxeCount, "</li>\n                            <li>S1 - ").concat(s1Count, "</li>\n                            <li>S2 - ").concat(s2Count, "</li>\n                        </ul>\n                    </div>\n                    <div style=\"width: 20%;\">\n                        <ul id=\"trains\" style=\"list-style-type: none; padding: 0; margin: 0;\">\n                            ").concat(route.trains.map(function (train) {
            trains +=
                "\n                                        <li style=\"display: flex; align-items: center; justify-content: space-between;\">\n                                            <span>".concat(train.number, "</span>\n                                            <button class=\"btn btn-warning\" style=\"margin-left: 0.5%\" onclick=\"window.location.href='/").concat(languageCode, "/Home/Book?trainId=").concat(train.id, "'\">\n                                                ").concat(languageCode === "uk" ? "Забронювати" : languageCode === 'en' ? "Book" : "", "\n                                            </button>\n                                        </li>  \n                                    ");
        }), "\n                            ").concat(trains, "\n                        </ul>\n                    </div>\n                </div>\n            </li>");
    });
    list += "</ul>";
    document.getElementById("resultholder").innerHTML = list;
}
//# sourceMappingURL=search-service.js.map