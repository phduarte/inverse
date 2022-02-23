using Inverse.Domain.Models;

namespace Inverse.Domain.Services
{
    public interface IDatabaseGeneratorStrategy
    {
        Provider Provider { get; }
        Database LoadDatabase(string connectionString);
    }
}
