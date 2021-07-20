using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities
{
    public class CatOwner: IdentityUser
    {

        public int Age { get; set; }
        [Required]
        public int CatPoints { get; set; }


        public List<CatsAndOwners> CatsAndOwners { get; set; } = new List<CatsAndOwners>();
    }

    public class AccountRole
    {
        public const string Admin = "Admin";
        public const string CatOwner = "CatOwner";
        public const string User = "User";

        public static readonly string[] Roles = { Admin, CatOwner, User };
    }
}
