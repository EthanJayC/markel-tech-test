namespace Markel.Application.Companies;

public sealed record CompanyDto(
    int Id,
    string Name,
    string? Address1,
    string? Address2,
    string? Address3,
    string? Postcode,
    string? Country,
    bool Active,
    DateTime InsuranceEndDate,
    bool HasActiveInsurancePolicy);
