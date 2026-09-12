using Markel.Domain.Abstractions;
using Markel.Domain.Entities;

namespace Markel.Application.Tests.Fakes;

public sealed class FakeClock(DateTime utcNow) : IClock
{
    public DateTime UtcNow { get; } = utcNow;
}

public sealed class FakeCompanyRepository : ICompanyRepository
{
    private readonly List<Company> _companies = [];

    public void Add(Company company) => _companies.Add(company);

    public Task<IReadOnlyList<Company>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Company>>(_companies.ToList());

    public Task<Company?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_companies.FirstOrDefault(company => company.Id == id));
}

public sealed class FakeClaimRepository : IClaimRepository
{
    private readonly List<Claim> _claims = [];

    public void Add(Claim claim) => _claims.Add(claim);

    public Task<IReadOnlyList<Claim>> GetByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Claim>>(_claims.Where(claim => claim.CompanyId == companyId).ToList());

    public Task<Claim?> GetByUcrAsync(string ucr, CancellationToken cancellationToken = default) =>
        Task.FromResult(_claims.FirstOrDefault(claim =>
            string.Equals(claim.Ucr, ucr, StringComparison.OrdinalIgnoreCase)));

    public Task UpdateAsync(Claim claim, CancellationToken cancellationToken = default)
    {
        var index = _claims.FindIndex(existing =>
            string.Equals(existing.Ucr, claim.Ucr, StringComparison.OrdinalIgnoreCase));
        if (index >= 0)
        {
            _claims[index] = claim;
        }

        return Task.CompletedTask;
    }
}
