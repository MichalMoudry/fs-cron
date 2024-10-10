namespace FsCron.TestApp.Domain;

public abstract class Entity
{
    public Guid Id { get; protected init; }

    public DateTimeOffset Added { get; protected init; }
}