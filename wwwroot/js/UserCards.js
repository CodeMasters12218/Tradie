document.addEventListener("DOMContentLoaded", function () {
    const deleteCardPopUp = document.getElementById("DeleteCardPopup");
    const deleteCardButton = document.querySelectorAll(".DeleteCardButton");
    const cancelButton = document.getElementById("CancelButton");

    deleteCardButton.forEach(button => {
        button.addEventListener("click", function () {
            deleteCardPopUp.showModal();
        });
    });

    cancelButton.addEventListener("click", function ()
    {
        deleteCardPopUp.close();
    });
});