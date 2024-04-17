namespace DepoQuick.Domain.Exceptions.ControllerExceptions;

public class ExceptionDepositNotFound : Exception
{
    public ExceptionDepositNotFound(string message) : base(message) {}
}