using Mediator;

namespace FsCron.TestApp.Services;

public sealed record PetInsertCommand(string PetName) : IRequest;