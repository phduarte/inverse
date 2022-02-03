using EngenhariaReversaDb.Domain.Model;

namespace EngenhariaReversaDb.Domain.Services
{
    public interface IDatabaseGeneratorService
    {
        Provider Provider { get; }
        Database GetDatabase(string connectionString);
    }
}
