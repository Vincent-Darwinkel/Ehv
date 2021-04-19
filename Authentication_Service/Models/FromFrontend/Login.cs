using System.ComponentModel.DataAnnotations;

namespace Authentication_Service.Models.FromFrontend
{
    public class Login
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}