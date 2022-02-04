namespace Inverse.Domain.Model
{
    public class PrimaryKey : Column
    {
        public override bool IsPrimaryKey => true;
        public override string Prefix => "PK";
    }
}
