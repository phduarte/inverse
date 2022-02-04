using Inverse.Domain.Model;

namespace Inverse.Domain.Services
{
    public interface IDatabaseGeneratorStrategy
    {
        Provider Provider { get; }
        Database LoadDatabase(string connectionString);
    }
}
