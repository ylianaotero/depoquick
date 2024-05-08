namespace DepoQuick.Exceptions.UserExceptions;

public class PasswordTooShortException : Exception
{
    public PasswordTooShortException(string message) : base(message) {}
}