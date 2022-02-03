namespace EngenhariaReversaDb.Domain.Model
{
    public class ForeignKey : Column
    {
        public string RelatedTable { get; set; }
        public string RelatedColumn { get; set; }
        public override bool IsForeignKey => true;
        public override string Prefix => "FK";
    }
}
