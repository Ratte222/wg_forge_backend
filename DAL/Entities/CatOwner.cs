using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class CatOwner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }


        public List<Cat> Cats { get; set; } = new List<Cat>();
        public List<CatsAndOwners> CatsAndOwners { get; set; } = new List<CatsAndOwners>();
    }
}
