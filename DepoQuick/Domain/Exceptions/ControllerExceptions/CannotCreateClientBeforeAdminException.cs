namespace DepoQuick.Domain.Exceptions.MemoryDataBaseExceptions;

public class CannotCreateClientBeforeAdminException : Exception
{
    public CannotCreateClientBeforeAdminException(string message) : base(message) {}
}