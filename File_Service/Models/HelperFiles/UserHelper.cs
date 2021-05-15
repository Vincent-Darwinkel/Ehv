using System;
using File_Service.Enums;

namespace File_Service.Models.HelperFiles
{
    public class UserHelper
    {
        public Guid Uuid { get; set; }
        public AccountRole AccountRole { get; set; }
    }
}
