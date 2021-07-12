using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BLL.ValidationClass;

namespace BLL.DTO
{
    public class NewCatDTO
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
        [Required, CatColors(ErrorMessage = "There is no such color of a cat")]
        public string Color { get; set; }
        /// <summary>
        /// Tail length in centimeters
        /// </summary>
        /// <example>15</example>
        [Required, Range(0, 43, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int TailLength { get; set; }
        /// <summary>
        /// Whiskers length in centimeters
        /// </summary>
        /// <example>10</example>
        [Required, Range(0, 20, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int WhiskersLength { get; set; }

        /// <summary>
        /// Cat owners list
        /// </summary>
        //public List<CatOwnerDTO> CatOwnersDTO { get; set; } = new List<CatOwnerDTO>();
    }
}
