using Inverse.Domain.Model;

namespace Inverse.Domain.Services
{
    public interface IDatabaseService
    {
        Database LoadDatabase(Provider provider, string connectionString);
        void Export(Database database, string fileName);
        Database OpenFile(string fileName);
        void SaveFile(Database database, string fileName);
    }
}
