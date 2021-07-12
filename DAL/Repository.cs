using System;
using System.Collections.Generic;
using DAL.Interface;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly CatContext context;
        protected DbSet<T> entities;
        //string errorMessage = string.Empty;

        public Repository(CatContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        
        public T Get(long id)
        {
            //return entities.SingleOrDefault(s => s.Id == id);
            return entities.Find(id);
        }

        public IEnumerable<T> GetAll_Enumerable()
        {
            return entities.AsEnumerable();
        }

        public IQueryable<T> GetAll_Queryable()
        {
            return entities;
        }

        public void Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void CreateRange(IEnumerable<T> entitys)
        {
            if (entitys == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.AddRange(entitys);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }
        public virtual void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges(); 
        }

        //private bool disposed = false;

        //public virtual void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            context.Dispose();
        //        }
        //        this.disposed = true;
        //    }
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
    }
}
