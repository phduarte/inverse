using System;
using System.Collections.Generic;

namespace EngenhariaReversaDb.Domain.Model
{
    public class Database : Entity<Guid>
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public Provider Provider { get; }
        public List<Table> Tables { get; set; } = new List<Table>();
        public bool IsEmpty => string.IsNullOrEmpty(ConnectionString) || Tables.Count == 0;

        public Database(Provider provider)
        {
            Provider = provider;
        }

        public override string ToString()
        {
            return ConnectionString;
        }
    }
}
