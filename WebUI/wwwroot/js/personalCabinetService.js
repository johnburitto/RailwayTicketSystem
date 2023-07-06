import getUserById from "./userService.js";
function generateUserPersonalCabinetInfo(id) {
    var user = getUserById(id);
    document.getElementById("FirstName").textContent = user.firstName;
    document.getElementById("MiddleName").textContent = user.middleName;
    document.getElementById("LastName").textContent = user.lastName;
    document.getElementById("UserName").textContent = user.userName;
    document.getElementById("Email").textContent = user.email;
    document.getElementById("PhoneNumber").textContent = user.phoneNumber;
}
