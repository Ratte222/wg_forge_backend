using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using DAL.Validation;
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
        [Key, Required, Column("name")]
        public string Name { get; set; }
        //не понял как сдеать перечесления сторок чтоб использовать атрибут "EnumDataType" 
        //так что сделал свой атрибут
        [Column("color"), Required, MaxLength(40), CatColors(ErrorMessage = "There is no such color of a cat")]
        public string Color { get; set; }
        [Column("tail_length"), Required, Range(0, 43, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int TailLength { get; set; }
        [Column("whiskers_length"), Required, Range(0, 20, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int WhiskersLength { get; set; }
    }
}
