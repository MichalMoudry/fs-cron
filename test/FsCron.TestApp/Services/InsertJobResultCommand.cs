using FsCron.TestApp.Domain;
using Mediator;

namespace FsCron.TestApp.Services;

public sealed record InsertJobResultCommand(
    JobType Type,
    DateTimeOffset Start,
    DateTimeOffset End
) : IRequest;