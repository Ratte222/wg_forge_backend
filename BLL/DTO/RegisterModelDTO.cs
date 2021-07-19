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
        [Required(ErrorMessage = "UserName not specified")]
        public string UserName { get; set; }

        /// <summary>
        /// user age
        /// </summary>
        [Required(ErrorMessage = "Age not specified")]
        public int Age { get; set; }


        /// <summary>
        /// username
        /// </summary>
        /// <example>Ratte222</example>
        [StringLength(40, ErrorMessage = "Email length can't be more than 40.")]
        [Required(ErrorMessage = "Email not specified")]
        public string Email { get; set; }

        [StringLength(32, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Entered password not correct")]
        public string ConfirmPassword { get; set; }
    }
}
