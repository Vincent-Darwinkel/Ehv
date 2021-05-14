using System;
using System.Collections.Generic;
using Authentication_Service.Enums;

namespace Authentication_Service.Models.ToFrontend
{
    public class LoginResultViewmodel
    {
        public string Jwt { get; set; }
        public Guid RefreshToken { get; set; }
        public bool UserHasMultipleAccountRoles { get; set; }
        public List<AccountRole> SelectableAccountRoles { get; set; }
    }
}