using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDB_Benchmark
{
    public static class Avg
    {
        /// <summary>
        /// This Average without Max/Min values
        /// </summary>
        public static double Avg2(this List<Dictionary<string, long>> list, string key)
        {
            var max = list.Max(x => x[key]);
            var min = list.Min(x => x[key]);
            var nlist = new List<Dictionary<string, long>>(list);

            nlist.Remove(nlist.First(x => x[key] == max));
            nlist.Remove(nlist.First(x => x[key] == min));

            return nlist.Average(x => x[key]);
        }
    }
}
