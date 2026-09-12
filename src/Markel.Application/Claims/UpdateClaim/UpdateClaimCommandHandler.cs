using Markel.Application.Claims;
using Markel.Application.Exceptions;
using Markel.Application.Mapping;
using Markel.Domain.Abstractions;
using MediatR;

namespace Markel.Application.Claims.UpdateClaim;

public sealed class UpdateClaimCommandHandler(
    IClaimRepository claimRepository,
    IClock clock) : IRequestHandler<UpdateClaimCommand, ClaimDto>
{
    public async Task<ClaimDto> Handle(UpdateClaimCommand request, CancellationToken cancellationToken)
    {
        var claim = await claimRepository.GetByUcrAsync(request.Ucr, cancellationToken)
                    ?? throw new NotFoundException(nameof(Domain.Entities.Claim), request.Ucr);

        claim.ClaimDate = request.ClaimDate;
        claim.LossDate = request.LossDate;
        claim.AssuredName = request.AssuredName;
        claim.IncurredLoss = request.IncurredLoss;
        claim.Closed = request.Closed;

        await claimRepository.UpdateAsync(claim, cancellationToken);
        return claim.ToDto(clock);
    }
}
