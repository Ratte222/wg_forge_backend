using System.Linq;
using System.ComponentModel.DataAnnotations;
using DAL.Entities;
using DAL.Helpers;
using System;
using Microsoft.Extensions.Options;

namespace BLL.ValidationClass
{
    public class CatColorsAttribute : ValidationAttribute
    {
        //private AppSettings _appSettings;
        //public CatColorsAttribute(IOptions<AppSettings> appSettings) : base()
        //{
        //    _appSettings = appSettings.Value;
        //}

        public override bool IsValid(object value)
        {
            var inputValue = value as string;
            var isValid = false;

            if (!string.IsNullOrEmpty(inputValue))
            {
                //isValid = _appSettings.HexColor.Any(i => i.Key.ToLower() == inputValue.ToLower());
            }

            return isValid;
        }
    }
}
