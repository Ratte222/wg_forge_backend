using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.DTO
{
    public class CatOwnerDTO
    {
        //[Required]
        //public int Id { get; set; }
        /// <summary>
        /// Owner name
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// Owner age
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Points received for actions with cats 
        /// </summary>
        public int CatPoints { get; set; }

        /// <summary>
        /// Cat list
        /// </summary>
        public List<CatDTO> Cats { get; set; } = new List<CatDTO>();
    }
}
