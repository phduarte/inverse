using System.Drawing;

namespace Inverse.Desktop
{
    internal static class Theme
    {
        public static class Table
        {
            public static Text Text { get; } = new Text(Brushes.Black);
            public static Background Background { get; set; } = new Background(Brushes.White);
            public static Text ForeignKeyText { get; } = new Text { Color = Brushes.Black, SelectedColor = Brushes.Blue };
            public static Line Border { get; } = new Line(Brushes.Black);
            public static Line Separator { get; } = new Line(Brushes.Transparent);

            public static void SetTextColor(Brush brush)
            {
                Text.Color = brush;
                Column.Text.Color = brush;
            }

            public static void SetTextColorSelected(Brush brush)
            {
                Column.Text.SelectedColor = brush;
            }

            public static void SetBackgroundColor(Brush color)
            {
                Background.Color = color;
            }

            public static void SetBackgroundColorSelected(Brush color)
            {
                Background.SelectedColor = color;
            }

            public static void SetBorderSize(int size)
            {
                Border.Size = size;
            }

            public static void SetBorderColor(Brush brush)
            {
                Border.Color = brush;
            }

            public static void SetBorderColorSelected(Brush brush)
            {
                Border.SelectedColor = brush;
            }

            public static void SetTitle(Brush fontColor, Brush backgroundColor)
            {
                Title.SetTheme(backgroundColor, fontColor);
            }

            public static class Title
            {
                public static Text Text { get; set; } = new Text(Brushes.Black);
                public static Background Background { get; set; } = new Background(Brushes.White);
                public static Background Activated { get; set; } = new Background(Brushes.LightYellow);

                public static void SetTheme(Brush backgroundColor, Brush fontColor)
                {
                    Background.Color = backgroundColor;
                    Text.Color = fontColor;
                }
            }

            public static class Column
            {
                public static Text Text { get; set; } = new Text(Brushes.Black);
                public static Background Background { set; get; } = new Background(Brushes.Transparent);
            }
        }
    }

    public class Text
    {
        public int Size { get; set; } = 10;
        public Color Color { get; set; } = Brushes.Black;
        public Color SelectedColor { get; set; } = Brushes.Red;

        public Text() { }

        public Text(Brush brush)
        {
            Color = brush;
        }

        //public static implicit operator Brush(Text text)
        //{
        //    return text.Color;
        //}
    }

    public class Background
    {
        public Color Color { get; set; } = Brushes.Transparent;
        public Color SelectedColor { get; set; } = Brushes.Transparent;

        public Background() { }

        public Background(Brush brush)
        {
            Color = brush;
        }

        public void SetColor(Brush color, Brush selectedColor)
        {
            Color = color;
            SelectedColor = selectedColor;
        }
    }

    public class Line
    {
        public int Size { get; set; } = 1;
        public Color Color { get; set; } = Brushes.Black;
        public Color SelectedColor { get; set; } = Brushes.Red;

        public Line(Brush color)
        {
            Color = color;
        }

        public Pen GetPen(bool isSelected = false)
        {
            return new Pen(isSelected ? SelectedColor : Color, Size);
        }
    }

    public class Color
    {
        public Brush Brush { get; set; }

        public static implicit operator Pen(Color color)
        {
            return new(color.Brush);
        }

        public static implicit operator Brush(Color color)
        {
            return color.Brush;
        }

        public static implicit operator Color(Brush brush)
        {
            return new Color { Brush = brush };
        }

        public override string ToString()
        {
            return Brush.ToString();
        }
    }
}
