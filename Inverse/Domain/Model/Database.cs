using System;
using System.Collections.Generic;
using System.Linq;

namespace Inverse.Domain.Model
{
    public class Database : Entity<Guid>
    {
        private int cindex = 0;

        private readonly List<Table> _tables = new();

        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public Provider Provider { get; }
        public IReadOnlyList<Table> Tables => _tables;
        public bool IsEmpty => string.IsNullOrEmpty(ConnectionString) || Tables.Count == 0;

        public Database(Provider provider)
        {
            Provider = provider;
        }

        public override string ToString()
        {
            return ConnectionString;
        }

        public void Add(Table table)
        {
            table.Index = cindex++;
            table.Database = this;
            _tables.Add(table);
        }

        public void AddRange(params Table[] tables)
        {
            foreach (var t in tables)
            {
                Add(t);
            }
        }

        public void BringToFront(Table table)
        {
            cindex = 1;

            foreach (var t in _tables.Except(new[] { table }).OrderBy(x => x.Index))
            {
                t.Index = cindex++;
            }

            table.Index = 0;
        }

        public void SendToBack(Table table)
        {
            cindex = 0;

            foreach (var t in _tables.Except(new[] { table }).OrderBy(x => x.Index))
            {
                t.Index = cindex++;
            }

            table.Index = _tables.Max(t => t.Index) + 1;
        }
    }
}
