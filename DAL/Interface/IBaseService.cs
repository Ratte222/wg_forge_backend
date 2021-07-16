using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Entities;
namespace DAL.Interface
{
    public interface IBaseService<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll_Enumerable();
        IQueryable<T> GetAll_Queryable();
        T Get(long id);
        void Create(T item);
        void CreateRange(IEnumerable<T> items);
        void Update(T item);
        void Delete(T item);
    }
}
