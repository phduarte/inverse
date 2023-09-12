using System;
using System.Collections.Generic;
using System.Linq;
using Inverse.Extensions;
using static System.Math;

namespace Inverse.Domain
{
    public class Table : Entity<string>, IDraggableElement, IAggregateRoot
    {
        readonly List<Column> _columns = new();
        readonly List<Comment> _comments = new();

        public const int HEIGHT = 30;
        public const int WIDTH = 100;
        public const int MARGIN = 50;
        public const int PREFIX_WIDTH = 40;
        public const int TYPE_WIDTH = 90;

        public string Name { get; set; }
        public IReadOnlyList<Comment> Comments => _comments;
        public IReadOnlyList<Column> Columns => _columns;
        public int Width { get; private set; } = WIDTH;
        public int Height { get; private set; } = HEIGHT;
        public int Left { get; set; }
        public int Right => Left + Width;
        public int Top { get; set; }
        public int Bottom => Top + Height;
        public int Center => Left + Width / 2;
        public int Middle => Top + Height / 2;
        public int ForeignKeysCount => ForeignKeys.Count();
        public int PrimaryKeysCount => PrimaryKeys.Count();
        public IEnumerable<IPrimaryKey> PrimaryKeys => Columns.Where(_ => _.IsPrimaryKey).Cast<IPrimaryKey>();
        public IEnumerable<IForeignKey> ForeignKeys => Columns.Where(_ => _.IsForeignKey).Cast<IForeignKey>();
        public bool IsHidden { get; set; }
        public Database Database { get; set; }
        public int Index { get; set; }
        public bool IsModified { get; set; }
        public string Notes => Comment.ToNotes(Comments);

        public Table()
        {
            Id = Guid.NewGuid().ToString();
        }

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
                        if (_columns[i] is PrimaryKey && column is ForeignKey fk)
                        {
                            _columns[i] = ForeignPrimaryKey.Parse(fk);
                        }
                        else
                        {
                            _columns[i] = column;
                        }

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

        public void AddRange(params Column[] columns)
        {
            foreach (var c in columns)
            {
                Add(c);
            }
        }

        public void AddRange(IEnumerable<Column> enumerable)
        {
            enumerable.ToList().ForEach(e => Add(e));
        }

        public void Add(Comment comment)
        {
            comment.Table = this;
            _comments.Add(comment);
        }

        public void AddRange(IEnumerable<Comment> comments)
        {
            foreach (var comment in comments)
            {
                Add(comment);
            }
        }

        public void Clear()
        {
            _columns.Clear();
            _comments.Clear();
            IsModified = true;
        }

        public void MoveTo(int x, int y)
        {
            Left = Max(0, x);
            Top = Max(0, y);
        }
        public bool CanMoveOffset(int offsetX, int offsetY) => Left + offsetX > -1 && Top + offsetY > -1;

        public virtual void MoveOffset(int x, int y)
        {
            MoveTo(Left + x, Top + y);
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
            var max = Columns.Max(x => x.Name.Length);
            Width = Max(Name.Length, max) * LayoutDefinition.Chars.WIDTH + PREFIX_WIDTH + TYPE_WIDTH;
            Height = Columns.Sum(x => x.Height) + HEIGHT;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
