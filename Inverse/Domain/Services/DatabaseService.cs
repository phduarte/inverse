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
                return SqliteDatabaseGeneratorStrategy.Create().LoadDatabase(connectionString);
            }
            else if (provider == Provider.MSSQLServer)
            {
                return SqlServerDatabaseGeneratorStrategy.Create().LoadDatabase(connectionString);
            }

            throw new NotImplementedException();
        }

        public void Export(Database database, string filename)
        {
            var svc = new SqlServerScriptingGeneratorStrategy();
            svc.ExportToFile(database, filename);
        }
    }
}
