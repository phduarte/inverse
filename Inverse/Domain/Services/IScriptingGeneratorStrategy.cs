using Inverse.Domain.Model;

namespace Inverse.Domain.Services
{
    public interface IScriptingGeneratorStrategy
    {
        void ExportToFile(Database database, string filename);
    }
}
