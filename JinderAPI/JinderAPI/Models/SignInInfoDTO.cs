﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinderAPI.Models
{
    public class SignInInfoDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
    }
}