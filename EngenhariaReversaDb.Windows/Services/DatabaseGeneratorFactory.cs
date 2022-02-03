using System;
using EngenhariaReversaDb.Domain;

namespace EngenhariaReversaDb.Services
{
    public static class DatabaseGeneratorFactory
    {
        public static IDatabaseGeneratorService Create(Provider provider)
        {
            if (provider == Provider.SQLite)
            {
                return new SqliteDatabaseGeneratorService(provider);
            }
            else if (provider == Provider.MSSQLServer)
            {
                return new SqlServerDatabaseGeneratorService(provider);
            }

            throw new NotImplementedException();
        }
    }
}
