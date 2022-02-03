using EngenhariaReversaDb.Domain;

namespace EngenhariaReversaDb.Services
{
    public interface IDatabaseGeneratorService
    {
        Provider Provider { get; }
        Database GetDatabase(string connectionString);
    }
}
