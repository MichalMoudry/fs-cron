namespace FsCron.TestApp.Services.Model;

public sealed record PetInfo(
    Guid Id,
    string Name,
    DateTimeOffset Added,
    Guid ConcurrencyStamp
);