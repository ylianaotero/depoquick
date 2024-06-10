namespace BusinessLogic.Exceptions.DepositControllerExceptions;

public class DepositDateIsOverlappingException : Exception
{
    public DepositDateIsOverlappingException(string message) : base(message) {}

}