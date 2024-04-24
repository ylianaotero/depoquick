namespace DepoQuick.Domain.Exceptions.MemoryDataBaseExceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string message) : base(message) {}   
}