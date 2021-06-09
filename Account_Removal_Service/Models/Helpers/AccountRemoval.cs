using Account_Removal_Service.Enums;
using System;
using System.Collections.Generic;

namespace Account_Removal_Service.Models.Helpers
{
    public class AccountRemoval
    {
        public Guid UserUuid { get; set; }
        public List<RemovableOptions> DataToRemove { get; set; }
    }
}