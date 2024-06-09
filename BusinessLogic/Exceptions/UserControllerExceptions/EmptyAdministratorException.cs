namespace BusinessLogic.Exceptions.UserControllerExceptions;

public class EmptyAdministratorException : Exception
{
    public EmptyAdministratorException(string message) : base(message) {}  
}