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
    trains: Array<Train>
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
    trainCarType: number;
    trainId: number;
    places: Array<Place>;
}

interface Place {
    id: number;
    number: number;
    price: number;
    placeType: number;
    isAvaliable: boolean;
    trainCarId: number;
}

var placeTypes: { [placeType: string]: number } = {
    "Compartment": 0,
    "Public": 1,
    "Luxe": 2,
    "S1": 3,
    "S2": 4
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
        }
    });

    return arr;
}

function getNumberOfPlaces(trainCarId: number, placeType: number) {
    let count: number;
    let token: string = getToken({
        clientId: `m2m.client`,
        clientSecret: `SuperSecretPassword`,
        scope: `railwaytickets.read`
    });

    $.ajax({
        type: "GET",
        async: false,
        url: `https://localhost:7250/api/Place/${trainCarId}/${placeType}/count`,
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

function displaySearched(form: HTMLFormElement) {
    let languageCode = window.location.pathname.split("/")[1];
    let dto: SearchDto = convertFormToDto(form);
    var data: Array<Route> = searchRoutes(dto);
    var trains: string;
    var list: string = `
        <ul style="list-style-type: none; padding: 0; margin: 0; width: 80%;">
    `
    data.map((route) => {
        let compartmentCount: number = 0;
        let publictCount: number = 0;
        let luxeCount: number = 0;
        let s1Count: number = 0;
        let s2Count: number = 0;
        trains = ``;

        route.trains.map((train) => {
            train.trainCars.map((trainCar) => {
                compartmentCount += getNumberOfPlaces(trainCar.id, placeTypes["Compartment"]);
                publictCount += getNumberOfPlaces(trainCar.id, placeTypes["Public"]);
                luxeCount += getNumberOfPlaces(trainCar.id, placeTypes["Luxe"]);
                s1Count += getNumberOfPlaces(trainCar.id, placeTypes["S1"]);
                s2Count += getNumberOfPlaces(trainCar.id, placeTypes["S2"]);
            });
        });

        list += `
            <li style="width: 110%">
                <div class="resultitem">
                    <span>${route.fromCity}</span>
                    <span>${route.toCity}</span>
                    <span>${new Date(route.departureTime.toString()).toLocaleTimeString()}</span>
                    <span>${new Date(route.arrivalTime.toString()).toLocaleTimeString()}</span>
                    <span>${route.travelTime}</span> 
                    <div>
                        <ul style="list-style-type: none; padding: 0; margin: 0;">
                            <li>Compartment - ${compartmentCount}</li>
                            <li>Public - ${publictCount}</li>
                            <li>Luxe - ${luxeCount}</li>
                            <li>S1 - ${s1Count}</li>
                            <li>S2 - ${s2Count}</li>
                        </ul>
                    </div>
                    <div style="width: 20%;">
                        <ul id="trains" style="list-style-type: none; padding: 0; margin: 0;">
                            ${route.trains.map((train) => {
                                trains += 
                                    `
                                        <li style="display: flex; align-items: center; justify-content: space-between;">
                                            <span>${train.number}</span>
                                            <button class="btn btn-warning" style="margin-left: 0.5%" onclick="window.location.href='/${languageCode}/Home/Book?trainId=${train.id}'">
                                                ${languageCode === "uk" ? "Забронювати" : languageCode === 'en' ? "Book" : ""}
                                            </button>
                                        </li>  
                                    `;
                            })}
                            ${trains}
                        </ul>
                    </div>
                </div>
            </li>`
    });

    list += `</ul>`

    document.getElementById("resultholder").innerHTML = list;
}

