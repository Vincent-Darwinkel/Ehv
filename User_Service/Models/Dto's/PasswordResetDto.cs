﻿using System;

namespace User_Service.Models
{
    public class PasswordResetDto
    {
        public Guid Uuid { get; set; }
        public Guid UserUuid { get; set; }
        public string Code { get; set; }
    }
}
