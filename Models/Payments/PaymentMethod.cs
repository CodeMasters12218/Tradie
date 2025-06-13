using Tradie.Models.ShoppingCart;

namespace Tradie.Models.Payments
{
	//public enum PaymentMethod { CreditCard, Paypal }

	public class PaymentMethod
	{
		public string? CardName { get; set; }
		public string? CardNumber { get; set; }
		public string? ExpirationDate { get; set; }
		public string? CVC { get; set; }

        public List<CartItem> Items { get; set; } = new List<CartItem>();

		public decimal Subtotal => Items.Sum(i => i.PriceAtAddition * i.Quantity);
		public decimal DeliveryFee => Items.Any() ? 4.00m : 0.00m;
		public decimal Total => Subtotal + DeliveryFee;
	}
}
