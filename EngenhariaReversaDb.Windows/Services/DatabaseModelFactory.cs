using System;

namespace EngenhariaReversaDb.Services
{
    public static class DatabaseModelFactory
    {
        public static IDatabaseGeneratorService Create(Domain.Provider provider)
        {
            if (provider == Domain.Provider.SQLite)
            {
                return new SqliteDatabaseGeneratorService(provider);
            }
            else if (provider == Domain.Provider.MSSQLServer)
            {
                return new SqlServerDatabaseGeneratorService(provider);
            }

            throw new NotImplementedException();
        }
    }
}
