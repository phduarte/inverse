using System.Collections.Generic;
using System.Linq;

namespace Inverse.Domain;

public class DatabaseService : IDatabaseService
{
    private readonly IDictionary<Provider, IDatabaseGeneratorStrategy> _databaseGenetorStrategies = new Dictionary<Provider, IDatabaseGeneratorStrategy>();
    private readonly IDictionary<string, IScriptingGeneratorStrategy> _scriptingGeneratorStrategies = new Dictionary<string, IScriptingGeneratorStrategy>();
    private readonly IDictionary<string, IFileManagerStrategy> _fileManagerStrategies = new Dictionary<string, IFileManagerStrategy>();

    public void Install(IDatabaseGeneratorStrategy databaseGeneratorStrategy)
    {
        if (!_databaseGenetorStrategies.ContainsKey(databaseGeneratorStrategy.Provider))
        {
            _databaseGenetorStrategies.Add(databaseGeneratorStrategy.Provider, databaseGeneratorStrategy);
        }
    }

    public void Install(IScriptingGeneratorStrategy scriptingGeneratorStrategy)
    {
        if (!_scriptingGeneratorStrategies.ContainsKey(scriptingGeneratorStrategy.Extension))
        {
            _scriptingGeneratorStrategies.Add(scriptingGeneratorStrategy.Extension, scriptingGeneratorStrategy);
        }
    }

    public void Install(IFileManagerStrategy fileManagerStrategy)
    {
        if (!_fileManagerStrategies.ContainsKey(fileManagerStrategy.Extension))
        {
            _fileManagerStrategies.Add(fileManagerStrategy.Extension, fileManagerStrategy);
        }
    }

    public DatabaseService With<T>(T strategy)
    {
        if (strategy is IScriptingGeneratorStrategy s)
        {
            Install(s);
        }
        else if (strategy is IFileManagerStrategy f)
        {
            Install(f);
        }
        else if (strategy is IDatabaseGeneratorStrategy d)
        {
            Install(d);
        }

        return this;
    }

    public void SaveFile(Database database, string fileName)
    {
        var strategy = GetStrategyByFile(fileName);
        strategy.SaveFile(database, fileName);
    }

    public Database OpenFile(string fileName)
    {
        var strategy = GetStrategyByFile(fileName);
        return strategy.OpenFile(fileName);
    }

    public Database LoadDatabase(Provider provider, string connectionString)
    {
        return CreateGeneratorStrategy(provider).LoadDatabase(connectionString);
    }

    public void Export(Database database, string fileName, int selectedIndex)
    {
        GetScriptingGeneratorStrategy(selectedIndex).ExportToFile(database, fileName);
    }

    public string[] GetCompatiblesFileTypes()
    {
        return _fileManagerStrategies.Values.Select(r => $"{r.Description}|*{r.Extension}").ToArray();
    }

    public string[] GetCompatiblesScriptings()
    {
        return _scriptingGeneratorStrategies.Values.Select(r => $"{r.Name}|*{r.Extension}").ToArray();
    }

    private IDatabaseGeneratorStrategy CreateGeneratorStrategy(Provider provider)
    {
        return _databaseGenetorStrategies[provider];
    }

    private IScriptingGeneratorStrategy GetScriptingGeneratorStrategy(int index)
    {
        return _scriptingGeneratorStrategies.ElementAt(index).Value;
    }

    private IFileManagerStrategy GetStrategyByFile(string filename)
    {
        var ext = GetExtension(filename);
        return _fileManagerStrategies.FirstOrDefault(x => x.Value.Extension == ext).Value;
    }

    private string GetExtension(string filename)
    {
        var fi = new System.IO.FileInfo(filename);
        return fi.Extension;
    }
}