// Función para alternar la visibilidad del menú
function toggleMenu(menuId) {
    const menu = document.getElementById(menuId);
    if (menu.style.display === "none" || menu.style.display === "") {
        menu.style.display = "block"; // Mostrar el menú
    } else {
        menu.style.display = "none"; // Ocultar el menú
    }
}
