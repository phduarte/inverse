namespace Inverse.Domain.Columns
{
    public class PrimaryKey : Column, IPrimaryKey
    {
        public override bool IsPrimaryKey => true;
        public override string Prefix => PRIMARY_KEY_PREFIX;
    }
}
