namespace DepoQuick.Domain.Exceptions.ControllerExceptions;

public class EmptyUserListException : Exception
{
    public EmptyUserListException(string message) : base(message) {}
}