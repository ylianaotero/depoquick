namespace DepoQuick.Domain.Exceptions.MemoryDataBaseExceptions;

public class UserDoesNotExistException : Exception
{
    public UserDoesNotExistException(string message) : base(message) {} 
}