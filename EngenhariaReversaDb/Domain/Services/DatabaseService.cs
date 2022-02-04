using System;
using Inverse.Domain.Model;

namespace Inverse.Domain.Services
{
    public class DatabaseService : IDatabaseService
    {
        public Database LoadDatabase(Provider provider, string connectionString)
        {
            if (provider == Provider.SQLite)
            {
                return new SqliteDatabaseGeneratorStrategy(provider).LoadDatabase(connectionString);
            }
            else if (provider == Provider.MSSQLServer)
            {
                return new SqlServerDatabaseGeneratorStrategy(provider).LoadDatabase(connectionString);
            }

            throw new NotImplementedException();
        }

        public void Export(Database database, string filename)
        {
            if (database.Provider == Provider.MSSQLServer)
            {
                var svc = new SqlServerScriptingGeneratorStrategy();
                svc.ExportToFile(database, filename);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
