using System.ComponentModel.DataAnnotations;

namespace Tradie.Models.UserCards
{
    public enum CardType
    {
        MasterCard,
        PayPal,
        GooglePay
    }

    public class UserCardModel
    {
        public int Id { get; set; }
        [StringLength(16, ErrorMessage = "El número de la tarjeta debe tener 16 dígitos.", MinimumLength = 16)]
        public string Number { get; set; } 
        public string Payeer { get; set; }
        public CardType CardType { get; set; } 
        public DateTime ExpiryDate { get; set; }
        [StringLength(3, ErrorMessage = "El CVV debe tener 3 dígitos.", MinimumLength = 3)]
        public string Cvv { get; set; }

        // Paypal and google pay only
        public string? Email { get; set; }
        public int UserId { get; internal set; }
    }
}

