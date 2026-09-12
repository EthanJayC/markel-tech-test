namespace Markel.Domain.Entities;

public sealed class Claim
{
    public string Ucr { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public DateTime ClaimDate { get; set; }
    public DateTime LossDate { get; set; }
    public string AssuredName { get; set; } = string.Empty;
    public decimal IncurredLoss { get; set; }
    public bool Closed { get; set; }
}
