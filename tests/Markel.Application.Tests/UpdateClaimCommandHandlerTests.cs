using FluentAssertions;
using FluentValidation;
using Markel.Application.Claims.UpdateClaim;
using Markel.Application.Exceptions;
using Markel.Application.Tests.Fakes;
using Markel.Domain.Entities;

namespace Markel.Application.Tests;

public sealed class UpdateClaimCommandHandlerTests
{
    private static readonly DateTime FrozenUtc = new(2026, 9, 12, 0, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task Updates_mutable_fields_and_leaves_ucr_and_company_id()
    {
        var claims = new FakeClaimRepository();
        claims.Add(new Claim
        {
            Ucr = "UCR-1",
            CompanyId = 7,
            ClaimDate = FrozenUtc.AddDays(-20),
            LossDate = FrozenUtc.AddDays(-21),
            AssuredName = "Old Name",
            IncurredLoss = 10m,
            Closed = false
        });

        var handler = new UpdateClaimCommandHandler(claims, new FakeClock(FrozenUtc));
        var command = new UpdateClaimCommand(
            "UCR-1",
            FrozenUtc.AddDays(-5),
            FrozenUtc.AddDays(-6),
            "New Name",
            250.25m,
            true);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Ucr.Should().Be("UCR-1");
        result.CompanyId.Should().Be(7);
        result.AssuredName.Should().Be("New Name");
        result.IncurredLoss.Should().Be(250.25m);
        result.Closed.Should().BeTrue();
        result.AgeInDays.Should().Be(5);

        var stored = await claims.GetByUcrAsync("UCR-1");
        stored!.AssuredName.Should().Be("New Name");
        stored.CompanyId.Should().Be(7);
    }

    [Fact]
    public async Task Throws_not_found_when_claim_is_missing()
    {
        var handler = new UpdateClaimCommandHandler(new FakeClaimRepository(), new FakeClock(FrozenUtc));
        var command = new UpdateClaimCommand("missing", FrozenUtc, FrozenUtc, "Name", 1m, false);

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Validator_rejects_negative_loss_and_loss_after_claim_date()
    {
        var validator = new UpdateClaimCommandValidator(new FakeClock(FrozenUtc));

        var negative = await validator.ValidateAsync(new UpdateClaimCommand(
            "UCR-1", FrozenUtc, FrozenUtc, "Name", -1m, false));
        var dates = await validator.ValidateAsync(new UpdateClaimCommand(
            "UCR-1", FrozenUtc.AddDays(-2), FrozenUtc, "Name", 1m, false));

        negative.IsValid.Should().BeFalse();
        dates.IsValid.Should().BeFalse();
        dates.Errors.Should().Contain(error => error.PropertyName == nameof(UpdateClaimCommand.LossDate));
    }
}
