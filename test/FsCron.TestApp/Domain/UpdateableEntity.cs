namespace FsCron.TestApp.Domain;

public abstract class UpdateableEntity : Entity
{
    public DateTimeOffset Updated { get; set; }

    public Guid ConcurrencyStamp { get; set; }
}