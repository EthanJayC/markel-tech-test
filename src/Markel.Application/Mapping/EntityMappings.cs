using Markel.Application.Claims;
using Markel.Application.Companies;
using Markel.Domain.Abstractions;
using Markel.Domain.Entities;

namespace Markel.Application.Mapping;

public static class EntityMappings
{
    public static CompanyDto ToDto(this Company company, IClock clock) =>
        new(
            company.Id,
            company.Name,
            company.Address1,
            company.Address2,
            company.Address3,
            company.Postcode,
            company.Country,
            company.Active,
            company.InsuranceEndDate,
            HasActiveInsurancePolicy(company, clock));

    public static ClaimDto ToDto(this Claim claim, IClock clock) =>
        new(
            claim.Ucr,
            claim.CompanyId,
            claim.ClaimDate,
            claim.LossDate,
            claim.AssuredName,
            claim.IncurredLoss,
            claim.Closed,
            AgeInDays(claim, clock));

    public static bool HasActiveInsurancePolicy(Company company, IClock clock) =>
        company.Active && company.InsuranceEndDate.Date >= clock.UtcNow.Date;

    public static int AgeInDays(Claim claim, IClock clock) =>
        (clock.UtcNow.Date - claim.ClaimDate.Date).Days;
}
