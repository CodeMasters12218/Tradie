document.addEventListener("DOMContentLoaded", function () {
    const deleteCardPopUp = document.getElementById("DeleteAddressPopup");
    const createAddressPopUp = document.getElementById("CreateAddressPopUp");
    const createAddressButton = document.getElementById("CreateAddressButton");
    const deleteCardButton = document.querySelectorAll("DeleteAddressButton");
    const deleteAddressButton = document.getElementById("DeleteAddressButton");
    const cancelButton = document.getElementById("CancelButton");
    const cancelCreateButton = document.getElementById("CancelCreateButton");   

    createAddressButton.addEventListener("click", function () {
            createAddressPopUp.showModal();
        });

    deleteCardButton.forEach(button => {
        button.addEventListener("click", function () {
            deleteCardPopUp.showModal();
        });
    });

    deleteAddressButton.addEventListener("click", function () {
        deleteCardPopUp.close();
    });

    cancelCreateButton.addEventListener("click", function () {
        createAddressPopUp.close();
    });

    cancelButton.addEventListener("click", function () {
        deleteCardPopUp.close();
    });
});