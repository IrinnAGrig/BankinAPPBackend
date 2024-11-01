namespace BankingAppBackend.Model
{
    public class Transaction
    {
        public required string Id { get; set; }
        public TransactionType Type { get; set; }
        public TransactionInfo Info { get; set; }
        public string Date { get; set; }
    }

    public enum TransactionType
    {
        Buy,
        Transfer
    }

    public class TransactionInfo
    {
        public string ReceiverId { get; set; }
        public User Receiver { get; set; }
        public string SenderId { get; set; }
        public User Sender { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Emblem { get; set; }
        public decimal Sum { get; set; }
        public string Valute { get; set; }
    }
}
