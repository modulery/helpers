using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moduler.Helpers
{
    public static class ParallelHelpers
    {
        public static IEnumerable<IEnumerable<T>> SplitWithMaxCount<T>(this IEnumerable<T> list, int maxCount)
        {
            var splitedList = new List<List<T>>();
            for (int i = 0; i < list.Count(); i += maxCount)
            {
                var takecount = maxCount;
                var skipCount = i;

                var partList = list.Skip(skipCount).Take(takecount).ToList();
                splitedList.Add(partList);
            }
            return splitedList;
        }
    }
}
