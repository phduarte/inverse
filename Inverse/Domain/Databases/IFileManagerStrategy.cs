namespace Inverse.Domain.Databases
{
    public interface IFileManagerStrategy
    {
        string Name { get; }
        string Description { get; }
        string Extension { get; }
        void SaveFile(Database database, string fileName);
        Database OpenFile(string fileName);
    }
}
