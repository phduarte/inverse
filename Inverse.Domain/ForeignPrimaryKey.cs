namespace Inverse.Domain
{
    public class ForeignPrimaryKey : ForeignKey, IForeignPrimaryKey
    {
        public override bool IsPrimaryKey => true;
        public override bool IsForeignKey => true;
        public override string Prefix => FOREIGN_PRIMART_KEY_PREFIX;

        public new static ForeignPrimaryKey Parse(Column foreignKey)
        {
            var fk = foreignKey as ForeignKey;

            return new ForeignPrimaryKey
            {
                Id = foreignKey.Id,
                Index = foreignKey.Index,
                Name = foreignKey.Name,
                IsRequired = foreignKey.IsRequired,
                Table = foreignKey.Table,
                Type = foreignKey.Type,
                DefaultValue = foreignKey.DefaultValue,
                Description = foreignKey.Description,
                RelatedColumn = fk?.RelatedColumn,
                RelatedTable = fk?.RelatedTable
            };
        }
    }
}