using FsCron.TestApp.Database;
using FsCron.TestApp.Services.Model;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FsCron.TestApp.Services.Handlers;

public sealed class PetSelectCommandHandler(PetStoreContext dbContext)
    : IRequestHandler<PetSelectCommand, List<PetInfo>>
{
    public async ValueTask<List<PetInfo>> Handle(
        PetSelectCommand request,
        CancellationToken cancellationToken)
    {
        var pets = await dbContext
            .Pets
            .Select(i => new PetInfo(
                i.Id,
                i.Name,
                i.Added,
                i.ConcurrencyStamp
            ))
            .ToListAsync(cancellationToken);
        return pets;
    }
}