using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class CatsAndOwners
    {
        public long CatOwnersId { get; set; }
        public CatOwner CatOwner { get; set; }

        public long CatsId { get; set; }
        public Cat Cat { get; set; }
    }
}
