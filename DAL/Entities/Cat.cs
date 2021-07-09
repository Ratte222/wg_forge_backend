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
        [NotMapped]
        public static readonly string[] CatColor = 
        {
            "black",
            "white",
            "black & white",
            "red",
            "red & white",
            "red & black & white"
        };
        [Key, Column("name")]
        public string Name { get; set; }
        //не понял как сдеать перечесления сторок чтоб использовать атрибут "EnumDataType" 
        //так что сделал свой атрибут
        [Column("color")]
        public string Color { get; set; }
        [Column("tail_length")]
        public int TailLength { get; set; }
        [Column("whiskers_length")]
        public int WhiskersLength { get; set; }

        public int CatOwnerId { get; set; }
    }
}
