document.addEventListener("DOMContentLoaded", function () {
    const createButtons = document.querySelectorAll(".Create-Button");
    const editButtons = document.querySelectorAll(".Edit-Button");
    const deleteButtons = document.querySelectorAll(".Delete-Button");
    const createPopUp = document.getElementById("CreateUser-PopUp");
    const editPopUp = document.getElementById("EditUser-PopUp");
    const deletePopUp = document.getElementById("DeleteUser-PopUp");
    const confirmCreateButton = document.getElementById("confirmCreate");
    const confirmEditButton = document.getElementById("confirmEdit");
    const confirmDeleteButton = document.getElementById("confirmDelete");
    const closeButton = document.getElementById("closeCreatePopUp");
    const closeEditButton = document.getElementById("closeEditPopUp");
    const closeDeleteButton = document.getElementById("closeDeletePopUp");

    createButtons.forEach(button => {
        button.addEventListener("click", function () {
            createPopUp.showModal();
        });
    });

    editButtons.forEach(button => {
        button.addEventListener("click", function () {
            editPopUp.showModal();
        });
    });

    deleteButtons.forEach(button => {
        button.addEventListener("click", function () {
            openDeletePopup(button);
        });
    });

    confirmCreateButton.addEventListener("click", function () {
        createPopUp.close();
    });

    confirmEditButton.addEventListener("click", function () {
        editPopUp.close();
    });
    confirmDeleteButton.addEventListener("click", function () {
        deletePopUp.close();
    });

    confirm

    closeButton.addEventListener("click", function () {
        createPopUp.close();
    });

    closeEditButton.addEventListener("click", function () {
        editPopUp.close();
    });

    closeDeleteButton.addEventListener("click", function () {
        deletePopUp.close();
    });

    function openDeletePopup(button) {
        const userId = button.getAttribute("data-user-id");
        const userName = button.getAttribute("data-user-name");
        document.getElementById("deleteUserNameMessage").textContent = `${userName}`;
        document.getElementById("deleteUserIdMessage").textContent = `ID de usuario: ${userId}`;

        document.getElementById("deleteUserId").value = userId;

        const deletePopUp = document.getElementById("DeleteUser-PopUp");
        deletePopUp.showModal();
    }
});

function toggleMenu(menuId) {
    const menu = document.getElementById(menuId);
    if (menu.style.display === "none" || menu.style.display === "") {
        menu.style.display = "block"; // Show options
    } else {
        menu.style.display = "none"; // Hide options
    }
}

document.addEventListener('click', function (event) {
    if (event.target && event.target.matches('.Edit-Button')) {
        const userId = event.target.dataset.userId;
        const userName = event.target.dataset.userName;
        const userEmail = event.target.dataset.userEmail;
        const userRole = event.target.dataset.userRole;

        // Comprobamos que los datos son correctos
        console.log("Datos obtenidos:", userId, userName, userEmail, userRole);

        // Asignamos valores al formulario
        document.querySelector('input[name="CurrentUser.Id"]').value = userId;
        document.querySelector('input[name="CurrentUser.Name"]').value = userName;
        document.querySelector('input[name="CurrentUser.Email"]').value = userEmail;
        document.querySelector('select[name="CurrentUser.Role"]').value = userRole;

        // Abre el diálogo
        document.getElementById('EditUser-PopUp').showModal();
    }
});



document.querySelector('form').addEventListener('submit', function (e) {
    const id = document.querySelector('input[name="CurrentUser.Id"]').value;
    const name = document.querySelector('input[name="CurrentUser.Name"]').value;
    const email = document.querySelector('input[name="CurrentUser.Email"]').value;
    const role = document.querySelector('select[name="CurrentUser.Role"]').value;

    console.log("Formulario a enviar:");
    console.log("ID:", id);
    console.log("Nombre:", name);
    console.log("Email:", email);
    console.log("Rol:", role);
});
