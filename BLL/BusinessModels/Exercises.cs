using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Interface;
using DAL.Entities;

namespace BLL.BusinessModels
{
    public class Exercises
    {
        public void ProcessingExercise1(IRepository<Cat> repoCat, IRepository<CatColorInfo> repoCatColorInfo)
        {
            List<CatColorInfo> catInfoNow = repoCatColorInfo.GetAll_Enumerable().ToList();
            if(catInfoNow.Count == 0)
            {
                IEnumerable<CatColorInfo> query = repoCat.GetAll_Queryable().GroupBy(
                cat => cat.Color,
                cat => cat.Color,
                (keyColor, color) => new CatColorInfo
                {
                    Color = keyColor,
                    Count = color.Count()
                }).AsEnumerable();

                repoCatColorInfo.CreateRange(query);

            }            
        }
        public void ProcessingExercise2(IRepository<Cat> repoCat, IRepository<CatStat> repoCatStat)
        {
            List<Cat> cats = repoCat.GetAll_Queryable().OrderBy(i => i.TailLength).ToList();
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
            if(repoCatStat.GetAll_Queryable().Count() > 0)
            {
                stats.Id = 1;
                repoCatStat.Update(stats);
            }
            else
                repoCatStat.Create(stats);
            
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
