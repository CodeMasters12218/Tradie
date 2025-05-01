document.addEventListener("DOMContentLoaded", function ()
{
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

    createButtons.forEach(button =>
    {
        button.addEventListener("click", function ()
        {
            createPopUp.showModal();
        });
    });

    editButtons.forEach(button =>
    {
        button.addEventListener("click", function ()
        {
            editPopUp.showModal();
        });
    });

    deleteButtons.forEach(button =>
    {
        button.addEventListener("click", function ()
        {
            openDeletePopup(button);
        });
    });

    confirmCreateButton.addEventListener("click", function ()
    {
        createPopUp.close();
    });

    confirmEditButton.addEventListener("click", function ()
    {
        editPopUp.close();
    });
    confirmDeleteButton.addEventListener("click", function ()
    {
        deletePopUp.close();
    });

    confirm

    closeButton.addEventListener("click", function ()
    {
        createPopUp.close();
    });

    closeEditButton.addEventListener("click", function ()
    {
        editPopUp.close();
    });

    closeDeleteButton.addEventListener("click", function ()
    {
        deletePopUp.close();
    });

    function openDeletePopup(button)
    {
        const userId = button.getAttribute("data-user-id");
        const userName = button.getAttribute("data-user-name");
        document.getElementById("deleteUserNameMessage").textContent = `${userName}`;
        document.getElementById("deleteUserIdMessage").textContent = `ID de usuario: ${userId}`;

        document.getElementById("deleteUserId").value = userId;

        const deletePopUp = document.getElementById("DeleteUser-PopUp");
        deletePopUp.showModal();
    }
});

function toggleMenu(menuId)
{
    const menu = document.getElementById(menuId);
    if (menu.style.display === "none" || menu.style.display === "")
    {
        menu.style.display = "block"; // Show options
    } else {
        menu.style.display = "none"; // Hide options
    }
}
