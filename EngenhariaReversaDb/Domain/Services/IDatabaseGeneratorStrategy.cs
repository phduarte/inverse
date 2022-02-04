using EngenhariaReversaDb.Domain.Model;

namespace EngenhariaReversaDb.Domain.Services
{
    public interface IDatabaseGeneratorStrategy
    {
        Provider Provider { get; }
        Database LoadDatabase(string connectionString);
    }
}
