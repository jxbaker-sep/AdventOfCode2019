using System.Collections.Generic;

namespace AdventOfCode2019.Utils
{
    public static class LongEnumerable
    {
        public static IEnumerable<long> Range(long start, long count)
        {
            for(var current = 0L; current < count; count++) yield return start + current;

        }
    }
}