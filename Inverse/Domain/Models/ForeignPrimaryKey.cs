namespace Inverse.Domain.Models
{
    public class ForeignPrimaryKey : ForeignKey
    {
        public override bool IsPrimaryKey => true;
        public override bool IsForeignKey => true;
        public override string Prefix => LayoutDefinition.Columns.FOREIGN_PRIMART_KEY_PREFIX;

        public static ForeignPrimaryKey Parse(ForeignKey foreignKey)
        {
            return new ForeignPrimaryKey
            {
                Id = foreignKey.Id,
                Index = foreignKey.Index,
                Name = foreignKey.Name,
                RelatedColumn = foreignKey.RelatedColumn,
                RelatedTable = foreignKey.RelatedTable,
                Required = foreignKey.Required,
                Table = foreignKey.Table,
                Type = foreignKey.Type
            };
        }
    }
}
