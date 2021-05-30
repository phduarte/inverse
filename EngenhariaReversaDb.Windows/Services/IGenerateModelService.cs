using EngenhariaReversaDb.Domain;

namespace EngenhariaReversaDb.Services
{
    public interface IGenerateModelService
    {
        Database GetDatabase(string connectionString);
    }
}
