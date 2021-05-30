namespace EngenhariaReversaDb.Domain
{
    public class ForeignKey : Column
    {
        public string RelatedTable { get; set; }
        public string RelatedColumn { get; set; }
    }
}
