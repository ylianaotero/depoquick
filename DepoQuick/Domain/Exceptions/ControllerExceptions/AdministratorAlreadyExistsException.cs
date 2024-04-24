namespace DepoQuick.Domain.Exceptions.MemoryDataBaseExceptions;

public class AdministratorAlreadyExistsException : Exception
{
    public AdministratorAlreadyExistsException(string message) : base(message) {}   
}