using Markel.Application.Claims;
using Markel.Application.Exceptions;
using Markel.Application.Mapping;
using Markel.Domain.Abstractions;
using MediatR;

namespace Markel.Application.Claims.GetClaimByUcr;

public sealed class GetClaimByUcrQueryHandler(
    IClaimRepository claimRepository,
    IClock clock) : IRequestHandler<GetClaimByUcrQuery, ClaimDto>
{
    public async Task<ClaimDto> Handle(GetClaimByUcrQuery request, CancellationToken cancellationToken)
    {
        var claim = await claimRepository.GetByUcrAsync(request.Ucr, cancellationToken)
                    ?? throw new NotFoundException(nameof(Domain.Entities.Claim), request.Ucr);

        return claim.ToDto(clock);
    }
}
