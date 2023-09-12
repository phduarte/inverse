using Inverse.Extensions;
using System.Drawing;

namespace Inverse.Desktop
{
    internal static class IsBetweenExtensions
    {
        public static bool IsOverlap(this Point point1, Rectangle rectangle)
        {
            return point1.X.IsBetween(rectangle.Left, rectangle.Right) 
                && point1.Y.IsBetween(rectangle.Top, rectangle.Bottom);
        }
    }
}
