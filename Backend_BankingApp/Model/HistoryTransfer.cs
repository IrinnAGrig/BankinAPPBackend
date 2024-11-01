using BankingAppBackend.Model;

namespace Backend_BankingApp.Model
{
    public class HistoryTransfer
    {
          public int Id { get; set; }  // Primary key
          public string CardNumberSender { get; set; }
          public string IdUserSender { get; set; }
          public User UserSender { get; set; }

          // Foreign key to User
          public string CardNumberReceiver { get; set; }
          public string IdUserReceiver { get; set; }
          public User UserReceiver { get; set; }
     }
}
