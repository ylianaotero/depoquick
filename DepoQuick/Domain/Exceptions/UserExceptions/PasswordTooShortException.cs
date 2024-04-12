namespace DepoQuick.Domain.Exceptions.UserExceptions;

public class PasswordTooShortException : Exception
{
    public PasswordTooShortException(string message) : base(message) {}
}