using System.Collections.Generic;

namespace Inverse.Extensions
{
    public static class IsBetweenExtensions
    {
        public static bool IsBetween(this int item, int start, int end)
        {
            return item.CompareIsBetween(start, end);
        }

        public static bool IsBetween(this decimal item, decimal start, decimal end)
        {
            return item.CompareIsBetween(start, end);
        }

        public static bool IsBetween(this long item, long start, long end)
        {
            return item.CompareIsBetween(start, end);
        }

        public static bool IsBetween(this string item, string start, string end)
        {
            return item.CompareIsBetween(start, end);
        }

        private static bool CompareIsBetween<T>(this T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;
        }
    }
}
