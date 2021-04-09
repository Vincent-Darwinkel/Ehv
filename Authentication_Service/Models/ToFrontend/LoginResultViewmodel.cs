namespace Authentication_Service.Models.ToFrontend
{
    public class LoginResultViewmodel
    {
        public string Jwt { get; set; }
        public string RefreshToken { get; set; }
    }
}