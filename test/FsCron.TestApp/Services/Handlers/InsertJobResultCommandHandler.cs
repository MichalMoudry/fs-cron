using FsCron.TestApp.Database;
using FsCron.TestApp.Domain;
using Mediator;
using Microsoft.Extensions.Logging;

namespace FsCron.TestApp.Services.Handlers;

public sealed class InsertJobResultCommandHandler(
    TestDbContext dbContext,
    ILogger<InsertJobResultCommandHandler> logger)
    : IRequestHandler<InsertJobResultCommand>
{
    public async ValueTask<Unit> Handle(
        InsertJobResultCommand request,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Unit.Value;
        }

        var jobResult = new JobResult
        {
            Type = request.Type,
            StartTime = request.Start,
            FinishTime = request.End
        };
        dbContext.JobResults.Add(jobResult);
        logger.LogInformation("Inserted {JobResult}", jobResult);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}