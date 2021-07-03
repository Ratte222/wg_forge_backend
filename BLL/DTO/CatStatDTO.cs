using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class CatStatDTO
    {
        public decimal TailLengthMean { get; set; }
        public decimal TailLengthMedian { get; set; }
        public int TailLengthMode { get; set; }
        public decimal WiskersLengthMean { get; set; }
        public decimal WiskersLengthMedian { get; set; }
        public int WiskersLengthMode { get; set; }
    }
}
