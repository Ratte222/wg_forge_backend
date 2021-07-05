using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using DAL.Validation;

namespace BLL.DTO
{
    public class NewCatDTO
    {
        [JsonPropertyName("name"), Required]
        public string Name { get; set; }
        [JsonPropertyName("color"), Required, CatColors(ErrorMessage = "There is no such color of a cat")]
        public string Color { get; set; }
        [JsonPropertyName("tail_length"), Required, Range(0, 43, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int TailLength { get; set; }
        [JsonPropertyName("whiskers_length"), Required, Range(0, 20, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int WhiskersLength { get; set; }
    }
}
