﻿using System;
using Authentication_Service.Enums;

namespace Authentication_Service.Models.Dto
{
    public class UserDto
    {
        public Guid UserUuid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public AccountRole AccountRole { get; set; }
    }
}