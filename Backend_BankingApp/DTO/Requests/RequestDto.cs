using BankingAppBackend.Model;

namespace BankingAppBackend.DTO.Requests
{
    public class RequestDto
    {
        public string Name { get; set; }
        public string ReceiverId { get; set; }
        public string SenderId { get; set; }
        public decimal Amount { get; set; }
        public string Valute { get; set; }
        public string DueDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Closed { get; set; }
        public char Status { get; set; }
    }
}
