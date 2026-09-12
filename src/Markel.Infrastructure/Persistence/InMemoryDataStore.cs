using Markel.Domain.Entities;

namespace Markel.Infrastructure.Persistence;

public sealed class InMemoryDataStore
{
    private readonly object _gate = new();
    private readonly Dictionary<int, Company> _companies = new();
    private readonly Dictionary<string, Claim> _claims = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<int, ClaimType> _claimTypes = new();

    public InMemoryDataStore()
    {
        Seed();
    }

    public IReadOnlyList<Company> GetCompanies()
    {
        lock (_gate)
        {
            return _companies.Values.Select(Clone).OrderBy(c => c.Id).ToList();
        }
    }

    public Company? GetCompany(int id)
    {
        lock (_gate)
        {
            return _companies.TryGetValue(id, out var company) ? Clone(company) : null;
        }
    }

    public IReadOnlyList<Claim> GetClaimsByCompanyId(int companyId)
    {
        lock (_gate)
        {
            return _claims.Values
                .Where(claim => claim.CompanyId == companyId)
                .Select(Clone)
                .OrderBy(claim => claim.ClaimDate)
                .ToList();
        }
    }

    public Claim? GetClaim(string ucr)
    {
        lock (_gate)
        {
            return _claims.TryGetValue(ucr, out var claim) ? Clone(claim) : null;
        }
    }

    public void UpdateClaim(Claim claim)
    {
        lock (_gate)
        {
            if (!_claims.ContainsKey(claim.Ucr))
            {
                throw new InvalidOperationException($"Claim '{claim.Ucr}' was not found.");
            }

            _claims[claim.Ucr] = Clone(claim);
        }
    }

    public IReadOnlyList<ClaimType> GetClaimTypes()
    {
        lock (_gate)
        {
            return _claimTypes.Values.Select(type => new ClaimType { Id = type.Id, Name = type.Name }).ToList();
        }
    }

    private void Seed()
    {
        _claimTypes[1] = new ClaimType { Id = 1, Name = "Motor" };
        _claimTypes[2] = new ClaimType { Id = 2, Name = "Property" };
        _claimTypes[3] = new ClaimType { Id = 3, Name = "Liability" };

        _companies[1] = new Company
        {
            Id = 1,
            Name = "Markel",
            Address1 = "Leeds street",
            Address2 = "",
            Address3 = "Leeds",
            Postcode = "LD1 1AA",
            Country = "United Kingdom",
            Active = true,
            InsuranceEndDate = new DateTime(2027, 12, 31, 0, 0, 0, DateTimeKind.Utc)
        };

        _companies[2] = new Company
        {
            Id = 2,
            Name = "Ethans awesome company",
            Address1 = "5 gileswood crescent",
            Address2 = null,
            Address3 = "Rotherham",
            Postcode = "s63 6bu",
            Country = "United Kingdom",
            Active = true,
            InsuranceEndDate = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc)
        };

        _companies[3] = new Company
        {
            Id = 3,
            Name = "ACME inc",
            Address1 = "Unit 4",
            Address2 = "Industrial Estate",
            Address3 = "Sheffield",
            Postcode = "S2 3RB",
            Country = "United Kingdom",
            Active = false,
            InsuranceEndDate = new DateTime(2028, 6, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        _claims["UCR-1001"] = new Claim
        {
            Ucr = "UCR-1001",
            CompanyId = 1,
            ClaimDate = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc),
            LossDate = new DateTime(2026, 3, 8, 0, 0, 0, DateTimeKind.Utc),
            AssuredName = "Markel HQ",
            IncurredLoss = 12500.50m,
            Closed = false
        };

        _claims["UCR-1002"] = new Claim
        {
            Ucr = "UCR-1002",
            CompanyId = 1,
            ClaimDate = new DateTime(2025, 11, 2, 0, 0, 0, DateTimeKind.Utc),
            LossDate = new DateTime(2025, 10, 28, 0, 0, 0, DateTimeKind.Utc),
            AssuredName = "Markel 2nd office",
            IncurredLoss = 4800.00m,
            Closed = true
        };

        _claims["UCR-2001"] = new Claim
        {
            Ucr = "UCR-2001",
            CompanyId = 2,
            ClaimDate = new DateTime(2024, 6, 20, 0, 0, 0, DateTimeKind.Utc),
            LossDate = new DateTime(2024, 6, 19, 0, 0, 0, DateTimeKind.Utc),
            AssuredName = "Ethans claim",
            IncurredLoss = 95000.00m,
            Closed = false
        };
    }

    private static Company Clone(Company company) => new()
    {
        Id = company.Id,
        Name = company.Name,
        Address1 = company.Address1,
        Address2 = company.Address2,
        Address3 = company.Address3,
        Postcode = company.Postcode,
        Country = company.Country,
        Active = company.Active,
        InsuranceEndDate = company.InsuranceEndDate
    };

    private static Claim Clone(Claim claim) => new()
    {
        Ucr = claim.Ucr,
        CompanyId = claim.CompanyId,
        ClaimDate = claim.ClaimDate,
        LossDate = claim.LossDate,
        AssuredName = claim.AssuredName,
        IncurredLoss = claim.IncurredLoss,
        Closed = claim.Closed
    };
}
