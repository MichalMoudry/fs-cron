using System.ComponentModel.DataAnnotations;

namespace FsCron.TestApp.Domain;

internal class Pet : UpdateableEntity
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
}