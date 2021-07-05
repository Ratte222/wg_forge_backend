using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.ValidationClass
{
    class QueryParamAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputValue = value as string;
            var isValid = true;

            if (!string.IsNullOrEmpty(inputValue))
            {
                isValid = ((inputValue != "name") && (inputValue != "color") &&
                (inputValue != "tail_length") && (inputValue != "whiskers_length"));
            }

            return isValid;
        }
    }
}
