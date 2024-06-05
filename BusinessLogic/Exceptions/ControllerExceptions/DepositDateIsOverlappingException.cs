namespace BusinessLogic.Exceptions.ControllerExceptions;

public class DepositDateIsOverlappingException : Exception
{
    public DepositDateIsOverlappingException(string message) : base(message) {}

}