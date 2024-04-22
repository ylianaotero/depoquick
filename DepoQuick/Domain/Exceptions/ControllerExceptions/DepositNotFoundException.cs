namespace DepoQuick.Domain.Exceptions.ControllerExceptions;

public class DepositNotFoundException : Exception
{
    public DepositNotFoundException(string message) : base(message) {}
}