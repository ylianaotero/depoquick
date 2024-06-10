namespace BusinessLogic.Exceptions.UserControllerExceptions;

public class AdministratorAlreadyExistsException : Exception
{
    public AdministratorAlreadyExistsException(string message) : base(message) {}   
}