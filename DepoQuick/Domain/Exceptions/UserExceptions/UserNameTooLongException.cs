namespace DepoQuick.Domain.Exceptions.UserExceptions;

public class UserNameTooLongException : Exception
{
    public UserNameTooLongException(string message) : base(message) { }
}