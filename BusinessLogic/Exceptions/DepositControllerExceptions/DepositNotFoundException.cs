namespace BusinessLogic.Exceptions.DepositControllerExceptions;

public class DepositNotFoundException : Exception
{
    public DepositNotFoundException(string message) : base(message) {}
}