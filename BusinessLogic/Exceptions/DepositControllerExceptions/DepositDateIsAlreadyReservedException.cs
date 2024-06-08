namespace BusinessLogic.Exceptions.DepositControllerExceptions;

public class DepositDateIsAlreadyReservedException : Exception
{
    public DepositDateIsAlreadyReservedException(string message) : base(message)
    {
    }
}