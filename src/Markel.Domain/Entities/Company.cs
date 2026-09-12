namespace Markel.Domain.Entities;

public sealed class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Address3 { get; set; }
    public string? Postcode { get; set; }
    public string? Country { get; set; }
    public bool Active { get; set; }
    public DateTime InsuranceEndDate { get; set; }
}
