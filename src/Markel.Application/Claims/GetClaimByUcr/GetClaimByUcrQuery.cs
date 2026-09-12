using Markel.Application.Claims;
using MediatR;

namespace Markel.Application.Claims.GetClaimByUcr;

public sealed record GetClaimByUcrQuery(string Ucr) : IRequest<ClaimDto>;
