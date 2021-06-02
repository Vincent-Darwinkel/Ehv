using Account_Removal_Service.Enums;
using System;

namespace Account_Removal_Service.Models.Helpers
{
    public class UserHelper
    {
        public Guid Uuid { get; set; }
        public AccountRole AccountRole { get; set; }
    }
}
