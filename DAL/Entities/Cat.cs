using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace DAL.Entities
{


    public class Cat:BaseEntity
    {
        //[Key/*, Column("name")*/]
        public string Name { get; set; }
        public string Color { get; set; }
        //[Column("tail_length")]
        public int TailLength { get; set; }
        //[Column("whiskers_length")]
        public int WhiskersLength { get; set; }

        public List<CatPhoto> CatPhotos { get; set; } = new List<CatPhoto>();

        public List<CatsAndOwners> CatsAndOwners { get; set; } = new List<CatsAndOwners>();
    }
}
