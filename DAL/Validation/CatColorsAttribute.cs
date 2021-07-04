using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DAL.Entities;

namespace DAL.Validation
{
    public class CatColorsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputValue = value as string;
            var isValid = false;

            if (!string.IsNullOrEmpty(inputValue))
            {
                isValid = Cat.CatColor.Any(i=>i == inputValue);
            }

            return isValid;
        }
    }
}
