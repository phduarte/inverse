using System.Collections.Generic;
using Inverse.Domain.Model;

namespace Inverse.Domain.Services
{
    public class DatabaseService : IDatabaseService
    {
        IDictionary<Provider, IDatabaseGeneratorStrategy> _databaseGenetorStrategies = new Dictionary<Provider, IDatabaseGeneratorStrategy>()
        {
            { Provider.SQLite, SqliteDatabaseGeneratorStrategy.Create() },
            { Provider.MSSQLServer, SqlServerDatabaseGeneratorStrategy.Create() }
        };

        IDictionary<Provider, IScriptingGeneratorStrategy> _scriptingGeneratorStrategies = new Dictionary<Provider, IScriptingGeneratorStrategy>()
        {
            { Provider.SQLite, new SqlServerScriptingGeneratorStrategy()},
            { Provider.MSSQLServer, new SqlServerScriptingGeneratorStrategy()}
        };

        IDatabaseGeneratorStrategy _databaseGeneratorStrategy;
        IScriptingGeneratorStrategy _scriptingGeneratorStrategy;
        IFileManagerStrategy _fileManagerStrategy;

        public DatabaseService()
        {
            _databaseGeneratorStrategy = null;
            _scriptingGeneratorStrategy = null;
            _fileManagerStrategy = new EncryptedXmlFileManagerStrategy();
        }

        public DatabaseService(
            IDatabaseGeneratorStrategy databaseGenerator = null,
            IScriptingGeneratorStrategy scriptingGenerator = null,
            IFileManagerStrategy fileManagerStrategy = null)
        {
            _databaseGeneratorStrategy = databaseGenerator;
            _scriptingGeneratorStrategy = scriptingGenerator;
            _fileManagerStrategy = fileManagerStrategy;
        }

        public void SaveFile(Database database, string fileName)
        {
            _fileManagerStrategy.SaveFile(database, fileName);
        }

        public Database OpenFile(string currentFilename)
        {
            return _fileManagerStrategy.OpenFile(currentFilename);
        }

        public Database LoadDatabase(Provider provider, string connectionString)
        {
            return CreateGeneratorStrategy(provider).LoadDatabase(connectionString);
        }

        public void Export(Database database, string filename)
        {
            GetScriptingGeneratorStrategy(database.Provider).ExportToFile(database, filename);
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
