using System.Linq;
using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace BLL.ValidationClass
{
    public class CatColorsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputValue = value as string;
            var isValid = false;

            if (!string.IsNullOrEmpty(inputValue))
            {
                isValid = Cat.CatColor.Any(i => i == inputValue);
            }

            return isValid;
        }
    }
}
