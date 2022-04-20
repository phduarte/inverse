namespace Inverse.Domain.Columns
{
    public class ForeignKey : Column, IForeignKey
    {
        public string RelatedTable { get; set; }
        public string RelatedColumn { get; set; }
        public override bool IsForeignKey => true;
        public override string Prefix => LayoutDefinition.Columns.FOREIGN_KEY_PREFIX;
        public bool IsOneOrNone => IsPrimaryKey && IsForeignKey && Table.PrimaryKeysCount == 1;
    }
}
