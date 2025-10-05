using System;
using System.Collections.Generic;
using System.Linq;

namespace Inverse.Domain;

public class Database : Entity<Guid>, IAggregateRoot
{
    public delegate void DatabaseAddedTableEventHandler(Table table);

    private int cindex = 0;

    private readonly List<Table> _tables = new();

    public string Name { get; set; }
    public string ConnectionString { get; set; }
    public Provider Provider { get; set; } = Provider.Undefined;
    public IReadOnlyList<Table> Tables => _tables;
    public bool IsEmpty => Tables.Count == 0;

    public event DatabaseAddedTableEventHandler OnTableAdded;

    public Database()
    {
        Id = Guid.NewGuid();
    }

    public void AddTable(Table table)
    {
        table.Index = cindex++;
        table.Database = this;
        _tables.Add(table);
        OnTableAdded?.Invoke(table);
    }

    public void AddTables(params Table[] tables)
    {
        foreach (var t in tables)
        {
            AddTable(t);
        }
    }

    public void BringTableToFront(Table table)
    {
        cindex = 1;

        foreach (var t in _tables.Except(new[] { table }).OrderBy(x => x.Index))
        {
            t.Index = cindex++;
        }

        table.Index = 0;
    }

    public void SendTableToBack(Table table)
    {
        cindex = 0;

        foreach (var t in _tables.Except(new[] { table }).OrderBy(x => x.Index))
        {
            t.Index = cindex++;
        }

        table.Index = _tables.Max(t => t.Index) + 1;
    }

    public void RemoveTable(Table activeTable)
    {
        _tables.Remove(activeTable);

        foreach (var c in _tables
            .SelectMany(t => t.Columns)
            .OfType<ForeignKey>()
            .Where(fk => fk.RelatedTable == activeTable.Name)
            )
        {
            c.RemoveRelation();
        }
    }

    public Column GetColumnByPosition(int x, int y)
    {
        return Tables.SelectMany(x => x.Columns).LastOrDefault(col => col.IsHover(x, y));
    }

    public Table GetTableByPosition(int x, int y)
    {
        var table = Tables.LastOrDefault(f => f.IsHover(x, y));

        if (table != null)
        {
            table.Index = _tables.Max(x => x.Index);
            var tables = _tables.OrderBy(x => x.Index).ToList();
            var idx = 0;

            foreach (var t in tables.Except(new Table[] { table }))
            {
                t.Index = idx++;
            }

            table.Index = idx;
        }

        return table;
    }

    public IEnumerable<Table> GetAllTablesOrderedByDependency()
    {
        var sorted = new List<Table>();
        void addTable(Table t)
        {
            if (!sorted.Any(s => s.Name.Equals(t.Name, StringComparison.OrdinalIgnoreCase)))
            {
                sorted.Add(t);
            }
        }
        ;

        foreach (var table in Tables)
        {
            var referenedTables = table.ForeignKeys
                .Select(fk => Tables.FirstOrDefault(t => t.Name.Equals(fk.RelatedTable)))
                .Where(t => t != null);

            foreach (var refTable in referenedTables ?? Enumerable.Empty<Table>())
            {
                addTable(refTable);
            }

            addTable(table);
        }

        return sorted;
    }

    public override string ToString()
    {
        return ConnectionString;
    }
}