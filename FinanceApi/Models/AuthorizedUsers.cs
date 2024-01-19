﻿using Microsoft.AspNetCore.Identity;

namespace FinanceApi.Models
{
    public class AuthorizedUsers
    {
        public string OwnerId { get; set; }
        public string AuthorizedUserId { get; set; }
        public User Owner { get; set; }
        public User AuthorizedUser{ get; set; }
        public ICollection<IdentityRole> Roles{ get; set; }
    }
}
