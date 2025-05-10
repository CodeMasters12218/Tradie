using Tradie.Models.Payments;

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
	}
}
