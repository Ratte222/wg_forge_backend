using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities
{
    [Table("cats")]
    public class Cat
    {
        [Key, Column("name")]
        public string Name { get; set; }
        [Column("color"), MaxLength(40)]
        public string Color { get; set; }
        [Column("tail_length")]
        public int TailLength { get; set; }
        [Column("whiskers_length")]
        public int WhiskersLength { get; set; }
    }
}
