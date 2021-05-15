using File_Service.Enums;
using System;

namespace File_Service.Models.HelperFiles
{
    public class UserHelper
    {
        public Guid Uuid { get; set; }
        public AccountRole AccountRole { get; set; }
    }
}
