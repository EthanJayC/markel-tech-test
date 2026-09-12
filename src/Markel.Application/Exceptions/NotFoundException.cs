namespace Markel.Application.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string entityName, object key)
        : base($"{entityName} '{key}' was not found.")
    {
        EntityName = entityName;
        Key = key;
    }

    public string EntityName { get; }
    public object Key { get; }
}
