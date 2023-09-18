using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Inverse.Desktop.Themes
{
    internal class ThemeManager
    {
        public static Theme Load(string themeName = null)
        {
            var json = File.ReadAllText(GetThemeFileName(themeName));

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            var theme = JsonSerializer.Deserialize<Theme>(json, options);

            return theme;
        }

        public static void Save(Theme theme, string themeName = "")
        {
            var json = JsonSerializer.Serialize(theme);

            File.WriteAllText(GetThemeFileName(themeName), json);
        }

        public static IEnumerable<string> ListNames()
        {
            return Directory
                .GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes"))
                .Select(x => new FileInfo(x))
                .Where(x => x.Name.StartsWith("Theme") && x.Extension.Equals(".json"))
                .Select(x => ExtractThemeNameFromFileName(x.Name))
                .Where(x => !string.IsNullOrEmpty(x))
                ;
        }

        private static string GetThemeFileName(string themeName)
        {
            var filename = string.IsNullOrEmpty(themeName) ? $"theme.json" : $"theme.{themeName}.json";
            var fullname = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes", filename);

            return fullname;
        }

        private static string ExtractThemeNameFromFileName(string themeName)
        {
            var split = themeName.Split('.');

            if (split.Length < 3)
            {
                return string.Empty;
            }

            return split[1];
        }
    }
}
