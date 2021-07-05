using System.ComponentModel.DataAnnotations;
using BLL.ValidationClass;

namespace BLL.DTO
{
    public class NewCatDTO
    {
        [Required]
        public string Name { get; set; }
        [Required, CatColors(ErrorMessage = "There is no such color of a cat")]
        public string Color { get; set; }
        [Required, Range(0, 43, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int TailLength { get; set; }
        [Required, Range(0, 20, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int WhiskersLength { get; set; }
    }
}
