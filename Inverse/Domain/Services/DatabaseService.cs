using System.Collections.Generic;
using Inverse.Domain.Model;

namespace Inverse.Domain.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IDictionary<Provider, IDatabaseGeneratorStrategy> _databaseGenetorStrategies = new Dictionary<Provider, IDatabaseGeneratorStrategy>()
        {
            { Provider.SQLite, SqliteDatabaseGeneratorStrategy.Create() },
            { Provider.MSSQLServer, SqlServerDatabaseGeneratorStrategy.Create() }
        };

        private readonly IDictionary<Provider, IScriptingGeneratorStrategy> _scriptingGeneratorStrategies = new Dictionary<Provider, IScriptingGeneratorStrategy>()
        {
            { Provider.SQLite, new SqlServerScriptingGeneratorStrategy()},
            { Provider.MSSQLServer, new SqlServerScriptingGeneratorStrategy()}
        };

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
            _fileManagerStrategy = fileManagerStrategy?? new EncryptedXmlFileManagerStrategy();
        }

        public void SaveFile(Database database, string fileName)
        {
            _fileManagerStrategy.SaveFile(database, fileName);
        }

        public Database OpenFile(string fileName)
        {
            return _fileManagerStrategy.OpenFile(fileName);
        }

        public Database LoadDatabase(Provider provider, string connectionString)
        {
            return CreateGeneratorStrategy(provider).LoadDatabase(connectionString);
        }

        public void Export(Database database, string fileName)
        {
            GetScriptingGeneratorStrategy(database.Provider).ExportToFile(database, fileName);
        }

        private IDatabaseGeneratorStrategy CreateGeneratorStrategy(Provider provider)
        {
            return _databaseGeneratorStrategy ?? _databaseGenetorStrategies[provider];
        }

        private IScriptingGeneratorStrategy GetScriptingGeneratorStrategy(Provider provider)
        {
            return _scriptingGeneratorStrategy ?? _scriptingGeneratorStrategies[provider];
        }
    }
}
