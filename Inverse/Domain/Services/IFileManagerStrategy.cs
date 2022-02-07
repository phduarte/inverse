using Inverse.Domain.Model;

namespace Inverse.Domain.Services
{
    public interface IFileManagerStrategy
    {
        void SaveFile(Database database, string fileName);
        Database OpenFile(string fileName);
    }
}
