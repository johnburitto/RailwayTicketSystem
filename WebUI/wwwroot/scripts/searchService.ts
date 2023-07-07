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

function searchRoutes(dto: SearchDto) {
    let arr: Array<Route>;

    $.ajax({
        type: "POST",
        async: false,
        url: "https://localhost:7250/api/Route/search",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(dto),
        dataType: 'json',
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
        <ul>
    `
    data.map((route) => {
        list += `<li>${route.fromCity} - ${route.toCity} | ${route.travelTime}</li>`
    });

    list += `</ul>`

    document.getElementById("resultholder").innerHTML = list;
}

