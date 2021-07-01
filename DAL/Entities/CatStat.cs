using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities
{
    [Table("cats_stat")]
    public class CatStat
    {
        [Column("tail_length_mean")]
        public decimal TailLengthMean { get; set; }
        [Column("tail_length_median")]
        public decimal TailLengthMedian { get; set; }
        [Column("tail_length_mode")]
        public int TailLengthMode { get; set; }
        [Column("whiskers_length_mean")]
        public decimal WiskersLengthMean { get; set; }
        [Column("whiskers_length_median")]
        public decimal WiskersLengthMedian { get; set; }
        [Column("whiskers_length_mode")]
        public int WiskersLengthMode { get; set; }
    }
}
