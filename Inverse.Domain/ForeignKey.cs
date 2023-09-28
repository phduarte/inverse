namespace Inverse.Domain
{
    public class ForeignKey : Column, IForeignKey
    {
        public string RelatedTable { get; set; }
        public string RelatedColumn { get; set; }
        public override bool IsForeignKey => true;
        public override string Prefix => FOREIGN_KEY_PREFIX;
        public bool IsOneOrNone => IsPrimaryKey && IsForeignKey && Table.PrimaryKeysCount == 1;

        public static ForeignKey Parse(Column column)
        {
            return new ForeignKey()
            {
                Index = column.Index,
                Name = column.Name,
                Description = column.Description,
                Type = column.Type,
                Table = column.Table,
                IsRequired = column.IsRequired,
                Id = column.Id,
            };
        }
    }
}