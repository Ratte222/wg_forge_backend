using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Interface
{
    public interface IRepository<T> where T : class
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
