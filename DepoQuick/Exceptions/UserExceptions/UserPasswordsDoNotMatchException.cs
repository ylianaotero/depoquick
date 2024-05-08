namespace DepoQuick.Exceptions.UserExceptions;

public class UserPasswordsDoNotMatchException : Exception
{
    public UserPasswordsDoNotMatchException(string message) : base(message) {}
}