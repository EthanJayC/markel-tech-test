using FluentValidation;
using Markel.Domain.Abstractions;

namespace Markel.Application.Claims.UpdateClaim;

public sealed class UpdateClaimCommandValidator : AbstractValidator<UpdateClaimCommand>
{
    public UpdateClaimCommandValidator(IClock clock)
    {
        RuleFor(x => x.Ucr)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.AssuredName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.IncurredLoss)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.LossDate)
            .LessThanOrEqualTo(x => x.ClaimDate)
            .WithMessage("Loss date cannot be after the claim date.");

        RuleFor(x => x.ClaimDate)
            .LessThanOrEqualTo(_ => clock.UtcNow.Date.AddYears(1))
            .WithMessage("Claim date is unreasonably far in the future.");
    }
}
