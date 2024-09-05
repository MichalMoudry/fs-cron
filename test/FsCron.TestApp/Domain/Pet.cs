using System.ComponentModel.DataAnnotations;

namespace FsCron.TestApp.Domain;

public class Pet : UpdateableEntity
{
    public Pet()
    {
        Id = Guid.NewGuid();
        Added = DateTimeOffset.Now;
        Updated = DateTimeOffset.Now;
        ConcurrencyStamp = Guid.NewGuid();
    }

    [MaxLength(128)]
    public string Name { get; set; } = default!;
}