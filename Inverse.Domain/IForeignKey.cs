namespace Inverse.Domain;

public interface IForeignKey : IColumn
{
    string RelatedColumn { get; set; }
    string RelatedTable { get; set; }
    bool IsOneOrNone { get; }
}