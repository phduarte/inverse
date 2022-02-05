using System;
using System.Collections.Generic;

namespace Inverse.Domain.Model
{
    public class Database : Entity<Guid>
    {
        private List<Table> _tables = new List<Table>();

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
            table.Database = this;
            _tables.Add(table);
        }
    }
}
