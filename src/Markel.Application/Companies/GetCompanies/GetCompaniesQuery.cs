using Markel.Application.Companies;
using MediatR;

namespace Markel.Application.Companies.GetCompanies;

public sealed record GetCompaniesQuery : IRequest<IReadOnlyList<CompanyDto>>;
