using System;

namespace EngenhariaReversaDb.Services
{
    public static class DatabaseModelFactory
    {
        public static IGenerateModelService Create(Domain.Provider provider)
        {
            if (provider == Domain.Provider.SQLite)
            {
                return new GenerateModelService(provider);
            }
            else if (provider == Domain.Provider.MSSQLServer)
            {
                return new SqlServerGenerateModelService(provider);
            }

            throw new NotImplementedException();
        }
    }
}
