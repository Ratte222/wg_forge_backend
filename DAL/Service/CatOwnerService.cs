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
    public class CatOwnerService : BaseService<CatOwner>, ICatOwnerService
    {
        public CatOwnerService(CatContext catContext) : base(catContext) { }
        
        public override IQueryable<CatOwner> GetAll_Queryable()
        {
            return base.GetAll_Queryable().Include(i=>i.CatsAndOwners).ThenInclude(i=>i.Cat).AsNoTracking();
        }

        public override IEnumerable<CatOwner> GetAll_Enumerable()
        {
            return base.GetAll_Queryable().Include(i => i.CatsAndOwners).ThenInclude(i => i.Cat)
                .AsNoTracking().AsEnumerable();
        }

        public override CatOwner Get(long id)
        {
            return base.Get(id);
        }

        public override void Create(CatOwner model)
        {
            base.Create(model);
        }

        public override void CreateRange(IEnumerable<CatOwner> items)
        {
            base.CreateRange(items);
        }

        public override void Update(CatOwner item)
        {
            base.Update(item);
        }

        public override void Delete(CatOwner item)
        {
            base.Delete(item);
        }
    }
}
