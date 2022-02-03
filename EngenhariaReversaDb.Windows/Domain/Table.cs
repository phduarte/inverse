using System.Collections.Generic;
using System.Linq;

namespace EngenhariaReversaDb.Domain
{
    public class Table : Entity<string>, IDraggableElement
    {
        readonly List<Column> _columns = new();

        public string Name { get; set; }

        public IReadOnlyList<Column> Columns => _columns;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Left { get; set; }
        public int Right => Left + Width;
        public int Top { get; set; }
        public int Bottom => Top + Height;
        public int Center => Left + (Width / 2);
        public int Middle => Top + (Height / 2);

        public Database Database { get; set; }

        public Table()
        {
            Width = 100;
            Height = 30;
        }

        public void Add(Column column)
        {
            column.Index = _columns.Count + 1;
            _columns.Add(column);

            Resize();
        }

        public void Add(PrimaryKey column)
        {
            column.Index = _columns.Count + 1;
            _columns.Add(column);

            Resize();
        }

        public void Add(ForeignKey column)
        {
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

        internal void AddRange(IEnumerable<Column> enumerable)
        {
            foreach (var e in enumerable)
            {
                if (e is ForeignKey fk)
                {
                    Add(fk);
                }
                else
                {
                    Add(e);
                }
            }
        }

        public void MoveTo(int x, int y)
        {
            if (x < 0 || y < 0) return;

            Left = x;
            Top = y;
        }

        public virtual void MoveOffset(int x, int y)
        {
            if (x < 0 || y < 0) return;

            Left += x;
            Top += y;
        }

        public bool IsHover(int x, int y)
        {
            return x > Left && x < Right && y > Top && y < Bottom;
        }

        private void Resize()
        {
            var max = Columns.Select(x => x.Name.Length).Max();
            Width = System.Math.Max(Name.Length, max) * 11;
            Height = Columns.Sum(x => x.Height) + 30;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
