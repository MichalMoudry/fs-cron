namespace FsCron.TestApp.Domain;

internal abstract class Entity
{
    public Guid Id { get; init; }

    public DateTimeOffset Added { get; init; }
}