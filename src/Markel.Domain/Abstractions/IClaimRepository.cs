using Markel.Domain.Entities;

namespace Markel.Domain.Abstractions;

public interface IClaimRepository
{
    Task<IReadOnlyList<Claim>> GetByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
    Task<Claim?> GetByUcrAsync(string ucr, CancellationToken cancellationToken = default);
    Task UpdateAsync(Claim claim, CancellationToken cancellationToken = default);
}
