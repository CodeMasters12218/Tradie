namespace Tradie.Models.Wishlist
{
	public class Wishlist
	{
		public int Id { get; set; }
		public string UserId { get; set; } = string.Empty; // NEW
		public User? User { get; set; }

		public List<WishlistItem> Items { get; set; } = new List<WishlistItem>();

		public decimal Subtotal => Items.Sum(i => i.PriceAtAddition * i.Quantity);
		public decimal DeliveryFee => Items.Any() ? 4.00m : 0.00m;
		public decimal Total => Subtotal + DeliveryFee;
	}

}
