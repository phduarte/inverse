using System;

namespace EngenhariaReversaDb.Domain
{
    public class Column : Entity<Guid>
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Table Table { get; set; }
        public bool Required { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
