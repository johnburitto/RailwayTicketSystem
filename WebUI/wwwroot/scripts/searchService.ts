interface GetTokenDto {
    clientId: string;
    clientSecret: string;
    scope: string;
}

interface SearchDto {
    fromCity: string;
    toCity: string;
    departureDate: Date;
}

interface Route {
    id: number;
    departureTime: Date;
    arrivalTime: Date;
    travelTime: string;
    fromCity: string;
    toCity: string;
    tranins: Array<Train>
}

interface Train {
    id: number;
    number: string;
    routeId: number;
    trainCars: Array<TrainCar>
}

interface TrainCar {
    id: number;
    number: string;
    trainId: number;
    places: Array<Place>;
}

interface Place {
    id: number;
    number: string;
    price: number;
    placeType: number;
    isAvaliable: boolean;
    trainCarId: number;
}

function convertFormToDto(form: HTMLFormElement): SearchDto {
    return {
        fromCity: (form.elements.item(0) as HTMLInputElement).value,
        toCity: (form.elements.item(1) as HTMLInputElement).value,
        departureDate: new Date((form.elements.item(2) as HTMLInputElement).value),
    };
}

function getToken(dto: GetTokenDto) {
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

function searchRoutes(dto: SearchDto) {
    let arr: Array<Route>;
    let token: string = getToken({
        clientId: `m2m.client`,
        clientSecret: `SuperSecretPassword`,
        scope: `railwaytickets.read`
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

function displaySearched(form: HTMLFormElement) {
    let dto: SearchDto = convertFormToDto(form);
    var data: Array<Route> = searchRoutes(dto);
    var list: string = `
        <ul style="list-style-type: none; padding: 0; margin: 0;">
    `
    data.map((route) => {
        list += `<li>${route.fromCity} - ${route.toCity} | ${route.travelTime}</li>`
    });

    list += `</ul>`

    document.getElementById("resultholder").innerHTML = list;
}

