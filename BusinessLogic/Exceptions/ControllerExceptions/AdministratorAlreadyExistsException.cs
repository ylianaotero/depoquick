namespace BusinessLogic.Exceptions.ControllerExceptions;

public class AdministratorAlreadyExistsException : Exception
{
    public AdministratorAlreadyExistsException(string message) : base(message) {}   
}