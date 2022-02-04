using EngenhariaReversaDb.Domain.Model;

namespace EngenhariaReversaDb.Domain.Services
{
    public interface IScriptingGeneratorStrategy
    {
        void ExportToFile(Database database, string filename);
    }
}
