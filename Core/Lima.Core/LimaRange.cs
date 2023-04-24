using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Core
{
    public class LimaRange
    {
        public int[] Range { get; set; }

        public static implicit operator LimaRange(int[] range)
        {
            return new LimaRange() { Range = range };
        }

        public static explicit operator int[](LimaRange limaRange)
        {
            return limaRange.Range;
        }

        public int Start() => Range.Length == 0 ? 0 : Range[0];
        public int End() => Range.Length == 0 ? 0 : Range[Range.Length - 1];
    }
}
