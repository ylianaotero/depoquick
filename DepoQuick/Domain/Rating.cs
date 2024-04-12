using DepoQuick.Domain.Exceptions.RatingException;

namespace DepoQuick.Domain;

public class Rating
{
    private int _id; 
    private const int DEFAULT_MAX_CHARACTERS = 500;
    private int _stars;
    private String _comment;
    
    public int Id
    {
        get => _id;
        set => _id = value;
    }
    
    public Rating(int stars, String comment)
    {
        if (ratingIsValid(stars, comment))
        {
            _stars = stars;
            _comment = comment; 
        }
    }

    private bool ratingIsValid(int stars, String comment)
    {
        return starsAreValid(stars) && commentIsValid(comment); 
    }
    
    
    public int Stars
    {
        get => _stars;
        set
        {
            if(starsAreValid(value))
            {
                _stars = value; 
            }
        }
    }
    
    public String Comment
    {
        get => _comment;
        set
        {
            if(commentIsValid(value))
            {
                _comment = value; 
            }
        }
    }

    private bool commentIsValid(String comment)
    {
        if (commentHasMoreThan500Characters(comment))
        {
            throw new InvalidCommentForRatingException("Comentario no valido, debe tener menos de 500 caracteres");
        }
        else
        {
            return true; 
        }
    }
    
    private bool commentHasMoreThan500Characters(String comment)
    {
        return comment.Length > DEFAULT_MAX_CHARACTERS; 
    }

    private bool starsAreValid(int stars)
    {
        if (starsAreBetween1And5(stars))
        {
            return true; 
        }
        else
        {
            throw new InvalidStarsForRatingException("Estrellas no válida, debe estar entre 1 y 5");
        }
    }

    private bool starsAreBetween1And5(int stars)
    {
        return 0 < stars && 6 > stars; 
    }
}