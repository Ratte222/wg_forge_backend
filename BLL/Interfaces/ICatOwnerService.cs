using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Entities;
namespace BLL.Interfaces
{
    public interface ICatOwnerService
    {
        IQueryable<CatOwner> GetAll();
        IEnumerable<CatOwner> GetAll_Enumerable();
        IQueryable<CatOwner> GetAll_Queryable();
        CatOwner Get(string id);
        void Create(CatOwner item);
        void CreateRange(IEnumerable<CatOwner> items);
        void Update(CatOwner item);
        void Delete(CatOwner item);
        
    }
}
