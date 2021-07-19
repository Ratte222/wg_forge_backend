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
}
