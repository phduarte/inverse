using System;
using System.Collections.Generic;
using System.Linq;

namespace EngenhariaReversaDb.Domain
{
    public class Table : Entity<Guid>
    {
        readonly List<Column> _columns = new List<Column>();

        public string Name { get; set; }

        public IReadOnlyList<Column> Columns => _columns;

        public Database Database { get; set; }

        public void Add(Column column)
        {
            _columns.Add(column);
        }

        public void Add(PrimaryKey primaryKey)
        {
            _columns.Add(primaryKey);
        }

        public void Add(ForeignKey column)
        {
            if (_columns.Any(x => x.Id.Equals(column.Id)))
            {
                for (var i = 0; i < _columns.Count; i++)
                {
                    if (_columns[i].Id.Equals(column.Id))
                    {
                        _columns[i] = column;
                        break;
                    }
                }
            }
            else
            {
                _columns.Add(column);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
