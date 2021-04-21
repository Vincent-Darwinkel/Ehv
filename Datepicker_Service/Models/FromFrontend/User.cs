using System;
using Datepicker_Service.Enums;

namespace Datepicker_Service.Models.FromFrontend
{
    public class User
    {
        public Guid Uuid { get; set; }
        public AccountRole AccountRole { get; set; }
    }
}