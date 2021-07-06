using System;
using System.Collections.Generic;
using System.Linq;
using DAL.EF;
using DAL.Entities;

namespace BLL.BusinessModels
{
    public class Exercises
    {
        public void ProcessingExercise1(CatContext db)
        {
            List<CatColorInfo> catInfoNow = db.CatColorInfos.ToList();
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
        public void ProcessingExercise2(CatContext db)
        {
            List<Cat> cats = db.Cats.OrderBy(i => i.TailLength).ToList();
            CatStat stats = new CatStat();
            stats.TailLengthMean = (decimal)cats.Average(i => i.TailLength);
            stats.WiskersLengthMean = (decimal)cats.Average(i => i.WhiskersLength);
            #region Mediana
            if (cats.Count % 2 == 0)
            {
                stats.TailLengthMedian = cats[(cats.Count + 1) / 2].TailLength;
                cats = cats.OrderBy(i => i.WhiskersLength).ToList();
                stats.WiskersLengthMedian = cats[(cats.Count + 1) / 2].WhiskersLength;
            }
            else
            {
                int index = cats.Count / 2;
                stats.TailLengthMedian = (cats[index].TailLength + cats[index + 1].TailLength) / 2;
                cats = cats.OrderBy(i => i.WhiskersLength).ToList();
                stats.WiskersLengthMedian = (cats[index].WhiskersLength + cats[index + 1].WhiskersLength) / 2;
            }
            #endregion
            #region Mode
            double[] vs = (from i in cats
                           select Convert.ToDouble(i.TailLength)).ToArray();
            stats.TailLengthMode = Convert.ToInt32(Mode(vs));
            vs = (from i in cats
                           select Convert.ToDouble(i.WhiskersLength)).ToArray();
            stats.WiskersLengthMode = Convert.ToInt32(Mode(vs));
            #endregion
            if(db.CatStats.Count() > 0)
            {
                stats.Id = 1;
                db.CatStats.Update(stats);
            }
            else
                db.CatStats.Add(stats);
            db.SaveChanges();
        }

        static double Mode(double[] arr)
        {
            if (arr.Length == 0)
                throw new ArgumentException("Маccив не может быть пустым");

            Dictionary<double, int> dict = new Dictionary<double, int>();
            foreach (double elem in arr)
            {
                if (dict.ContainsKey(elem))
                    dict[elem]++;
                else
                    dict[elem] = 1;
            }

            int maxCount = 0;
            double mode = Double.NaN;
            foreach (double elem in dict.Keys)
            {
                if (dict[elem] > maxCount)
                {
                    maxCount = dict[elem];
                    mode = elem;
                }
            }

            return mode;
        }
    }
}
