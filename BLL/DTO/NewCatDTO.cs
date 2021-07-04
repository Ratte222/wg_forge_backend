using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace BLL.DTO
{
    public class NewCatDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("color")]
        public string Color { get; set; }
        [JsonPropertyName("tail_length"), Range(0, 43, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int TailLength { get; set; }
        [JsonPropertyName("whiskers_length"), Range(0, 20, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int WhiskersLength { get; set; }
    }
}
