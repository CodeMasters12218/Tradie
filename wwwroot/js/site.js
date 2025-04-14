// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function () {
    // Smooth scrolling for navigation links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();

            const targetId = this.getAttribute('href');
            const targetElement = document.querySelector(targetId);

            if (targetElement) {
                targetElement.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
});
function toggleHeart(button) {
    const icon = button.querySelector('i');
    icon.classList.toggle('bi-heart');
    icon.classList.toggle('bi-heart-fill');
    button.classList.toggle('favorited');
}

function toggleCart(button) {
    const icon = button.querySelector('i');
    const text = button.querySelector('.cart-text');

    const isActive = button.classList.contains('active');

    icon.classList.toggle('bi-cart', isActive);
    icon.classList.toggle('bi-cart-fill', !isActive);

    button.classList.toggle('active');

    // Back to small after 2secs
    if (!isActive) {
        setTimeout(() => {
            button.classList.remove('active');
            icon.classList('bi-cart');
            icon.classList.remove('bi-cart-fill');
        }, 2000);
    }
}