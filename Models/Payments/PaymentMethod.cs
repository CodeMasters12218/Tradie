using Tradie.Models.Payments;

namespace Tradie.Models.Payments
{
	//public enum PaymentMethod { CreditCard, Paypal }

	public class PaymentMethod
	{
		public string? CardName { get; set; }
		public string? CardNumber { get; set; }
		public string? ExpirationDate { get; set; }
		public string? CVC { get; set; }
	}
}
