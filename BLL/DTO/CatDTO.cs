using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.DTO
{
    public class CatDTO
    {
        [Required]
        public string Name { get; set; }        
        public string Color { get; set; }        
        public int TailLength { get; set; }        
        public int WhiskersLength { get; set; }
    }
}
