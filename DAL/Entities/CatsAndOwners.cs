using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class CatsAndOwners
    {
        public int CatOwnersId { get; set; }
        public CatOwner CatOwner { get; set; }

        public string CatsName { get; set; }
        public Cat Cat { get; set; }
    }
}
