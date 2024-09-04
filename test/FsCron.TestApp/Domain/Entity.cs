namespace FsCron.TestApp.Domain;

public abstract class Entity
{
    public Guid Id { get; init; }

    public DateTimeOffset Added { get; init; }
}