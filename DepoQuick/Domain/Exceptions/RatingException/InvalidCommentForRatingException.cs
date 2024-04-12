namespace DepoQuick.Domain.Exceptions.RatingException;

public class InvalidCommentForRatingException : Exception
{
    public InvalidCommentForRatingException(string message) : base(message) {}
}