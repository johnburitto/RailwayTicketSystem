var trainCarTypes = {
    "Compratment": 56,
    "Public": 64
};
var bookedPlacesButtons = [];
var bookedTickets = [];
var totalcost = 0;
function getTokenBS(dto) {
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
function getPlaces(trainCarId) {
    var arr;
    var token = getTokenBS({
        clientId: "m2m.client",
        clientSecret: "SuperSecretPassword",
        scope: "railwaytickets.read"
    });
    $.ajax({
        type: "GET",
        async: false,
        url: "https://localhost:8081/api/Place/".concat(trainCarId, "/places"),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        headers: {
            "Authorization": "Bearer " + token
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (result) {
            arr = result;
        }
    });
    return arr;
}
function bookTickets() {
    var token = getTokenBS({
        clientId: "m2m.client",
        clientSecret: "SuperSecretPassword",
        scope: "railwaytickets.write"
    });
    var languageCode = window.location.pathname.split("/")[1];
    $.ajax({
        type: "POST",
        async: false,
        url: "https://localhost:8081/api/Ticket/range",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(bookedTickets),
        dataType: "json",
        headers: {
            "Authorization": "Bearer " + token
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (result) {
            window.location.href = "/".concat(languageCode, "/Home/Index");
        }
    });
}
function generateTrainCarOverview(trainCarId, trainCarType, userId) {
    var places = getPlaces(trainCarId);
    var numberOfPlaces = trainCarTypes[trainCarType];
    var buttonWidth = 100 / (numberOfPlaces / 4 + 1);
    var buttonMargin = buttonWidth / (numberOfPlaces / 4);
    var numberOfPlacesHalf = numberOfPlaces / 2;
    if (!$("#placesholder").is(":visible")) {
        $("#placesholder").toggle();
    }
    if (!$("#bookinginfo").is(":visible")) {
        $("#bookinginfo").toggle();
    }
    if (bookedPlacesButtons[trainCarId] === undefined) {
        bookedPlacesButtons[trainCarId] = [];
    }
    document.getElementById("placesholder").innerHTML = "";
    console.log(bookedPlacesButtons[trainCarId]);
    var _loop_1 = function (i) {
        place = places.filter(function (place) { return place.number === i; })[0];
        if (i <= numberOfPlacesHalf) {
            document.getElementById("placesholder").innerHTML += "\n                <button id=\"".concat(i, "\" class=\"btn btn-primary ").concat(place === undefined || !place.isAvaliable || bookedPlacesButtons[trainCarId].indexOf(i.toString()) !== -1 ? "disabled" : "", "\" \n                                    style=\"width: ").concat(buttonWidth, "%; margin-right: ").concat(buttonMargin, "%; margin-bottom: ").concat(buttonMargin, "%;\n                                                  ").concat(place === undefined || !place.isAvaliable ? "background-color: gray; border-color: gray;" : "", "\"\n                                        onclick=\"addTicketToBookedList('").concat(userId, "', ").concat(place === undefined || !place.isAvaliable ? 0 : place.id, ", \n                                                                                   ").concat(place === undefined || !place.isAvaliable ? 0 : place.price, ", '").concat(i, "', ").concat(trainCarId, ")\">\n                    ").concat(i, "\n                </button>\n            ");
        }
        else {
            document.getElementById("placesholder").innerHTML += "\n                <button id=\"".concat(i, "\" class=\"btn btn-primary ").concat(place === undefined || !place.isAvaliable || bookedPlacesButtons[trainCarId].indexOf(i.toString()) !== -1 ? "disabled" : "", "\" \n                                    style=\"width: ").concat(buttonWidth, "%; margin-right: ").concat(buttonMargin, "%; margin-top: ").concat(buttonMargin, "%;\n                                                  ").concat(place === undefined || !place.isAvaliable ? "background-color: gray; border-color: gray;" : "", "\"\n                                        onclick=\"addTicketToBookedList('").concat(userId, "', ").concat(place === undefined || !place.isAvaliable ? 0 : place.id, ", \n                                                                                   ").concat(place === undefined || !place.isAvaliable ? 0 : place.price, ", '").concat(i, "', ").concat(trainCarId, ")\">\n                    ").concat(i, "\n                </button>\n            ");
        }
        if (i === numberOfPlacesHalf) {
            document.getElementById("placesholder").innerHTML += "<div style=\"width: 100%; margin-top: 3%; margin-bottom: 3%;\"></div>";
        }
    };
    var place;
    for (var i = 1; i <= numberOfPlaces; i++) {
        _loop_1(i);
    }
}
function addTicketToBookedList(userId, placeId, placeCost, buttonId, trainCarId) {
    if (bookedTickets.filter(function (dto) { return dto.placeId === placeId; })[0] === undefined) {
        bookedTickets.push({
            userId: userId,
            bookDate: new Date(),
            placeId: placeId
        });
        totalcost += placeCost;
        if (bookedPlacesButtons[trainCarId].filter(function (place) { return place === buttonId; })[0] === undefined) {
            bookedPlacesButtons[trainCarId].push(buttonId);
        }
        document.getElementById(buttonId).classList.add("disabled");
    }
    document.getElementById("totalnumber").innerHTML = bookedTickets.length.toString();
    document.getElementById("totalcost").innerHTML = totalcost.toString();
}
function clearBookedList() {
    bookedPlacesButtons.map(function (trainCarButtons) {
        trainCarButtons.map(function (buttonId) {
            document.getElementById(buttonId).classList.remove("disabled");
            document.getElementById(buttonId).style.backgroundColor = "#0d6efd";
            document.getElementById(buttonId).style.borderColor = "#0d6efd";
        });
    });
    bookedPlacesButtons.forEach(function (part, index) {
        this[index] = [];
    }, bookedPlacesButtons);
    bookedTickets = [];
    totalcost = 0;
    document.getElementById("totalnumber").innerHTML = bookedTickets.length.toString();
    document.getElementById("totalcost").innerHTML = totalcost.toString();
}
//# sourceMappingURL=booking-service.js.map