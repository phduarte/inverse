using System.Drawing;
using System.IO;
using System.Text.Json;

namespace Inverse.Desktop
{
    internal class ThemeManager
    {
        public static Theme Load(string themeName = null)
        {
            var filename = string.IsNullOrEmpty(themeName) ? $"theme.json" : $"theme.{themeName}.json";
            var json = File.ReadAllText(filename);
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            var theme = JsonSerializer.Deserialize<Theme>(json, options);

            return theme;
        }

        public static void Save(Theme themeLevel1, string themeName = "Theme.json")
        {
            var json = JsonSerializer.Serialize(themeLevel1);

            File.WriteAllText(themeName, json);
        }
    }

    internal class Theme
    {
        public ThemeItem Canvas { get; set; }
        public ThemeItem Table { get; set; }
        public ThemeItem Balloon { get; set; }
        public ThemeItem Column { get; set; }
        public ThemeItem Prefix { get; set; }
        public ThemeItem Type { get; set; }
        public ThemeItem Relationship { get; set; }
    }

    internal class ThemeItem
    {
        public ThemeConfiguration Background { get; set; }
        public ThemeConfiguration Text { get; set; }
        public ThemeConfiguration Line { get; set; }
        public ThemeConfiguration Arrow { get; set; }
    }

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
