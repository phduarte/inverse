namespace Inverse.Domain;

public interface IScriptingGeneratorStrategy
{
    string Name { get; }
    string Extension { get; }

    void ExportToFile(Database database, string filename);
}