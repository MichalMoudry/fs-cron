namespace FsCron.TestApp.Domain;

internal abstract class UpdateableEntity : Entity
{
    public DateTimeOffset Updated { get; set; }

    public Guid ConcurrencyStamp { get; set; }
}