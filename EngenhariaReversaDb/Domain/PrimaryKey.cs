namespace EngenhariaReversaDb.Domain
{
    public class PrimaryKey : Column
    {
        public override bool IsPrimaryKey => true;
        public override string Prefix => "PK";
    }
}
