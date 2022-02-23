using Inverse.Domain.Models;

namespace Inverse.Domain.Services
{
    public interface IScriptingGeneratorStrategy
    {
        string Name { get; }
        string Extension { get; }
        void ExportToFile(Database database, string filename);
    }
}
