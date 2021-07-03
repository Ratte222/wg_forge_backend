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
            List<CatColorInfo> catInfoNow = db.CatColorInfos.Select(i => i).ToList();
            if(catInfoNow.Count == 0)
            {
                IEnumerable<CatColorInfo> query = db.Cats.GroupBy(
                cat => cat.Color,
                cat => cat.Color,
                (keyColor, color) => new CatColorInfo
                {
                    Color = keyColor,
                    Count = color.Count()
                }).AsEnumerable();

                db.CatColorInfos.AddRange(query);

                db.SaveChanges();
            }            
        }

    }
}
