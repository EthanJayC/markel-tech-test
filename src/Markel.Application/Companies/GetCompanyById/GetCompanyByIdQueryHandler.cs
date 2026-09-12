using Markel.Application.Companies;
using Markel.Application.Exceptions;
using Markel.Application.Mapping;
using Markel.Domain.Abstractions;
using MediatR;

namespace Markel.Application.Companies.GetCompanyById;

public sealed class GetCompanyByIdQueryHandler(
    ICompanyRepository companyRepository,
    IClock clock) : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
{
    public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetByIdAsync(request.Id, cancellationToken)
                      ?? throw new NotFoundException(nameof(Domain.Entities.Company), request.Id);

        return company.ToDto(clock);
    }
}
