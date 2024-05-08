namespace DepoQuick.Exceptions.RatingException;

public class InvalidStarsForRatingException : Exception 
{
    public InvalidStarsForRatingException(string message) : base(message) {}
}