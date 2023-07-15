var roles = {
    "User": "0",
    "Admin": "1"
};
function getUsers() {
    var arr;
    $.ajax({
        type: "GET",
        async: false,
        url: "http://192.168.0.107:7128/api/User",
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
function getUserById(id) {
    var user;
    $.ajax({
        type: "GET",
        async: false,
        url: "http://192.168.0.107:7128/api/User/".concat(id),
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
function getUserRoles(id) {
    var roles;
    $.ajax({
        type: "GET",
        async: false,
        url: "http://192.168.0.107:7128/api/User/roles/".concat(id),
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
    var languageCode = window.location.pathname.split("/")[1];
    console.log(window.location.pathname);
    var arr = getUsers();
    var table = '';
    var tableBody = '';
    arr.map(function (user) {
        var roles = getUserRoles(user.id);
        tableBody +=
            "<tr>\n            <td>".concat(user.id, "</td>\n            <td>").concat(user.firstName, " ").concat(user.middleName, " ").concat(user.lastName, "</td>\n            <td>").concat(user.userName, "</td>\n            <td style=\"max-width: 250px; word-wrap: break-word;\">").concat(user.passwordHash, "</td>\n            <td>").concat(user.email, "</td>\n            <td>").concat(user.phoneNumber, "</td>\n            <td>\n                <ul style=\"list-style-type: none;margin: 0;padding: 0;\">\n                    ").concat(roles.map(function (role) {
                return "<li>".concat(role, "</li>");
            }), "\n                </ul>\n            </td>\n            <td>\n                <button class=\"btn btn-warning\"\n                        onclick=\"window.location.href='/").concat(languageCode, "/User/Update/").concat(user.id, "'\">\n                     ").concat(languageCode === 'uk' ? "Редагувати" : languageCode === 'en' ? "Update" : "", "\n                </button>\n            </td>\n            <td>\n                <button class=\"btn btn-danger\" onclick=\"deleteDialog('").concat(user.id, "')\">\n                    ").concat(languageCode === 'uk' ? "Видалити" : languageCode === 'en' ? "Delete" : "", "\n                </button>\n            </td>\n        </tr>");
    });
    table +=
        "\n            <tbody>\n                ".concat(tableBody, "\n            </tbody>\n        ");
    return document.write(table);
}
function deleteDialog(id) {
    var result = confirm("Ви впевнені в тому, що зочете видалити запис?");
    var languageCode = window.location.pathname.split("/")[1];
    if (result) {
        window.location.href = "http://192.168.0.107:7128/api/User/delete/".concat(id);
    }
}
function fillUpdateForm(id) {
    var user = getUserById(id);
    var userRoles = getUserRoles(user.id);
    console.log(userRoles[0]);
    console.log(roles[userRoles[0]]);
    document.getElementById("FirstName").value = user.firstName;
    document.getElementById("MiddleName").value = user.middleName;
    document.getElementById("LastName").value = user.lastName;
    document.getElementById("UserName").value = user.userName;
    document.getElementById("Email").value = user.email;
    document.getElementById("PhoneNumber").value = user.phoneNumber;
    document.getElementById("Role").value = roles[userRoles[0]];
}
//# sourceMappingURL=user-service.js.map