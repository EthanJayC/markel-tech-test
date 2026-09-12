using Markel.Application.Claims;
using MediatR;

namespace Markel.Application.Claims.UpdateClaim;

public sealed record UpdateClaimCommand(
    string Ucr,
    DateTime ClaimDate,
    DateTime LossDate,
    string AssuredName,
    decimal IncurredLoss,
    bool Closed) : IRequest<ClaimDto>;
