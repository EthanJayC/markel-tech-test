namespace Markel.Domain.Abstractions;

public interface IClock
{
    DateTime UtcNow { get; }
}
