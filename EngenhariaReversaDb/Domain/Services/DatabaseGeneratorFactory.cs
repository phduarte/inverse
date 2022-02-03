using System;
using EngenhariaReversaDb.Domain.Model;

namespace EngenhariaReversaDb.Domain.Services
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
