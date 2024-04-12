namespace Obli.Exceptions.PromotionExceptions;

public class InvalidPercentageForPromotionException : Exception
{
    public InvalidPercentageForPromotionException(string message) : base(message) {}
}