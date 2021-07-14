using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.DTO
{
    public class RegisterModelDTO
    {

        /// <summary>
        /// first name
        /// </summary>
        /// <example>Artur</example>
        [StringLength(40, ErrorMessage = "Name length can't be more than 40.")]
        [Required(ErrorMessage = "Не указан Name")]
        public string Name { get; set; }

        /// <summary>
        /// user age
        /// </summary>
        [Required(ErrorMessage = "Не указан Age")]
        public int? Age { get; set; }


        /// <summary>
        /// username
        /// </summary>
        /// <example>Ratte222</example>
        [StringLength(40, ErrorMessage = "Login length can't be more than 40.")]
        [Required(ErrorMessage = "Не указан Login")]
        public string Login { get; set; }

        [StringLength(32, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
    }
}
