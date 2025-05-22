document.addEventListener("DOMContentLoaded", function () {
    const deleteButton = document.getElementById("DeleteButton");
    const logOutButton = document.getElementById("LogOutButton");
    const deletePopUp = document.getElementById("DeletePopUp");
    const logOutPopUp = document.getElementById("LogOutPopUp");
    const deleteButtonPopUp = document.getElementById("DeleteButtonPopUp");
    const logOutButtonPopUp = document.getElementById("LogOutButtonPopUp");
    const cancelDeleteButton = document.getElementById("CancelDeleteButton");
    const cancelLogOutButton = document.getElementById("CancelLogOutButton");

    deleteButton.addEventListener("click", function () {
        deletePopUp.showModal();
    });

    logOutButton.addEventListener("click", function () {
        logOutPopUp.showModal();
    });

    cancelDeleteButton.addEventListener("click", function () {
        deletePopUp.close();
    });

    cancelLogOutButton.addEventListener("click", function () {
        logOutPopUp.close();
    });
});