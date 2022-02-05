namespace Inverse.Domain.Model
{
    public class Column : Entity<string>, IVisualElement
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Table Table { get; set; }
        public bool Required { get; set; }
        public virtual bool IsPrimaryKey { get; }
        public virtual bool IsForeignKey { get; }
        public virtual string Prefix { get; } = string.Empty;

        public int Width => Table.Width;

        public int Height => LayoutDefinition.Columns.HEIGHT;

        public int Left => Table.Left;

        public int Right => Table.Right;

        public int Top => Table.Top + (Index * 30);

        public int Bottom => Top + Height;

        public int Center => Table.Center;

        public int Middle => Top + (Height / 2);

        public bool IsHover(int x, int y)
        {
            return x.IsBetween(Left, Right) && y.IsBetween(Top, Bottom);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
