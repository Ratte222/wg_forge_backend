using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BLL.ValidationClass;
namespace BLL.DTO
{
    public class QueryCatColorInfoDTO
    {
        public string Attribute { get; set; }
        [QueryParamAttribute(ErrorMessage = @"The ""attribute"" parameter is not correct. 
                    Use ""name"" or ""color"" or ""tail_length"" or ""whiskers_length""")]
        public string Order { get; set; }
        public int? Offset { get; set;  }
        public int? Limit { get; set; }
    }
}
