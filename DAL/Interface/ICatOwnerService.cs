using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Entities;
namespace DAL.Interface
{
    public interface ICatOwnerService : IBaseService<CatOwner>
    {
        IQueryable<CatOwner> GetAll();
    }
}
