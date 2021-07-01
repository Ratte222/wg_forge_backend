using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities
{
    [Table("cat_colors_info")]
    public class CatColorInfo
    {
        [Column("color"), MaxLength(40)]
        public string Color { get; set; }
        [Column("count")]
        public int Count { get; set; }
    }
}
