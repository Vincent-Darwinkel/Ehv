using System;

namespace Authentication_Service.Models.ToFrontend
{
    public class AuthorizationTokensViewmodel
    {
        public string Jwt { get; set; }
        public Guid RefreshToken { get; set; }
    }
}