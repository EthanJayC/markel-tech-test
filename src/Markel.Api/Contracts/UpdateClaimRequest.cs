using Markel.Application.Claims.UpdateClaim;

namespace Markel.Api.Contracts;

public sealed record UpdateClaimRequest(
    DateTime ClaimDate,
    DateTime LossDate,
    string AssuredName,
    decimal IncurredLoss,
    bool Closed)
{
    public UpdateClaimCommand ToCommand(string ucr) =>
        new(ucr, ClaimDate, LossDate, AssuredName, IncurredLoss, Closed);
}
