using EngenhariaReversaDb.Domain.Model;

namespace EngenhariaReversaDb.Domain.Services
{
    public interface IScriptingGeneratorStrategy
    {
        void GenerateFile(Database database, string filename);
    }
}
