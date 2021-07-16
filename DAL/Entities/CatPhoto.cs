using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class CatPhoto:BaseEntity
    {
        //public int Id { get; set; }
        public string CatPhotoName { get; set; }

        public long CatId { get; set; }
        public Cat Cat { get; set; }
    }
}
