using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class CatOwnerDTO
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public List<CatDTO> Cats { get; set; }
    }
}
