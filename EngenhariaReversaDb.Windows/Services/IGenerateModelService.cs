using EngenhariaReversaDb.Domain;

namespace EngenhariaReversaDb.Services
{
    public interface IGenerateModelService
    {
        Provider Provider { get; }
        Database GetDatabase(string connectionString);
    }
}
