namespace BusinessLogic.Exceptions.ControllerExceptions;

public class ActionRestrictedToAdministratorException : Exception
{
    public ActionRestrictedToAdministratorException(string message) : base(message) {}   
}