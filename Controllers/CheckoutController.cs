using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tradie.Models.Paypal;

namespace PetInfo.Controllers
{
    public class CheckoutController : Controller
    {

        [TempData]
        public string TotalAmount { get; set; } = null;

        private readonly PaypalClient _paypalClient;
        public CheckoutController(PaypalClient paypalClient)
        {
            this._paypalClient = paypalClient;
        }



        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Order([FromBody] OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            try
            {
                var price = orderRequest.Price;
                var currency = "USD"; 
                var reference = GetRandomInvoiceNumber();

                var response = await _paypalClient.CreateOrder(price, currency, reference);

                if (response == null || string.IsNullOrEmpty(response.id))
                {
                    return BadRequest(new { message = "Error al obtener la orden de PayPal." });
                }

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.GetBaseException().Message });
            }
        }

        public class OrderRequest
        {
            public string Price { get; set; }
        }

        public async Task<IActionResult> Capture(string orderId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderId);

                var reference = response.purchase_units[0].reference_id;

                // Put your logic to save the transaction here
                // You can use the "reference" variable as a transaction key

                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }
        public static string GetRandomInvoiceNumber()
        {
            return new Random().Next(999999).ToString();
        }
        public IActionResult Success()
        {
            return View("~/Views/Payment/Success.cshtml");
        }
    }
}

