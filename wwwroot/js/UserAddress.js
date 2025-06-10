document.addEventListener("DOMContentLoaded", function () {
    const deleteCardPopUp = document.getElementById("DeleteAddressPopup");
    const createAddressPopUp = document.getElementById("CreateAddressPopUp");
    const createAddressButton = document.getElementById("CreateAddressButton");
    const deleteCardButtons = document.querySelectorAll(".Delete-Button");
    const deleteAddressButton = document.getElementById("DeleteAddressButton");
    const cancelButton = document.getElementById("CancelButton");
    const cancelCreateButton = document.getElementById("CancelCreateButton");
    const antiForgeryInput = document.querySelector('input[name="__RequestVerificationToken"]');

    let addressIdToDelete = null;

    if (createAddressButton && createAddressPopUp) {
        createAddressButton.addEventListener("click", function () {
            createAddressPopUp.showModal();
        });
    }

    deleteCardButtons.forEach(button => {
        button.addEventListener("click", function () {
            addressIdToDelete = button.getAttribute("data-address-id");
            deleteAddressButton.setAttribute("data-address-id", addressIdToDelete);
            deleteCardPopUp.showModal();
        });
    });

    if (deleteAddressButton && deleteCardPopUp) {
        deleteAddressButton.addEventListener("click", function () {
            const addressId = deleteAddressButton.getAttribute("data-address-id");
            if (addressId) {
                fetch('/UserAddress/DeleteAddress', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                        'RequestVerificationToken': antiForgeryInput.value
                    },
                    body: `addressId=${encodeURIComponent(addressId)}&__RequestVerificationToken=${encodeURIComponent(antiForgeryInput.value)}`
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
    }

    if (cancelCreateButton && createAddressPopUp) {
        cancelCreateButton.addEventListener("click", function () {
            createAddressPopUp.close();
        });
    }

    if (cancelButton && deleteCardPopUp) {
        cancelButton.addEventListener("click", function () {
            deleteCardPopUp.close();
        });
    }
});