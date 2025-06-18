using Tradie.Models.ShoppingCart;

namespace Tradie.Models.Payments
{
	public class PaymentSummary
	{
		public string? Name { get; set; }
		public string? EmailAddress { get; set; }
		public string? LastNames { get; set; }
		public string? Phone { get; set; }
		public string? Address { get; set; }
		public string? Country { get; set; }
		public string? City { get; set; }
		public string? Region { get; set; }
		public string? PostalCode { get; set; }
		public string? Note { get; set; }
		public string? CardName { get; set; }
		public string? CardNumber { get; set; }
		public string? ExpirationDate { get; set; }
		public string? CVC { get; set; }
		public List<string>? Countries { get; set; } = new List<string>();

		public List<CartItem> Items { get; set; } = new List<CartItem>();

		public decimal Subtotal => Items.Sum(i => i.PriceAtAddition * i.Quantity);
		public decimal DeliveryFee => Items.Any() ? 4.00m : 0.00m;
		public decimal Total => Subtotal + DeliveryFee;
	}
}
