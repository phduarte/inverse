namespace Inverse.Domain;

public interface IDatabaseService
{
    Database LoadDatabase(Provider provider, string connectionString);

    void Export(Database database, string fileName);

    Database OpenFile(string fileName);

    void SaveFile(Database database, string fileName);

    void Install(IDatabaseGeneratorStrategy databaseGeneratorStrategy);

    void Install(IScriptingGeneratorStrategy scriptingGeneratorStrategy);

    void Install(IFileManagerStrategy fileManagerStrategy);

    string[] GetCompatiblesFileTypes();

    string[] GetCompatiblesScriptings();

    DatabaseService With<T>(T strategy);
}