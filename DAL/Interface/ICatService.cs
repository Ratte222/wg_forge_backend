using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Entities;
namespace DAL.Interface
{
    public interface ICatService : IBaseService<Cat>
    {
        IQueryable<Cat> GetAll();
    }
}
