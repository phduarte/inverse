namespace Inverse.Domain.Databases
{
    public interface IScriptingGeneratorStrategy
    {
        string Name { get; }
        string Extension { get; }
        void ExportToFile(Database database, string filename);
    }
}
