using FluentAssertions;
using Markel.Application.Claims.GetClaimByUcr;
using Markel.Application.Exceptions;
using Markel.Application.Tests.Fakes;
using Markel.Domain.Entities;

namespace Markel.Application.Tests;

public sealed class GetClaimByUcrQueryHandlerTests
{
    private static readonly DateTime FrozenUtc = new(2026, 9, 12, 0, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task Age_in_days_is_counted_from_claim_date()
    {
        var claims = new FakeClaimRepository();
        claims.Add(new Claim
        {
            Ucr = "UCR-1",
            CompanyId = 1,
            ClaimDate = FrozenUtc.AddDays(-10),
            LossDate = FrozenUtc.AddDays(-12),
            AssuredName = "Warehouse",
            IncurredLoss = 100m,
            Closed = false
        });

        var handler = new GetClaimByUcrQueryHandler(claims, new FakeClock(FrozenUtc));

        var result = await handler.Handle(new GetClaimByUcrQuery("UCR-1"), CancellationToken.None);

        result.AgeInDays.Should().Be(10);
    }

    [Fact]
    public async Task Throws_not_found_when_claim_is_missing()
    {
        var handler = new GetClaimByUcrQueryHandler(new FakeClaimRepository(), new FakeClock(FrozenUtc));

        var act = async () => await handler.Handle(new GetClaimByUcrQuery("missing"), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*missing*");
    }
}
