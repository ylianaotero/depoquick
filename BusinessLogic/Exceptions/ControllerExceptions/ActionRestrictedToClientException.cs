namespace BusinessLogic.Exceptions.ControllerExceptions;

public class ActionRestrictedToClientException : Exception
{
    public ActionRestrictedToClientException(string message) : base(message) {} 
}