namespace BusinessLogic.Exceptions.ControllerExceptions;

public class EmptyAdministratorException : Exception
{
    public EmptyAdministratorException(string message) : base(message) {}  
}