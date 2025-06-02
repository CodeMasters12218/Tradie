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

    // Select 'ropa' as default category
    showCategory('ropa'); 
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

// CART
function toggleCart(btn) {
    btn.classList.add('added');
    const icon = btn.querySelector('i');
    icon.classList.remove('bi-cart');
    icon.classList.add('bi-cart-check');
    // Let the form submit normally (no return false!)
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
    const cardContainer = sliderWrapper.querySelector('.product-group:not(.d-none) .card-container');
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

    const cardContainer = sliderWrapper.querySelector('.product-group:not(.d-none) .card-container');
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

    async function updateQuantityOnServer(itemId, quantity) {
        const response = await fetch('/ShoppingCart/UpdateQuantity', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
            },
            body: JSON.stringify({ itemId, quantity })
        });

        if (!response.ok) {
            const errorText = await response.text();
            console.error("UpdateQuantity failed:", errorText);
        }

        if (response.ok) {
            const data = await response.json();
            if (data.success) {
                subtotalValue.innerText = `€${data.subtotal.toFixed(2)}`;
                overallTotalValue.innerText = `€${data.total.toFixed(2)}`;
            }
        }
    }

    document.querySelectorAll('.cart-item-row').forEach(row => {
        const quantityDisplay = row.querySelector('.quantity-display');
        const plusBtn = row.querySelector('.quantity-btn-plus');
        const minusBtn = row.querySelector('.quantity-btn-minus');
        const priceElement = row.querySelector('.item-price');
        const unitPrice = parseFloat(priceElement?.innerText?.replace(/[^\d.]/g, '')) || 0;

        // Store the original unit price in a data attribute for easy access
        priceElement.setAttribute('data-unit-price', unitPrice);

        plusBtn?.addEventListener('click', () => {
            let quantity = parseInt(quantityDisplay.innerText) || 1;
            quantity++;
            quantityDisplay.innerText = quantity.toString().padStart(2, '0');
            const itemId = row.querySelector('input[name="itemId"]').value;
            console.log("Updating quantity for item:", itemId, "New quantity:", quantity);
            updateQuantityOnServer(itemId, quantity);
        });

        minusBtn?.addEventListener('click', () => {
            let quantity = parseInt(quantityDisplay.innerText) || 1;
            if (quantity > 1) {
                quantity--;
                quantityDisplay.innerText = quantity.toString().padStart(2, '0');
                const itemId = row.querySelector('input[name="itemId"]').value;
                console.log("Updating quantity for item:", itemId, "New quantity:", quantity);
                updateQuantityOnServer(itemId, quantity);
            }
        });

    });
});

/* Quantity Selector for Product Details page */
document.addEventListener('DOMContentLoaded', function () {
    const productPage = document.querySelector('.product-details-page');
    if (productPage) {
        const quantityDisplay = productPage.querySelector('.quantity-display');
        const quantityInput = productPage.querySelector('input[name="quantity"]');
        const plusBtn = productPage.querySelector('.quantity-btn-plus');
        const minusBtn = productPage.querySelector('.quantity-btn-minus');

        plusBtn?.addEventListener('click', () => {
            let quantity = parseInt(quantityDisplay.innerText) || 1;
            quantity++;
            quantityDisplay.innerText = quantity.toString().padStart(2, '0');
            quantityInput.value = quantity;
        });

        minusBtn?.addEventListener('click', () => {
            let quantity = parseInt(quantityDisplay.innerText) || 1;
            if (quantity > 1) {
                quantity--;
                quantityDisplay.innerText = quantity.toString().padStart(2, '0');
                quantityInput.value = quantity;
            }
        });
    }
});

// Show products in HOME Category selectors
function showCategory(cat) {
    document.querySelectorAll('.product-group').forEach(g => g.classList.add('d-none'));

    if (cat === 'ropa') {
        document.getElementById('ropa-products').classList.remove('d-none');
    } else if (cat === 'electronica') {
        document.getElementById('electronica-products').classList.remove('d-none');
    } else if (cat === 'informatica') {
        document.getElementById('informatica-products').classList.remove('d-none');
    }

    document.querySelectorAll('.btn-box .btn, .btn-box .btn-active').forEach(btn => btn.classList.remove('btn-active'));
    const clicked = [...document.querySelectorAll('.btn-box a')].find(a => a.textContent.toLowerCase().includes(cat));
    if (clicked) clicked.classList.add('btn-active');
}

// CATEGORY SWITCHER
function showCategory(category) {
    // Hide all groups
    document.querySelectorAll('.product-group').forEach(group => group.classList.add('d-none'));

    // Show the selected group
    const selectedGroup = document.getElementById(`${category}-products`);
    if (selectedGroup) {
        selectedGroup.classList.remove('d-none');
    }

    // Update buttons
    document.querySelectorAll('.btn-box a').forEach(btn => {
        btn.classList.remove('btn-active');
        btn.classList.add('btn');
    });

    // Find and activate the clicked button
    const activeBtn = Array.from(document.querySelectorAll('.btn-box a')).find(btn =>
        btn.getAttribute('onclick')?.includes(category)
    );

    if (activeBtn) {
        activeBtn.classList.remove('btn');
        activeBtn.classList.add('btn-active');
    }
}