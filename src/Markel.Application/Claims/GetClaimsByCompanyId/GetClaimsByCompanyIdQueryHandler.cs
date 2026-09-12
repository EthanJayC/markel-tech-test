using Markel.Application.Claims;
using Markel.Application.Exceptions;
using Markel.Application.Mapping;
using Markel.Domain.Abstractions;
using MediatR;

namespace Markel.Application.Claims.GetClaimsByCompanyId;

public sealed class GetClaimsByCompanyIdQueryHandler(
    ICompanyRepository companyRepository,
    IClaimRepository claimRepository,
    IClock clock) : IRequestHandler<GetClaimsByCompanyIdQuery, IReadOnlyList<ClaimDto>>
{
    public async Task<IReadOnlyList<ClaimDto>> Handle(GetClaimsByCompanyIdQuery request, CancellationToken cancellationToken)
    {
        _ = await companyRepository.GetByIdAsync(request.CompanyId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Company), request.CompanyId);

        var claims = await claimRepository.GetByCompanyIdAsync(request.CompanyId, cancellationToken);
        return claims.Select(claim => claim.ToDto(clock)).ToList();
    }
}
