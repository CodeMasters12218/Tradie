using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradie.Data;
using Tradie.Models.Orders;
using Tradie.Models.Payments;

namespace Tradie.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(
            UserManager<User> userManager,
            ApplicationDbContext context
        ) : base(userManager, context)
        {
            _context = context;
        }

        public async Task<IActionResult> PaymentDetails()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

            if (cart == null || !cart.Items.Any())
                return RedirectToAction("Index", "ShoppingCart");

            var model = new PaymentDetails
            {
                Name = user.Name,
                EmailAddress = user.Email,
                Items = cart.Items
            };

            return View(model);
        }

        public async Task<IActionResult> PaymentMethod()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

            if (cart == null || !cart.Items.Any())
                return RedirectToAction("Index", "ShoppingCart");

            var model = new PaymentMethod
            {
                Items = cart.Items
            };

            return View(model);
        }

        public async Task<IActionResult> PaymentSummary()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

            if (cart == null || !cart.Items.Any())
                return RedirectToAction("Index", "ShoppingCart");

            var model = new PaymentSummary
            {
                Items = cart.Items
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

            if (cart == null || !cart.Items.Any())
                return RedirectToAction("Index", "ShoppingCart");

            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                CustomerId = user.Id,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Procesado,
                Items = cart.Items.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Product = item.Product,
                    ProductName = item.Product.Name,
                    PricePaid = item.Product.Price,
                    Quantity = item.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);

            cart.Items.Clear();

            await _context.SaveChangesAsync();

            TempData["ToastMessage"] = "¡Pago exitoso! Tus productos han sido pedidos.";
            TempData["ToastType"] = "success";

            // Redirigir a 'Mis Pedidos'
            return RedirectToAction("Pedidos", "UserProfile");
        }

        private static string GenerateOrderNumber()
        {
            var random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }
    }
}