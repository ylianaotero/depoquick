namespace BusinessLogic.Exceptions.ControllerExceptions;

public class CannotCreateClientBeforeAdminException : Exception
{
    public CannotCreateClientBeforeAdminException(string message) : base(message) {}
}