namespace Tradie.Models.Payments
{
    public class Country
    {
        public Name Name { get; set; }
        public Translations Translations { get; set; }
    }

    public class Name
    {
        public string Common { get; set; }
    }

    public class Translations
    {
        public Translation Spa { get; set; }
    }

    public class Translation
    {
        public string Official { get; set; }
        public string Common { get; set; }
    }

}
