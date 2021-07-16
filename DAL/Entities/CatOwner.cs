using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities
{
    public class CatOwner:BaseEntity
    {
        //public int Id { get; set; }
        [Required, MaxLength(40)]
        public string Name { get; set; }//userName
        public int Age { get; set; }
        [Required, MaxLength(40)]
        public string Login { get; set; }
        [Required, MaxLength(128)]
        public string Password { get; set; }
        [Required, MaxLength(20)]
        public string Role { get; set; }
        [Required]
        public int CatPoints { get; set; }

        //public List<Cat> Cats { get; set; } = new List<Cat>();
        public List<CatsAndOwners> CatsAndOwners { get; set; } = new List<CatsAndOwners>();
    }
}
