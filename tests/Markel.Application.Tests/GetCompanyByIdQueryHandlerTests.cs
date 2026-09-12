using FluentAssertions;
using Markel.Application.Companies.GetCompanyById;
using Markel.Application.Exceptions;
using Markel.Application.Tests.Fakes;
using Markel.Domain.Entities;

namespace Markel.Application.Tests;

public sealed class GetCompanyByIdQueryHandlerTests
{
    private static readonly DateTime FrozenUtc = new(2026, 9, 12, 0, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task Returns_active_policy_when_company_is_active_and_cover_has_not_ended()
    {
        var companies = new FakeCompanyRepository();
        companies.Add(new Company
        {
            Id = 1,
            Name = "Active Co",
            Active = true,
            InsuranceEndDate = FrozenUtc.AddDays(30)
        });

        var handler = new GetCompanyByIdQueryHandler(companies, new FakeClock(FrozenUtc));

        var result = await handler.Handle(new GetCompanyByIdQuery(1), CancellationToken.None);

        result.HasActiveInsurancePolicy.Should().BeTrue();
    }

    [Fact]
    public async Task Returns_inactive_policy_when_company_is_inactive_or_cover_has_ended()
    {
        var companies = new FakeCompanyRepository();
        companies.Add(new Company
        {
            Id = 1,
            Name = "Expired Co",
            Active = true,
            InsuranceEndDate = FrozenUtc.AddDays(-1)
        });
        companies.Add(new Company
        {
            Id = 2,
            Name = "Inactive Co",
            Active = false,
            InsuranceEndDate = FrozenUtc.AddYears(1)
        });

        var handler = new GetCompanyByIdQueryHandler(companies, new FakeClock(FrozenUtc));

        var expired = await handler.Handle(new GetCompanyByIdQuery(1), CancellationToken.None);
        var inactive = await handler.Handle(new GetCompanyByIdQuery(2), CancellationToken.None);

        expired.HasActiveInsurancePolicy.Should().BeFalse();
        inactive.HasActiveInsurancePolicy.Should().BeFalse();
    }

    [Fact]
    public async Task Throws_not_found_when_company_is_missing()
    {
        var handler = new GetCompanyByIdQueryHandler(new FakeCompanyRepository(), new FakeClock(FrozenUtc));

        var act = async () => await handler.Handle(new GetCompanyByIdQuery(99), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*99*");
    }
}
