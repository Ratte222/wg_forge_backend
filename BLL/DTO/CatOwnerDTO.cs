using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
namespace BLL.DTO
{
    public class CatOwnerDTO
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public List<Cat> Cats { get; set; }
    }
}
