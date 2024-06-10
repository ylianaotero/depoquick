namespace BusinessLogic.Exceptions.DepositControllerExceptions;

public class DepositNameAlreadyExistsException : Exception
{
    public DepositNameAlreadyExistsException(string message) : base(message)
    {
    }
}