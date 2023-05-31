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

            console.log(arr);
        }
    });

    return arr;
}

function getUserById(id) {
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

            console.log(user);
        }
    });

    return user;
}

function getUserRoles(id) {
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

            console.log(roles);
        }
    });

    return roles;
}

function rendeUsers() {
    let arr: Array<User> = getUsers();
    let table: string = 
    `<table class="table" style="align-content: center; text-align: center; vertical-align: middle;">
        <thead>
            <tr>
                <th>Id</th>
                <th>Ім'я</th>
                <th>Логін</th>
                <th>Пароль</th>
                <th>Пошта</th>
                <th>Номер телефона</th>
                <th>Ролі</th>
                <th>Редагувати</th>
                <th>Видалити</th>
            </tr>
        </thead>`;
    let tableBody: string = '';

    arr.map((user) => {
        let roles: Array<string> = getUserRoles(user.id);

        tableBody += 
        `<tr>
            <td>${user.id}</td>
            <td>${user.firstName} ${user.middleName} ${user.lastName}</td>
            <td>${user.userName}</td>
            <td>${user.passwordHash}</td>
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
                        onclick="window.location.href='/User/Update/${user.id}'">
                     Редагувати
                </button>
            </td>
            <td>
                <button class="btn btn-danger" onclick="deleteDialog('${user.id}')">Видалити</button>
            </td>
        </tr>`
    });

    table +=
        `
            <tbody>
                ${tableBody}
            </body>

        </table>`;

    return document.write(table);
}

function deleteDialog(id) {
    var result = confirm("Ви впевнені в тому, що зочете видалити запис?");

    if (result) {
        window.location.href = `https://localhost:7128/api/User/delete/${id}`;
    }
}

function fillUpdateForm(id) {
    let user: User = getUserById(id);

    console.log("Hello");

    (document.getElementById("FirstName") as HTMLInputElement).value = user.firstName;
    (document.getElementById("MiddleName") as HTMLInputElement).value = user.middleName;
    (document.getElementById("LastName") as HTMLInputElement).value = user.lastName;
    (document.getElementById("UserName") as HTMLInputElement).value = user.userName;
    (document.getElementById("Email") as HTMLInputElement).value = user.email;
    (document.getElementById("PhoneNumber") as HTMLInputElement).value = user.phoneNumber;
} 
