﻿interface User {
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

var roles: { [role: string]: string } = {
    "User": "0",
    "Admin": "1"
}

function getUsers() {
    let arr: Array<User>;

    $.ajax({
        type: "GET",
        async: false,
        url: "https://localhost:7128/api/User",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (result) {
            arr = JSON.parse(JSON.stringify(result));
        }
    });

    return arr;
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

function getUserRoles(id: string) {
    let roles: Array<string>

    $.ajax({
        type: "GET",
        async: false,
        url: `https://localhost:7128/api/User/roles/${id}`,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (result) {
            roles = JSON.parse(JSON.stringify(result));
        }
    });

    return roles;
}

function rendeUsers() {
    let languageCode = window.location.pathname.split("/")[1];

    console.log(window.location.pathname);

    let arr: Array<User> = getUsers();
    let table: string = '';
    let tableBody: string = '';

    arr.map((user) => {
        let roles: Array<string> = getUserRoles(user.id);

        tableBody += 
        `<tr>
            <td>${user.id}</td>
            <td>${user.firstName} ${user.middleName} ${user.lastName}</td>
            <td>${user.userName}</td>
            <td style="max-width: 250px; word-wrap: break-word;">${user.passwordHash}</td>
            <td>${user.email}</td>
            <td>${user.phoneNumber}</td>
            <td>
                <ul style="list-style-type: none;margin: 0;padding: 0;">
                    ${roles.map((role) => {
                        return `<li>${role}</li>`
                    })}
                </ul>
            </td>
            <td>
                <button class="btn btn-warning"
                        onclick="window.location.href='/${languageCode}/User/Update/${user.id}'">
                     ${languageCode === 'uk' ? "Редагувати" : languageCode === 'en' ? "Update" : ""}
                </button>
            </td>
            <td>
                <button class="btn btn-danger" onclick="deleteDialog('${user.id}')">
                    ${languageCode === 'uk' ? "Видалити" : languageCode === 'en' ? "Delete" : ""}
                </button>
            </td>
        </tr>`
    });

    table +=
        `
            <tbody>
                ${tableBody}
            </tbody>
        `;

    return document.write(table);
}

function deleteDialog(id: string) {
    var result = confirm("Ви впевнені в тому, що зочете видалити запис?");
    let languageCode = window.location.pathname.split("/")[1];

    if (result) {
        window.location.href = `https://localhost:7128/api/User/delete/${id}`;
    }
}

function fillUpdateForm(id: string) {
    let user: User = getUserById(id);
    let userRoles: string[] = getUserRoles(user.id);

    console.log(userRoles[0]);
    console.log(roles[userRoles[0]]);

    (document.getElementById("FirstName") as HTMLInputElement).value = user.firstName;
    (document.getElementById("MiddleName") as HTMLInputElement).value = user.middleName;
    (document.getElementById("LastName") as HTMLInputElement).value = user.lastName;
    (document.getElementById("UserName") as HTMLInputElement).value = user.userName;
    (document.getElementById("Email") as HTMLInputElement).value = user.email;
    (document.getElementById("PhoneNumber") as HTMLInputElement).value = user.phoneNumber;
    (document.getElementById("Role") as HTMLSelectElement).value = roles[userRoles[0]];
} 
