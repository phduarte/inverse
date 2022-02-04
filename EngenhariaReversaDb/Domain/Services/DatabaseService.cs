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
                return new SqliteDatabaseGeneratorService(provider).GetDatabase(connectionString);
            }
            else if (provider == Provider.MSSQLServer)
            {
                return new SqlServerDatabaseGeneratorService(provider).GetDatabase(connectionString);
            }

            throw new NotImplementedException();
        }

        public static void Export(Database database, string filename)
        {
            if (database.Provider == Provider.MSSQLServer)
            {
                var svc = new SqlServerScriptingGenerator();
                svc.GenerateFile(database, filename);
            }

            throw new NotImplementedException();
        }
    }
}
