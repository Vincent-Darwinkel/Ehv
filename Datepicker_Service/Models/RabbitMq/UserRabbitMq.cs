namespace Datepicker_Service.Models.RabbitMq
{
    public class UserRabbitMq
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public bool ReceiveEmail { get; set; }
    }
}