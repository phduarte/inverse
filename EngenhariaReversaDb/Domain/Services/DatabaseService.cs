using System;
using EngenhariaReversaDb.Domain.Model;

namespace EngenhariaReversaDb.Domain.Services
{
    public static class DatabaseService
    {
        public static Database GetDatabase(Provider provider, string connectionString)
        {
            if (provider == Provider.SQLite)
            {
                return new SqliteDatabaseGeneratorStrategy(provider).GetDatabase(connectionString);
            }
            else if (provider == Provider.MSSQLServer)
            {
                return new SqlServerDatabaseGeneratorStrategy(provider).GetDatabase(connectionString);
            }

            throw new NotImplementedException();
        }

        public static void Export(Database database, string filename)
        {
            if (database.Provider == Provider.MSSQLServer)
            {
                var svc = new SqlServerScriptingGeneratorStrategy();
                svc.GenerateFile(database, filename);
            }

            throw new NotImplementedException();
        }
    }
}
