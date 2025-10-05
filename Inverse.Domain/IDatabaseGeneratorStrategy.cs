namespace Inverse.Domain;

public interface IDatabaseGeneratorStrategy
{
    Provider Provider { get; }

    Database LoadDatabase(string connectionString);
}