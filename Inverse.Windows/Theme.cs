using System.Drawing;

namespace Inverse.Windows
{
    internal static class Theme
    {
        public static class Table
        {
            public static Line Border { get; } = new Line();
            public static Line Line { get; } = new Line();
            public static Line ColumnSeparator { get; } = new Line();
            public static Line BorderSelected { get; } = new Line();
            public static Background Column { get; } = new Background();
            public static Background Title { get; } = new Background(Brushes.Gray);
            public static Background SelectedColumn { get; } = new Background(Brushes.LightYellow);
            public static Text TitleFont { get; } = new Text(Brushes.White);
            public static Text Font { get; } = new Text(Brushes.Black);
            public static Text ForeignKeyColor { get; } = new Text(Brushes.Black);

            public static void SetBorderSize(int size)
            {
                Border.Size = size;
                BorderSelected.Size = size;
            }

            public static void SetForegroundColor(Brush brush)
            {
                Border.Color = brush;
            }

            public static void SetSelectedColor(Brush brush)
            {
                SelectedColumn.Color = brush;
            }

            public static void SetSelectedBorderColor(Brush brush)
            {
                BorderSelected.Color = brush;
            }

            public static void SetTitle(Brush color, Brush backgroundColor)
            {
                TitleFont.Color = color;
                Title.Color = backgroundColor;
            }
        }
    }

    public class Text
    {
        public int Size { get; set; } = 10;
        public Brush Color { get; set; }

        public Text()
        {
            Color = Brushes.Black;
        }

        public Text(Brush brush)
        {
            Color = brush;
        }

        public static implicit operator Brush(Text text)
        {
            return text.Color;
        }
    }

    public class Background
    {
        public Brush Color { get; set; } = Brushes.White;

        public Background()
        {

        }

        public Background(Brush brush)
        {
            Color = brush;
        }

        public static implicit operator Brush(Background border)
        {
            return border.Color;
        }

        public static implicit operator Background(Brush brush)
        {
            return new Background(brush);
        }
    }

    public class Line
    {
        public int Size { get; set; } = 1;
        public Brush Color { get; set; } = Brushes.Black;

        public static implicit operator Pen(Line border)
        {
            return new(border.Color, border.Size);
        }

        public static implicit operator Brush(Line border)
        {
            return border.Color;
        }
    }
}
