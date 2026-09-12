using Markel.Domain.Abstractions;
using Markel.Domain.Entities;

namespace Markel.Infrastructure.Persistence;

public sealed class InMemoryClaimRepository(InMemoryDataStore store) : IClaimRepository
{
    public Task<IReadOnlyList<Claim>> GetByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default) =>
        Task.FromResult(store.GetClaimsByCompanyId(companyId));

    public Task<Claim?> GetByUcrAsync(string ucr, CancellationToken cancellationToken = default) =>
        Task.FromResult(store.GetClaim(ucr));

    public Task UpdateAsync(Claim claim, CancellationToken cancellationToken = default)
    {
        store.UpdateClaim(claim);
        return Task.CompletedTask;
    }
}
