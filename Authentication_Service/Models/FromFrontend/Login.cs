using Authentication_Service.Enums;
using System.ComponentModel.DataAnnotations;

namespace Authentication_Service.Models.FromFrontend
{
    public class Login
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        // The following section is used if an user has an Admin or Site admin account.
        // These accounts can be used to select the role a current user needs
        public int LoginCode { get; set; }
        public AccountRole SelectedAccountRole { get; set; }
    }
}