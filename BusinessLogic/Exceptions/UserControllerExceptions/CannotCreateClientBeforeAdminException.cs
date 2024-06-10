namespace BusinessLogic.Exceptions.UserControllerExceptions;

public class CannotCreateClientBeforeAdminException : Exception
{
    public CannotCreateClientBeforeAdminException(string message) : base(message) {}
}