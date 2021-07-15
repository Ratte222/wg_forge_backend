using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DAL.Helpers;
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
        [Required, /*CatColors(ErrorMessage = "There is no such color of a cat")*/]
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

        public void CheckColors(AppSettings appSettings)
        {
            if(Color.IndexOf('&')>-1)//multicolor cat
            {
                string[] splitColor = Color.Split('&', System.StringSplitOptions.RemoveEmptyEntries);
                foreach(string s in splitColor)
                {
                    if (!appSettings.HexColor.Any(i => i.Key.ToLower() == s.ToLower().Trim()))
                        throw new BLL.Infrastructure.ValidationException("There is no such color of a cat");
                }
            }
            else
            if (!appSettings.HexColor.Any(i => i.Key.ToLower() == Color.ToLower()))
                throw new BLL.Infrastructure.ValidationException("There is no such color of a cat");
        }
    }
}
