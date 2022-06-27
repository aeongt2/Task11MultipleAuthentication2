﻿using Microsoft.AspNetCore.Identity;

namespace Task11MultipleAuthentication2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
