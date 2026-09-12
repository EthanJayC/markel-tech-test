using Markel.Application.Claims;
using MediatR;

namespace Markel.Application.Claims.GetClaimsByCompanyId;

public sealed record GetClaimsByCompanyIdQuery(int CompanyId) : IRequest<IReadOnlyList<ClaimDto>>;
