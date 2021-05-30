using System;
using System.Collections.Generic;

namespace EngenhariaReversaDb.Domain
{
    public class Database : Entity<Guid>
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }

        public List<Table> Tables { get; set; } = new List<Table>();

        public override string ToString()
        {
            return ConnectionString;
        }
    }
}
