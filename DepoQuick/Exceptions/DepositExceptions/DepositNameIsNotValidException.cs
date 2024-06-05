namespace DepoQuick.Exceptions.DepositExceptions;

public class DepositNameIsNotValidException : Exception
{
    public DepositNameIsNotValidException(string message) : base(message) {}
}