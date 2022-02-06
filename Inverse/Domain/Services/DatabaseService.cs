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

        public DatabaseService()
        {
            _databaseGeneratorStrategy = null;
        }

        public DatabaseService(
            IDatabaseGeneratorStrategy databaseGenerator = null,
            IScriptingGeneratorStrategy scriptingGenerator = null)
        {
            _databaseGeneratorStrategy = databaseGenerator;
            _scriptingGeneratorStrategy = scriptingGenerator;
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
