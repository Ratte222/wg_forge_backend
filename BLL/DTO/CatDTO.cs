using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.DTO
{
    public class CatDTO
    {
        /// <summary>
        /// Cat name
        /// </summary>
        /// <example>Tom</example>
        [Required]        
        public string Name { get; set; }        
        
        /// <summary>
        /// Cat color
        /// </summary>
        /// <example>black</example>
        public string Color { get; set; }

        /// <summary>
        /// Tail length in centimeters
        /// </summary>
        /// <example>15</example>
        public int TailLength { get; set; }

        /// <summary>
        /// Whiskers length in centimeters
        /// </summary>
        /// <example>10</example>
        public int WhiskersLength { get; set; }
    }
}
