using FsCron.TestApp.Database;
using FsCron.TestApp.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FsCron.TestApp.Services.Handlers;

public sealed class PetInsertCommandHandler(PetStoreContext dbContext)
    : IRequestHandler<PetInsertCommand>
{
    public async ValueTask<Unit> Handle(
        PetInsertCommand request,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Unit.Value;
        }

        var pet = new Pet { Name = request.PetName };
        dbContext.Pets.Add(pet);
        await dbContext.SaveChangesAsync(cancellationToken);
        Console.WriteLine($"Inserted pet named '{request.PetName}' and ID: {pet.Id}");

        var petsCount = await dbContext.Pets.CountAsync(cancellationToken);
        Console.WriteLine($"Number of pets: {petsCount}");
        return Unit.Value;
    }
}