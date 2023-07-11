interface Place {
    id: number;
    number: number;
    price: number;
    placeType: number;
    isAvaliable: boolean;
    trainCarId: number;
}

interface TicketCreateDto {
    userId: string;
    bookDate: Date;
    placeId: number;
}

var trainCarTypes: { [trainCarType: string]: number } = {
    "Compratment": 56,
    "Public": 64
}

var bookedPlacesButtons: Array<Array<string>> = [];
var bookedTickets: Array<TicketCreateDto> = [];
var totalcost: number = 0;

function getTokenBS(dto: GetTokenDto) {
    let token: string;

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

function getPlaces(trainCarId: number) {
    let arr: Array<Place>;
    let token = getTokenBS({
        clientId: `m2m.client`,
        clientSecret: `SuperSecretPassword`,
        scope: `railwaytickets.read`
    });

    $.ajax({
        type: "GET",
        async: false,
        url: `https://localhost:7250/api/Place/${trainCarId}/places`,
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
    let token = getTokenBS({
        clientId: `m2m.client`,
        clientSecret: `SuperSecretPassword`,
        scope: `railwaytickets.write`
    });
    let languageCode = window.location.pathname.split("/")[1];

    $.ajax({
        type: "POST",
        async: false,
        url: `https://localhost:7250/api/Ticket/range`,
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
            window.location.href = `/${languageCode}/Home/Index`
        }
    });
}

function generateTrainCarOverview(trainCarId: number, trainCarType: number, userId: string) {
    var places = getPlaces(trainCarId);
    var numberOfPlaces = trainCarTypes[trainCarType];
    let buttonWidth: number = 100 / (numberOfPlaces / 4 + 1);
    let buttonMargin = buttonWidth / (numberOfPlaces / 4);
    let numberOfPlacesHalf = numberOfPlaces / 2;

    if (!$("#placesholder").is(":visible")) {
        $("#placesholder").toggle();
    }
    if (!$("#bookinginfo").is(":visible")) {
        $("#bookinginfo").toggle();
    }
    if (bookedPlacesButtons[trainCarId] === undefined) {
        bookedPlacesButtons[trainCarId] = [];
    }

    document.getElementById("placesholder").innerHTML = ``;

    console.log(bookedPlacesButtons[trainCarId]);

    for (let i = 1; i <= numberOfPlaces; i++) {
        var place = places.filter(place => place.number === i)[0];

        if (i <= numberOfPlacesHalf) {
            document.getElementById("placesholder").innerHTML += `
                <button id="${i}" class="btn btn-primary ${place === undefined || !place.isAvaliable || bookedPlacesButtons[trainCarId].indexOf(i.toString()) !== -1 ? "disabled" : ""}" 
                                    style="width: ${buttonWidth}%; margin-right: ${buttonMargin}%; margin-bottom: ${buttonMargin}%;
                                                  ${place === undefined || !place.isAvaliable ? "background-color: gray; border-color: gray;" : ""}"
                                        onclick="addTicketToBookedList('${userId}', ${place === undefined || !place.isAvaliable ? 0 : place.id}, 
                                                                                   ${place === undefined || !place.isAvaliable ? 0 : place.price}, '${i}', ${trainCarId})">
                    ${i}
                </button>
            `;
        }
        else {
            document.getElementById("placesholder").innerHTML += `
                <button id="${i}" class="btn btn-primary ${place === undefined || !place.isAvaliable || bookedPlacesButtons[trainCarId].indexOf(i.toString()) !== -1 ? "disabled" : ""}" 
                                    style="width: ${buttonWidth}%; margin-right: ${buttonMargin}%; margin-top: ${buttonMargin}%;
                                                  ${place === undefined || !place.isAvaliable ? "background-color: gray; border-color: gray;" : ""}"
                                        onclick="addTicketToBookedList('${userId}', ${place === undefined || !place.isAvaliable ? 0 : place.id}, 
                                                                                   ${place === undefined || !place.isAvaliable ? 0 : place.price}, '${i}', ${trainCarId})">
                    ${i}
                </button>
            `;
        }

        if (i === numberOfPlacesHalf) {
            document.getElementById("placesholder").innerHTML += `<div style="width: 100%; margin-top: 3%; margin-bottom: 3%;"></div>`
        }
    }
}

function addTicketToBookedList(userId: string, placeId: number, placeCost: number, buttonId: string, trainCarId: number) {
    if (bookedTickets.filter(dto => dto.placeId === placeId)[0] === undefined) {
        bookedTickets.push({
            userId: userId,
            bookDate: new Date(),
            placeId: placeId
        });

        totalcost += placeCost;

        if (bookedPlacesButtons[trainCarId].filter(place => place === buttonId)[0] === undefined) {
            bookedPlacesButtons[trainCarId].push(buttonId);
        }

        document.getElementById(buttonId).classList.add("disabled");
    }

    document.getElementById("totalnumber").innerHTML = bookedTickets.length.toString();
    document.getElementById("totalcost").innerHTML = totalcost.toString();
}

function clearBookedList() {
    bookedPlacesButtons.map((trainCarButtons) => {
        trainCarButtons.map((buttonId) => {
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