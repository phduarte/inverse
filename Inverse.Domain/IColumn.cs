namespace Inverse.Domain
{
    public interface IColumn
    {
        int Bottom { get; }
        int Center { get; }
        int Height { get; }
        int Index { get; set; }
        bool IsForeignKey { get; }
        bool IsPrimaryKey { get; }
        int Left { get; }
        int Middle { get; }
        string Name { get; set; }
        string Prefix { get; }
        bool IsRequired { get; set; }
        int Right { get; }
        Table Table { get; set; }
        int Top { get; }
        string Type { get; set; }
        int Width { get; }

        bool IsHover(int x, int y);

        string ToString();
    }
}