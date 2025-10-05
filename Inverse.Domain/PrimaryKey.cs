namespace Inverse.Domain;

public class PrimaryKey : Column, IPrimaryKey
{
    public override bool IsPrimaryKey => true;
    public override string Prefix => PRIMARY_KEY_PREFIX;

    public static PrimaryKey Parse(Column column)
    {
        return new PrimaryKey()
        {
            Index = column.Index,
            Name = column.Name,
            Description = column.Description,
            Type = column.Type,
            Table = column.Table,
            IsRequired = true,
            Id = column.Id,
            DefaultValue = column.DefaultValue
        };
    }
}