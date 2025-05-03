// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

// Open modal by ID
function openModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.style.display = 'flex'; // Or 'block' if you're not using flex layout
        document.body.style.overflow = 'hidden'; // Prevent scrolling in background
    }
}

// Close modal by ID
function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.style.display = 'none';
        document.body.style.overflow = 'auto';
    }
}

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
    // Remove active class from "Gestión de Productos"
    const gestionLink = document.querySelector('.ges-prod-label').closest('.nav-link');
    if (gestionLink) {
        gestionLink.classList.remove('active');
    }

    // Add active class to "Registro de Productos"
    const registroLink = document.querySelector('a.nav-link[href="#"]:has(i.bi-clipboard-data)');
    if (registroLink) {
        registroLink.classList.add('active');
    } else {
        // Alternative selector if the :has selector is not supported in older browsers
        const allNavLinks = document.querySelectorAll('a.nav-link');
        allNavLinks.forEach(link => {
            if (link.querySelector('i.bi-clipboard-data')) {
                link.classList.add('active');
            }
        });
    }
});