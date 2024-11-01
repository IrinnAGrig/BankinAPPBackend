namespace BankingAppBackend.DTO.Card
{
    public class CardDTO
    {
        public string OwnerId { get; set; }
        public string CardNumber { get; set; }
        public string NameHolder { get; set; }
        public string ExpiryDate { get; set; }
        public int Cvv { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }
        public string Valute { get; set; }
    }
}
