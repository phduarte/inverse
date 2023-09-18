using Inverse.Extensions;
using System;

namespace Inverse.Domain
{
    public class Column : Entity<string>, IColumn, IVisualElement
    {
        public const int HEIGHT = 30;
        public const int TITLE_MARGIN = 30;
        public const string PRIMARY_KEY_PREFIX = "PK";
        public const string FOREIGN_KEY_PREFIX = "FK";
        public const string FOREIGN_PRIMART_KEY_PREFIX = "PK FK";
        public const int PREFIX_WIDTH = 40;
        public const int TYPE_WIDTH = 90;

        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public Table Table { get; set; }
        public string DefaultValue { get; set; }
        public bool IsRequired { get; set; }
        public virtual bool IsPrimaryKey { get; }
        public virtual bool IsForeignKey { get; }
        public virtual string Prefix { get; } = string.Empty;

        public int Width => Table.Width;

        public int Height => HEIGHT;

        public int Left => Table.Left;

        public int Right => Table.Right;

        public int Top => Table.Top + Index * 30;

        public int Bottom => Top + Height;

        public int Center => Table.Center;

        public int Middle => Top + Height / 2;

        public Column()
        {
            Id = Guid.NewGuid().ToString();
        }

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
