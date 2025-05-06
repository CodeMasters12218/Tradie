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

    // Initialize scroll buttons visibility
    document.querySelectorAll('.subcategory-cards').forEach(container => {
        showHideButtons(container);

        // Add scroll event listener to update button visibility during scrolling
        container.addEventListener('scroll', () => {
            showHideButtons(container);
        });
    });
});

// FAVORITES
function toggleHeart(btn) {
    btn.classList.toggle('favorited');
    var icon = btn.querySelector('i');

    if (btn.classList.contains('favorited')) {
        icon.classList.remove('bi-heart');
        icon.classList.add('bi-heart-fill');
    } else {
        icon.classList.remove('bi-heart-fill');
        icon.classList.add('bi-heart');
    }
}

/*
// ADD TO CART
function toggleCart(btn) {
    btn.classList.toggle('active');
    var icon = btn.querySelector('i');
    if (btn.classList.contains('active')) {
        icon.classList.remove('bi-cart');
        icon.classList.add('bi-cart-check-fill');
    } else {
        icon.classList.remove('bi-cart-check-fill');
        icon.classList.add('bi-cart');
    }
}
*/

// BETTER toggleCART
function toggleCart(btn) {
    btn.classList.toggle('active');
    var icon = btn.querySelector('i');

    const quantity = parseInt(document.querySelector('.quantity-display')?.innerText) || 1;

    if (btn.classList.contains('active')) {
        icon.classList.remove('bi-cart');
        icon.classList.add('bi-cart-check-fill');

        // Simulate sending data to server
        const productId = btn.dataset.productId;
        const price = parseFloat(document.querySelector('.price-label').dataset.price);

        fetch('/ShoppingCart/AddToCart', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
            },
            body: JSON.stringify({
                productId: productId,
                quantity: quantity,
                price: price
            })
        })
            .then(response => response.json())
            .then(data => {
                console.log('Added to cart:', data);
                // Optionally update cart UI here dynamically
            })
            .catch(error => {
                console.error('Error adding to cart:', error);
            });

    } else {
        icon.classList.remove('bi-cart-check-fill');
        icon.classList.add('bi-cart');
        // Optionally: Remove item from cart
    }
}


// ADD TO CART HOME
function toggleCartHome(btn) {
    btn.classList.toggle('active');
    var icon = btn.querySelector('i');

    if (btn.classList.contains('active')) {
        btn.innerHTML = 'Añadido a la cesta <i class="bi bi-cart-check-fill"></i>';
    } else {
        btn.innerHTML = 'Añadir a la cesta <i class="bi bi-cart"></i>';
    }
}


// Slider functionality
let sliderPositions = {}; // Tracks current position of each slider

function slideCards(sliderId, direction) {
    const sliderWrapper = document.getElementById(sliderId);
    const cardContainer = sliderWrapper.querySelector('.card-container');
    const cards = cardContainer.querySelectorAll('.card');

    // Initialize position if not set
    if (!(sliderId in sliderPositions)) {
        sliderPositions[sliderId] = 0;
    }

    // Calculate number of cards that fit in the viewport
    const containerWidth = sliderWrapper.parentElement.offsetWidth;
    const cardWidth = cards[0].offsetWidth + 30; // card width + gap
    const visibleCards = Math.floor(containerWidth / cardWidth);

    // Calculate max position
    const maxPosition = Math.max(0, cards.length - visibleCards);

    // Update position based on direction
    if (direction === 'left') {
        sliderPositions[sliderId] = Math.max(0, sliderPositions[sliderId] - 1);
    } else {
        sliderPositions[sliderId] = Math.min(maxPosition, sliderPositions[sliderId] + 1);
    }

    // Apply transformation
    const translateX = -sliderPositions[sliderId] * cardWidth;
    cardContainer.style.transform = `translateX(${translateX}px)`;

    // Update visibility of controls
    updateArrowVisibility(sliderId);
}

function updateArrowVisibility(sliderId) {
    const sliderWrapper = document.getElementById(sliderId);
    const container = sliderWrapper.parentElement;
    const leftArrow = container.querySelector('.slider-arrow.left');
    const rightArrow = container.querySelector('.slider-arrow.right');
    const cardContainer = sliderWrapper.querySelector('.card-container');
    const cards = cardContainer.querySelectorAll('.card');

    // Calculate maximum positions
    const containerWidth = sliderWrapper.parentElement.offsetWidth;
    const cardWidth = cards[0].offsetWidth + 30; // card width + gap
    const visibleCards = Math.floor(containerWidth / cardWidth);
    const maxPosition = Math.max(0, cards.length - visibleCards);

    // Show/hide arrows based on current position
    if (sliderPositions[sliderId] <= 0) {
        leftArrow.style.visibility = 'hidden';
    } else {
        leftArrow.style.visibility = 'visible';
    }

    if (sliderPositions[sliderId] >= maxPosition) {
        rightArrow.style.visibility = 'hidden';
    } else {
        rightArrow.style.visibility = 'visible';
    }
}

// Initialize all sliders on page load
document.addEventListener('DOMContentLoaded', function () {
    const sliders = document.querySelectorAll('.slider-wrapper');
    sliders.forEach(slider => {
        const sliderId = slider.id;
        sliderPositions[sliderId] = 0;
        updateArrowVisibility(sliderId);
    });

    // Handle window resize
    window.addEventListener('resize', function () {
        sliders.forEach(slider => {
            const sliderId = slider.id;
            updateArrowVisibility(sliderId);
        });
    });
});

// Category next buttons
function scrollContainerLeft(button) {
    const scrollWrapper = button.closest('.subcategory-scroll-wrapper');
    const container = scrollWrapper.querySelector('.subcategory-cards');

    container.scrollBy({
        left: -300, // adjust this value to match card width + gap
        behavior: 'smooth'
    });
}

function scrollRight(button) {
    const scrollWrapper = button.closest('.subcategory-scroll-wrapper');
    const container = scrollWrapper.querySelector('.subcategory-cards');

    container.scrollBy({
        left: 300, // adjust this value as needed
        behavior: 'smooth'
    });
}
function showHideButtons(container) {
    const scrollWrapper = container.closest('.subcategory-scroll-wrapper');
    const leftBtn = scrollWrapper.querySelector('.scroll-left');
    const rightBtn = scrollWrapper.querySelector('.scroll-right');

    // Show/hide left button
    if (container.scrollLeft <= 0) {
        leftBtn.style.display = 'none';
    } else {
        leftBtn.style.display = 'block';
    }

    // Show/hide right button
    const maxScrollLeft = container.scrollWidth - container.clientWidth;
    if (container.scrollLeft >= maxScrollLeft - 1) {
        rightBtn.style.display = 'none';
    } else {
        rightBtn.style.display = 'block';
    }
}

// === QUANTITY FUNCTIONS ===
document.addEventListener('DOMContentLoaded', function () {
    const deliveryPayValue = document.querySelector('.delivery-pay-value');
    const subtotalValue = document.querySelector('.subtotal-value');
    const overallTotalValue = document.querySelector('.overall-total-value');
    const deliveryFee = parseFloat(deliveryPayValue?.innerText?.replace(/[^\d.]/g, '')) || 0;

    document.querySelectorAll('.cart-item-row').forEach(row => {
        const quantityDisplay = row.querySelector('.quantity-display');
        const plusBtn = row.querySelector('.quantity-btn-plus');
        const minusBtn = row.querySelector('.quantity-btn-minus');
        const priceElement = row.querySelector('.item-price');
        const unitPrice = parseFloat(priceElement?.innerText?.replace(/[^\d.]/g, '')) || 0;

        function updatePrices(quantity) {
            const subtotal = unitPrice * quantity;
            subtotalValue.innerText = `€${subtotal.toFixed(2)}`;
            const total = subtotal + deliveryFee;
            overallTotalValue.innerText = `€${total.toFixed(2)}`;
        }

        plusBtn?.addEventListener('click', () => {
            let quantity = parseInt(quantityDisplay.innerText) || 1;
            quantity++;
            quantityDisplay.innerText = quantity.toString().padStart(2, '0');
            updatePrices(quantity);
        });

        minusBtn?.addEventListener('click', () => {
            let quantity = parseInt(quantityDisplay.innerText) || 1;
            if (quantity > 1) {
                quantity--;
                quantityDisplay.innerText = quantity.toString().padStart(2, '0');
                updatePrices(quantity);
            }
        });
    });
});

