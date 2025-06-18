document.addEventListener("DOMContentLoaded", function () {
    const deleteButton = document.getElementById("DeleteButton");
    const logOutButtonPopUp = document.getElementById("LogOutButtonPopUp");
    const deletePopUp = document.getElementById("DeletePopUp");
    const logOutPopUp = document.getElementById("LogOutPopUp");
    const deleteButtonPopUp = document.getElementById("DeleteButtonPopUp");
    const cancelDeleteButton = document.getElementById("CancelDeleteButton");
    const cancelLogOutButton = document.getElementById("CancelLogOutButton");

    if (deleteButton) {
        deleteButton.addEventListener("click", function () {
            if (deletePopUp) {
                deletePopUp.showModal();
            }
        });
    }

    if (logOutButtonPopUp) {
        logOutButtonPopUp.addEventListener("click", function () {
            if (logOutPopUp) {
                logOutPopUp.showModal();
            }
        });
    }

    if (cancelDeleteButton) {
        cancelDeleteButton.addEventListener("click", function () {
            if (deletePopUp) {
                deletePopUp.close();
            }
        });
    }

    if (cancelLogOutButton) {
        cancelLogOutButton.addEventListener("click", function () {
            if (logOutPopUp) {
                logOutPopUp.close();
            }
        });
    }
});
