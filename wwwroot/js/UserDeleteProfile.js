document.addEventListener("DOMContentLoaded", function () {
    const deletePopUpButton = document.getElementById("DeletePopUpButton");
    const deletePopUp = document.getElementById("DeletePopUp");
    const cancelButton = document.getElementById("CancelButton");

    deletePopUpButton.addEventListener("click", function () {
        deletePopUp.showModal();
    });

    cancelButton.addEventListener("click", function () {
        deletePopUp.close();
    });
});