interface User {
    accessFailedCount: number;
    concurrencyStamp: string;
    email: string;
    emailConfirmed: boolean;
    firstName: string;
    id: string;
    lastName: string;
    lockoutEnabled: boolean;
    lockoutEnd: boolean;
    middleName: string;
    normalizedEmail: string;
    normalizedUserName: string;
    passwordHash: string;
    phoneNumber: string;
    phoneNumberConfirmed: boolean;
    securityStamp: string;
    twoFactorEnabled: boolean;
    userName: string;
}

function getUserById(id: string) {
    let user: User;

    $.ajax({
        type: "GET",
        async: false,
        url: `https://localhost:7128/api/User/${id}`,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (result) {
            user = JSON.parse(JSON.stringify(result));
        }
    });

    return user;
}

function generateUserPersonalCabinetInfo(id: string) {
    var user = getUserById(id);

    document.getElementById("FirstName").textContent = user.firstName;
    document.getElementById("MiddleName").textContent = user.middleName;
    document.getElementById("LastName").textContent = user.lastName;
    document.getElementById("UserName").textContent = user.userName;
    document.getElementById("Email").textContent = user.email;
    document.getElementById("PhoneNumber").textContent = user.phoneNumber;
}