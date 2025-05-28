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
            openEditPopup(button);
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

    function openEditPopup(button) {
        const userId = button.getAttribute("data-user-id");
        const userName = button.getAttribute("data-user-name");
        const userLastName = button.getAttribute("data-user-lastnames");
        const userEmail = button.getAttribute("data-user-email");
        const userRole = button.getAttribute("data-user-role");
        document.getElementById("editUserId").value = userId;
        document.getElementById("editUserName").value = userName;
        document.getElementById("editUserEmail").value = userEmail;
        document.getElementById("editUserLastNames").value = userLastName;
        document.getElementById("editUserRole").value = userRole;
        const editPopUp = document.getElementById("EditUser-PopUp");
        editPopUp.showModal();
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

// Submenu collapsible
function toggleSubmenu(id, element) {
    const submenu = document.getElementById(id);
    const icon = element.querySelector('.bi-chevron-down, .bi-chevron-right');

    if (submenu.style.display === 'none' || submenu.style.display === '') {
        submenu.style.display = 'block';
        // Update icon to down arrow
        if (icon) {
            icon.classList.remove('bi-chevron-right');
            icon.classList.add('bi-chevron-down');
        }
    } else {
        submenu.style.display = 'none';
        // Update icon to right arrow
        if (icon) {
            icon.classList.remove('bi-chevron-down');
            icon.classList.add('bi-chevron-right');
        }
    }
}

// Function to initialize the active state on "Registro de Productos"
document.addEventListener('DOMContentLoaded', function () {
    // Collapse "Gestión de Productos"
    const productSubMenu = document.getElementById('productSubMenu');
    if (productSubMenu) productSubMenu.style.display = 'none';

    const productLink = document.querySelector('.ges-prod-label').closest('.nav-link');
    if (productLink) {
        const productIcon = productLink.querySelector('.bi-chevron-down, .bi-chevron-right');
        if (productIcon) {
            productIcon.classList.remove('bi-chevron-down');
            productIcon.classList.add('bi-chevron-right');
        }
    }

    // Expand "Gestión de Usuarios" submenu (no active class on the parent)
    const userSubMenu = document.getElementById('userSubMenu');
    if (userSubMenu) userSubMenu.style.display = 'block';

    const userLink = document.querySelector('.ges-usuario-label').closest('.nav-link');
    if (userLink) {
        const userIcon = userLink.querySelector('.bi-chevron-down, .bi-chevron-right');
        if (userIcon) {
            userIcon.classList.remove('bi-chevron-right');
            userIcon.classList.add('bi-chevron-down');
        }
    }

    // Set only "Registro de usuarios" as active
    const allLinks = document.querySelectorAll('.nav-link');
    allLinks.forEach(link => link.classList.remove('active'));

    const registroUsuariosLink = Array.from(document.querySelectorAll('#userSubMenu .nav-link')).find(link =>
        link.innerText.includes('Registro de usuarios')
    );
    if (registroUsuariosLink) {
        registroUsuariosLink.classList.add('active');
    }
});
