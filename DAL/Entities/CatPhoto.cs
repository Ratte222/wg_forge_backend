using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class CatPhoto
    {
        public int Id { get; set; }
        public string CatPhotoName { get; set; }

        public string CatName { get; set; }
        public Cat Cat { get; set; }
    }
}
