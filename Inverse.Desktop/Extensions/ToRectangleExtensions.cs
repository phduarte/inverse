using Inverse.Domain;
using System.Drawing;

namespace Inverse.Desktop.Extensions;

internal static class ToRectangleExtensions
{
    public static Rectangle ToRectangle(this Table table)
    {
        return new Rectangle(table.Left, table.Top, table.Width, table.Height);
    }
}
