namespace BankingAppBackend.Model
{
    public class Card
    {
        public required string Id { get; set; }
        public required string OwnerId { get; set; }
        public User User { get; set; }
        public string CardNumber { get; set; }
        public string NameHolder { get; set; }
        public string ExpiryDate { get; set; }
        public int Cvv { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }
        public string Valute { get; set; }
    }
}
