namespace BusinessLogic.Exceptions.UserControllerExceptions;

public class ActionRestrictedToAdministratorException : Exception
{
    public ActionRestrictedToAdministratorException(string message) : base(message) {}   
}