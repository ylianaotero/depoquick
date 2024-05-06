using DepoQuick.Domain.Exceptions.RatingException;

namespace DepoQuick.Domain;

public class Rating
{
    private const int DefaultMaxCharacters = 500;
    
    private int _stars;
    private String _comment;
    
    public Rating(int stars, String comment)
    {
        if (RatingIsValid(stars, comment))
        {
            _stars = stars;
            _comment = comment; 
        }
    }

    public int GetStars()
    {
        return _stars; 
    }

    public String GetComment()
    {
        return _comment; 
    }

    public void UpdateRating(int newStars, string newComment)
    {
        _stars = newStars;
        _comment = newComment;
    }

    private bool RatingIsValid(int stars, String comment)
    {
        return StarsAreValid(stars) && CommentIsValid(comment); 
    }
    
    private bool StarsAreValid(int stars)
    {
        if (StarsAreBetween1And5(stars))
        {
            return true; 
        }
        else
        {
            throw new InvalidStarsForRatingException("Estrellas no válida, debe estar entre 1 y 5");
        }
    }
    
    private bool StarsAreBetween1And5(int stars)
    {
        return 0 < stars && 6 > stars; 
    }
    
    
    private bool CommentIsValid(String comment)
    {
        if (CommentHasMoreThan500Characters(comment))
        {
            throw new InvalidCommentForRatingException("Comentario no valido, debe tener menos de 500 caracteres");
        }
        else
        {
            return true; 
        }
    }
    
    private bool CommentHasMoreThan500Characters(String comment)
    {
        return comment.Length > DefaultMaxCharacters; 
    }
    
}