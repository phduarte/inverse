using EngenhariaReversaDb.Domain.Model;

namespace EngenhariaReversaDb.Domain.Services
{
    public interface IDatabaseService
    {
        Database LoadDatabase(Provider provider, string connectionString);
        void Export(Database database, string filename);
    }
}
