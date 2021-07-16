using DAL.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        public string ReasoneDeleteCat { get; set; }
        //public List<CatOwnerDTO> CatOwnersDTO { get; set; } = new List<CatOwnerDTO>();

        public List<CatPhotoDTO> CatPhotosDTO { get; set; } = new List<CatPhotoDTO>();

        public void CheckReasoneAddCat(AppSettings appSettings)
        {
            if (!appSettings.ReasoneDeleteCat.Any(i => (i.Key.ToLower() == ReasoneDeleteCat.ToLower()) &&
                    (i.Value <= 0)))
                throw new BLL.Infrastructure.ValidationException("There is no such reasone delete of a cat");
        }
    }
}
