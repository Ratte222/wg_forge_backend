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
    public class CatService : BaseService<Cat>, ICatService
    {
        public CatService(CatContext catContext) : base(catContext) { }

        public override IQueryable<Cat> GetAll_Queryable()
        {
            return base.GetAll_Queryable().Include(i => i.CatPhotos).AsNoTracking();
        }

        public override IEnumerable<Cat> GetAll_Enumerable()
        {
            return base.GetAll_Queryable().Include(i => i.CatPhotos).AsNoTracking().AsEnumerable();
        }

        public IQueryable<Cat> GetAll()
        {
            return base.GetAll_Queryable();
        }

        public override Cat Get(long id)
        {
            return base.Get(id);
        }

        public override void Create(Cat model)
        {
            base.Create(model);
        }

        public override void CreateRange(IEnumerable<Cat> items)
        {
            base.CreateRange(items);
        }

        public override void Update(Cat item)
        {
            base.Update(item);
        }

        public override void Delete(Cat item)
        {
            base.Delete(item);
        }
    }
}
