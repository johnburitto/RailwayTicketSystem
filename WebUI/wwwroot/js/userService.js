function getUsers() {
    var arr;
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
    var user;
    $.ajax({
        type: "GET",
        async: false,
        url: "https://localhost:7128/api/User/".concat(id),
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
    var roles;
    $.ajax({
        type: "GET",
        async: false,
        url: "https://localhost:7128/api/User/roles/".concat(id),
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
    var arr = getUsers();
    var table = "<table class=\"table\" style=\"align-content: center; text-align: center; vertical-align: middle;\">\n        <thead>\n            <tr>\n                <th>Id</th>\n                <th>\u0406\u043C'\u044F</th>\n                <th>\u041B\u043E\u0433\u0456\u043D</th>\n                <th>\u041F\u0430\u0440\u043E\u043B\u044C</th>\n                <th>\u041F\u043E\u0448\u0442\u0430</th>\n                <th>\u041D\u043E\u043C\u0435\u0440 \u0442\u0435\u043B\u0435\u0444\u043E\u043D\u0430</th>\n                <th>\u0420\u043E\u043B\u0456</th>\n                <th>\u0420\u0435\u0434\u0430\u0433\u0443\u0432\u0430\u0442\u0438</th>\n                <th>\u0412\u0438\u0434\u0430\u043B\u0438\u0442\u0438</th>\n            </tr>\n        </thead>";
    var tableBody = '';
    arr.map(function (user) {
        var roles = getUserRoles(user.id);
        tableBody +=
            "<tr>\n            <td>".concat(user.id, "</td>\n            <td>").concat(user.firstName, " ").concat(user.middleName, " ").concat(user.lastName, "</td>\n            <td>").concat(user.userName, "</td>\n            <td>").concat(user.passwordHash, "</td>\n            <td>").concat(user.email, "</td>\n            <td>").concat(user.phoneNumber, "</td>\n            <td>\n                <ul style=\"list-style-type: none;margin: 0;padding: 0;\">\n                    ").concat(roles.map(function (role) {
                return "<li>".concat(role, "</li>");
            }), "\n                </ul>\n            </td>\n            <td>\n                <button class=\"btn btn-warning\"\n                        onclick=\"window.location.href='/User/Update/").concat(user.id, "'\">\n                     \u0420\u0435\u0434\u0430\u0433\u0443\u0432\u0430\u0442\u0438\n                </button>\n            </td>\n            <td>\n                <button class=\"btn btn-danger\" onclick=\"deleteDialog('").concat(user.id, "')\">\u0412\u0438\u0434\u0430\u043B\u0438\u0442\u0438</button>\n            </td>\n        </tr>");
    });
    table +=
        "\n            <tbody>\n                ".concat(tableBody, "\n            </body>\n\n        </table>");
    return document.write(table);
}
function deleteDialog(id) {
    var result = confirm("Ви впевнені в тому, що зочете видалити запис?");
    if (result) {
        window.location.href = "https://localhost:7128/api/User/delete/".concat(id);
    }
}
function fillUpdateForm(id) {
    var user = getUserById(id);
    console.log("Hello");
    document.getElementById("FirstName").value = user.firstName;
    document.getElementById("MiddleName").value = user.middleName;
    document.getElementById("LastName").value = user.lastName;
    document.getElementById("UserName").value = user.userName;
    document.getElementById("Email").value = user.email;
    document.getElementById("PhoneNumber").value = user.phoneNumber;
}
//# sourceMappingURL=userService.js.map