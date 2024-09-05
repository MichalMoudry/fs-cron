using FsCron.TestApp.Services.Model;
using Mediator;

namespace FsCron.TestApp.Services;

public sealed record PetSelectCommand(int Limit = 100)
    : IRequest<List<PetInfo>>;