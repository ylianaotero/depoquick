namespace DepoQuick.Domain.Exceptions.PromotionExceptions;

public class PromotionWithEmptyLabelException : Exception
{
    public PromotionWithEmptyLabelException(string message) : base(message) {}
}