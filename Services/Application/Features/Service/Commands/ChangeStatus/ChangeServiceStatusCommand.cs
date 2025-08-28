using MediatR;

public sealed record ChangeServiceStatusCommand(
    Guid ServiceId,
    bool Status
) : IRequest<Unit>;
