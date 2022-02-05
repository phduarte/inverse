using System.Collections.Generic;
using System.Linq;

namespace Inverse.Domain.Model
{
    public class Table : Entity<string>, IDraggableElement
    {
        readonly List<Column> _columns = new();

        public string Name { get; set; }
        public IReadOnlyList<Column> Columns => _columns;
        public int Width { get; private set; } = LayoutDefinition.Tables.WIDTH;
        public int Height { get; private set; } = LayoutDefinition.Columns.HEIGHT;
        public int Left { get; set; }
        public int Right => Left + Width;
        public int Top { get; set; }
        public int Bottom => Top + Height;
        public int Center => Left + (Width / 2);
        public int Middle => Top + (Height / 2);
        public int ForeignKeysCount => ForeignKeys.Count();
        public int PrimaryKeysCount => PrimaryKeys.Count();
        public IEnumerable<PrimaryKey> PrimaryKeys => Columns.OfType<PrimaryKey>();
        public IEnumerable<ForeignKey> ForeignKeys => Columns.OfType<ForeignKey>();
        public bool IsHidden { get; set; }
        public Database Database { get; set; }

        public void Add(Column column)
        {
            column.Table = this;

            if (_columns.Any(x => x.Id.Equals(column.Id)))
            {
                for (var i = 0; i < _columns.Count; i++)
                {
                    if (_columns[i].Id.Equals(column.Id))
                    {
                        column.Index = _columns[i].Index;
                        _columns[i] = column;
                        break;
                    }
                }
            }
            else
            {
                column.Index = _columns.Count + 1;
                _columns.Add(column);
            }

            Resize();
        }

        public void AddRange(IEnumerable<Column> enumerable)
        {
            enumerable.ToList().ForEach(e => Add(e));
        }

        public void MoveTo(int x, int y)
        {
            if (x < 0 || y < 0) return;

            Left = x;
            Top = y;
        }

        public virtual void MoveOffset(int x, int y)
        {
            if (Left + x < 0 || Top + y < 0) return;

            Left += x;
            Top += y;
        }

        public bool IsHover(int x, int y)
        {
            return x.IsBetween(Left, Right) && y.IsBetween(Top, Bottom);
        }

        public void Hide()
        {
            IsHidden = true;
        }

        public void Show()
        {
            IsHidden = false;
        }

        private void Resize()
        {
            var max = Columns.Select(x => x.Name.Length).Max();
            Width = System.Math.Max(Name.Length, max) * LayoutDefinition.Chars.WIDTH;
            Height = Columns.Sum(x => x.Height) + LayoutDefinition.Columns.HEIGHT;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
