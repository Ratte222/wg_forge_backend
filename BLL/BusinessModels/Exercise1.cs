using System;
using System.Collections.Generic;
using System.Linq;
using DAL.EF;
using DAL.Entities;
namespace BLL.BusinessModels
{
    public class Exercise1
    {
        public void ProcessingExercise1(CatContext db)
        {
            IEnumerable<CatColorInfo> query = db.Cats.GroupBy(
                cat => cat.Color,
                cat => cat.Color,
                (colorKey, color) => new CatColorInfo
                {
                    Color = colorKey,
                    Count = color.Count()
                }).AsEnumerable();
            db.CatColorInfos.AddRange(query);
            db.SaveChanges();
        }
    }
}
