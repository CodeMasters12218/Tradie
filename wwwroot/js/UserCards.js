document.addEventListener("DOMContentLoaded", function () {
    const deleteCardPopUp = document.getElementById("DeleteCardPopup");
    const deleteCardButtons = document.querySelectorAll(".Delete-Button");
    const cancelButton = document.getElementById("CancelButton");
    const confirmDeleteButton = document.getElementById("ConfirmDeleteButton");

    let cardIdToDelete = null;

    deleteCardButtons.forEach(button => {
        button.addEventListener("click", function () {
            cardIdToDelete = button.getAttribute("data-card-id");
            confirmDeleteButton.setAttribute("data-card-id", cardIdToDelete);
            deleteCardPopUp.showModal();
        });
    });

    cancelButton.addEventListener("click", function () {
        deleteCardPopUp.close();
    });

    confirmDeleteButton.addEventListener("click", function () {
        const cardId = confirmDeleteButton.getAttribute("data-card-id");
        if (cardId) {
            fetch(`/UserCardProfile/DeleteCard`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded",
                    "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: `cardId=${cardId}`
            })
                .then(response => {
                    if (response.redirected) {
                        window.location.href = response.url;
                    } else {
                        window.location.reload();
                    }
                });
        }
    });
});