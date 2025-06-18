using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Tradie.Data;
using Tradie.Models.Orders;
using Tradie.Models.Payments;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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

        public async Task<IActionResult> PaymentDetails(PaymentSummary model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

            if (cart == null || !cart.Items.Any())
                return RedirectToAction("Index", "ShoppingCart");

            model.Items = cart.Items;
            model.Countries = await GetCountryNamesAsync();

            return View(model);
        }

        public async Task<IActionResult> PaymentMethod(PaymentSummary model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

            if (cart == null || !cart.Items.Any())
                return RedirectToAction("Index", "ShoppingCart");

            var  paymentSummaryModel = new PaymentSummary
            {
                Name = model.Name,
                EmailAddress = model.EmailAddress,
                LastNames = model.LastNames,
                Phone = model.Phone,
                Address = model.Address,
                Country = model.Country,
                City = model.City,
                Region = model.Region,
                PostalCode = model.PostalCode,
                Note = model.Note,
                Items = cart.Items,
                Countries = await GetCountryNamesAsync()
            };

            return View(paymentSummaryModel);
        }

        public async Task<IActionResult> PaymentSummary(PaymentDetails model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

            if (cart == null || !cart.Items.Any())
                return RedirectToAction("Index", "ShoppingCart");

            var paymentSummaryModel = new PaymentSummary
            {
                Name = model.Name,
                EmailAddress = model.EmailAddress,
                LastNames = model.LastNames,
                Phone = model.Phone,
                Address = model.Address,
                Country = model.Country,
                City = model.City,
                Region = model.Region,
                PostalCode = model.PostalCode,
                Note = model.Note,
                Items = cart.Items,
                Countries = await GetCountryNamesAsync()
            };

            return View(paymentSummaryModel);
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

            // Filtra los items cuyo producto es nulo
            var validCartItems = cart.Items
                .Where(item => item.Product != null && !string.IsNullOrEmpty(item.Product.Name) && item.Product.Price != null)
                .ToList();

            if (!validCartItems.Any())
            {
                TempData["ToastMessage"] = "No hay productos válidos en el carrito.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Index", "ShoppingCart");
            }

            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                CustomerId = user.Id,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Procesado,
                Items = validCartItems.Select(item => new OrderItem
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
            return RedirectToAction("Orders", "Order");
        }

        private static string GenerateOrderNumber()
        {
            var random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }

        private async Task<List<string>> GetCountryNamesAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://restcountries.com/v3.1/all?fields=name,translations";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var countries = JsonConvert.DeserializeObject<List<Country>>(json);
                    return countries
                        .Select(c => c.Translations?.Spa?.Common ?? c.Name.Common)
                        .OrderBy(n => n)
                        .ToList();
                }
            }
            return new List<string>();
        }
    }
}