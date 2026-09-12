using Markel.Domain.Entities;

namespace Markel.Domain.Abstractions;

public interface ICompanyRepository
{
    Task<IReadOnlyList<Company>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Company?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
