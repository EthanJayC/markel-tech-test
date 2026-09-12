using Markel.Application.Mapping;
using Markel.Domain.Abstractions;
using MediatR;

namespace Markel.Application.Companies.GetCompanies;

public sealed class GetCompaniesQueryHandler(
    ICompanyRepository companyRepository,
    IClock clock) : IRequestHandler<GetCompaniesQuery, IReadOnlyList<CompanyDto>>
{
    public async Task<IReadOnlyList<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await companyRepository.GetAllAsync(cancellationToken);
        return companies.Select(company => company.ToDto(clock)).ToList();
    }
}
