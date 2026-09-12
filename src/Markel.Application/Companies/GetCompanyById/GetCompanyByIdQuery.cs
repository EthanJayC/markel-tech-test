using Markel.Application.Companies;
using MediatR;

namespace Markel.Application.Companies.GetCompanyById;

public sealed record GetCompanyByIdQuery(int Id) : IRequest<CompanyDto>;
