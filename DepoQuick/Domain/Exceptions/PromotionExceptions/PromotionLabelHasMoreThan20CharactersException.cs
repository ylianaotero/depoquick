namespace DepoQuick.Domain.Exceptions.PromotionExceptions;

public class PromotionLabelHasMoreThan20CharactersException : Exception
{
    public PromotionLabelHasMoreThan20CharactersException(string message) : base(message) {}
}