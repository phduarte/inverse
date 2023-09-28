using System.Drawing;

namespace Inverse.Desktop.Themes
{
    internal class ThemeConfiguration
    {
        public int Size { get; set; }
        public string Normal { get; set; }
        public string Hover { get; set; }
        public string Selected { get; set; }

        public Color AsColor(bool isHover = false, bool isSelected = false)
        {
            var color = isSelected ? Selected : isHover ? Hover : Normal;
            return ColorTranslator.FromHtml(color);
        }

        public Pen AsPen(bool isHover = false, bool isSelected = false)
        {
            return new Pen(AsColor(isHover, isSelected), Size);
        }

        public Brush AsBrush(bool isHover = false, bool isSelected = false)
        {
            return new SolidBrush(AsColor(isHover, isSelected));
        }
    }
}