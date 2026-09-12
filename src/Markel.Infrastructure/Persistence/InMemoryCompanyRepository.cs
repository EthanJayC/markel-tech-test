using Markel.Domain.Abstractions;
using Markel.Domain.Entities;

namespace Markel.Infrastructure.Persistence;

public sealed class InMemoryCompanyRepository(InMemoryDataStore store) : ICompanyRepository
{
    public Task<IReadOnlyList<Company>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(store.GetCompanies());

    public Task<Company?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        Task.FromResult(store.GetCompany(id));
}
