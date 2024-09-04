using FsCron.TestApp.Database;
using Mediator;

namespace FsCron.TestApp.Services.Handlers;

public sealed class PetInsertCommandHandler(PetStoreContext dbContext)
    : IRequestHandler<PetInsertCommand>
{
    public ValueTask<Unit> Handle(
        PetInsertCommand request,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Unit>(cancellationToken);
        }
        throw new NotImplementedException();
    }
}