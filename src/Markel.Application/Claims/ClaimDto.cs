namespace Markel.Application.Claims;

public sealed record ClaimDto(
    string Ucr,
    int CompanyId,
    DateTime ClaimDate,
    DateTime LossDate,
    string AssuredName,
    decimal IncurredLoss,
    bool Closed,
    int AgeInDays);
