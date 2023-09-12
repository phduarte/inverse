namespace Inverse.Domain
{
    public class ForeignPrimaryKey : ForeignKey, IForeignPrimaryKey
    {
        public override bool IsPrimaryKey => true;
        public override bool IsForeignKey => true;
        public override string Prefix => FOREIGN_PRIMART_KEY_PREFIX;

        public static ForeignPrimaryKey Parse(ForeignKey foreignKey)
        {
            return new ForeignPrimaryKey
            {
                Id = foreignKey.Id,
                Index = foreignKey.Index,
                Name = foreignKey.Name,
                RelatedColumn = foreignKey.RelatedColumn,
                RelatedTable = foreignKey.RelatedTable,
                IsRequired = foreignKey.IsRequired,
                Table = foreignKey.Table,
                Type = foreignKey.Type
            };
        }
    }
}
