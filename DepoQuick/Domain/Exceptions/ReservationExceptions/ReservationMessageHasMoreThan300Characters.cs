namespace DepoQuick.Domain.Exceptions.ReservationExceptions;

public class ReservationMessageHasMoreThan300CharactersException : Exception
{
    public ReservationMessageHasMoreThan300CharactersException(string message) : base(message) {}
}