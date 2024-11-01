using BankingAppBackend.Model;

namespace BankingAppBackend.DTO
{
    public class TransactionDto
    {
        public string Type { get; set; }
          public string ReceiverId { get; set; }
          public string SenderId { get; set; }
          public string Title { get; set; }
          public string Subtitle { get; set; }
          public string Emblem { get; set; }
          public decimal Sum { get; set; }
          public string Valute { get; set; }
     }

}
