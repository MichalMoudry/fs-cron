using System.ComponentModel.DataAnnotations;

namespace FsCron.TestApp.Domain;

public class Pet : UpdateableEntity
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
}