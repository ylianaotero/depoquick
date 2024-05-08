namespace DepoQuick.Exceptions.RatingException;

public class InvalidCommentForRatingException : Exception
{
    public InvalidCommentForRatingException(string message) : base(message) {}
}