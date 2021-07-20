using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BLL.Interfaces;
using DAL.Entities;
using DAL.EF;
namespace BLL.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected CatContext _context;
        public BaseService(CatContext catContext)
        {
            _context = catContext;
        }

        public virtual IQueryable<T> GetAll_Queryable()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public virtual IEnumerable<T> GetAll_Enumerable()
        {
            return _context.Set<T>().AsNoTracking().AsEnumerable();
        }

        public virtual T Get(long id)
        {
            return _context.Set<T>().AsNoTracking().Single(i=>i.Id == id);
        }

        public virtual void Create(T model)
        {
            _context.Set<T>().Add(model);
            _context.SaveChanges();
        }

        public virtual void CreateRange(IEnumerable<T> items)
        {
            _context.Set<T>().AddRange(items);
            _context.SaveChanges();
        }

        public virtual void Update(T item)
        {
            _context.Set<T>().Update(item);
            _context.SaveChanges();
        }

        public virtual void Delete(T item)
        {
            _context.Set<T>().Remove(item);
            _context.SaveChanges();
        }
    }
}
