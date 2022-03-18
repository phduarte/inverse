namespace Inverse.Domain.Databases
{
    public interface IDatabaseGeneratorStrategy
    {
        Provider Provider { get; }
        Database LoadDatabase(string connectionString);
    }
}
