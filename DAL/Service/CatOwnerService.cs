using DAL.Entities;
using DAL.Interface;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DAL.Service
{
    public class CatOwnerService : ICatOwnerService
    {
        protected CatContext _context;
        public CatOwnerService(CatContext catContext)
        {
            _context = catContext;
        }

        public IQueryable<CatOwner> GetAll_Queryable()
        {
            return _context.CatOwners.Include(i => i.CatsAndOwners).ThenInclude(i => i.Cat)
                .ThenInclude(i => i.CatPhotos).AsNoTracking();
        }

        public IQueryable<CatOwner> GetAll()
        {
            return _context.CatOwners.AsNoTracking();
        }

        public IEnumerable<CatOwner> GetAll_Enumerable()
        {
            return _context.CatOwners.Include(i => i.CatsAndOwners).ThenInclude(i => i.Cat)
                .AsNoTracking().AsEnumerable();
        }

        public CatOwner Get(string id)
        {
            return _context.CatOwners.AsNoTracking().Single(i => i.Id == id);
        }

        public void Create(CatOwner model)
        {
            _context.CatOwners.Add(model);
            _context.SaveChanges();
        }

        public void CreateRange(IEnumerable<CatOwner> items)
        {
            _context.CatOwners.AddRange(items);
            _context.SaveChanges();
        }

        public void Update(CatOwner item)
        {
            _context.CatOwners.Update(item);
            _context.SaveChanges();
        }

        public void Delete(CatOwner item)
        {
            _context.CatOwners.Remove(item);
            _context.SaveChanges();
        }
    }
}
