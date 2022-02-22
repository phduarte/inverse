using System.Collections.Generic;
using System.Linq;
using Inverse.Domain.Model;

namespace Inverse.Domain.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IDictionary<Provider, IDatabaseGeneratorStrategy> _databaseGenetorStrategies = new Dictionary<Provider, IDatabaseGeneratorStrategy>();
        private readonly IDictionary<string, IScriptingGeneratorStrategy> _scriptingGeneratorStrategies = new Dictionary<string, IScriptingGeneratorStrategy>();
        private readonly IDictionary<string, IFileManagerStrategy> _fileManagerStrategies = new Dictionary<string, IFileManagerStrategy>();

        private readonly IDatabaseGeneratorStrategy _databaseGeneratorStrategy;
        private readonly IScriptingGeneratorStrategy _scriptingGeneratorStrategy;
        private readonly IFileManagerStrategy _fileManagerStrategy;

        public DatabaseService(
            IDatabaseGeneratorStrategy databaseGenerator = null,
            IScriptingGeneratorStrategy scriptingGenerator = null,
            IFileManagerStrategy fileManagerStrategy = null)
        {
            _databaseGeneratorStrategy = databaseGenerator;
            _scriptingGeneratorStrategy = scriptingGenerator;
            _fileManagerStrategy = fileManagerStrategy;
        }

        public void Install(IDatabaseGeneratorStrategy databaseGeneratorStrategy)
        {
            if (!_databaseGenetorStrategies.ContainsKey(databaseGeneratorStrategy.Provider))
            {
                _databaseGenetorStrategies.Add(databaseGeneratorStrategy.Provider, databaseGeneratorStrategy);
            }
        }

        public void Install(IScriptingGeneratorStrategy scriptingGeneratorStrategy)
        {
            if (!_scriptingGeneratorStrategies.ContainsKey(scriptingGeneratorStrategy.Name))
            {
                _scriptingGeneratorStrategies.Add(scriptingGeneratorStrategy.Name, scriptingGeneratorStrategy);
            }
        }

        public void Install(IFileManagerStrategy fileManagerStrategy)
        {
            if (!_fileManagerStrategies.ContainsKey(fileManagerStrategy.Extension))
            {
                _fileManagerStrategies.Add(fileManagerStrategy.Extension, fileManagerStrategy);
            }
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

        public void Export(Database database, string strategyName, string fileName)
        {
            var st = _scriptingGeneratorStrategies.Values.FirstOrDefault(r => $"{r.Name}|*{r.Extension}".Equals(strategyName)).Name;
            GetScriptingGeneratorStrategy(st).ExportToFile(database, fileName);
        }

        private IDatabaseGeneratorStrategy CreateGeneratorStrategy(Provider provider)
        {
            return _databaseGeneratorStrategy ?? _databaseGenetorStrategies[provider];
        }

        private IScriptingGeneratorStrategy GetScriptingGeneratorStrategy(string strategyName)
        {
            return _scriptingGeneratorStrategy ?? _scriptingGeneratorStrategies[strategyName];
        }

        public string[] GetCompatiblesFileTypes()
        {
            return _fileManagerStrategies.Values.Select(r => $"{r.Description}|*{r.Extension}").ToArray();
        }

        private IFileManagerStrategy GetStrategyByFile(string filename)
        {
            var fi = new System.IO.FileInfo(filename);
            var ext = fi.Extension;
            return _fileManagerStrategies.FirstOrDefault(x => x.Value.Extension == ext).Value;
        }

        public string[] GetCompatiblesScriptings()
        {
            return _scriptingGeneratorStrategies.Values.Select(r => $"{r.Name}|*{r.Extension}").ToArray();
        }
    }
}
