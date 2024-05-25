using System.ComponentModel.DataAnnotations;
using DepoQuick.Exceptions.RatingException;

namespace DepoQuick.Domain;

public class Rating
{
    private const int MaxCharacters = 500;
    
    private static int s_lastId = 0;
    
    [Key]
    public int Id { get; set; }
    
    private int _stars;
    private String _comment;
    
    public int Stars
    {
        get => _stars;
        set
        {
            ValidateStars(value);
            _stars = value;
        }
    }
    
    public String Comment
    {
        get => _comment;
        set
        {
            ValidateComment(value);
            _comment = value;
        }
    }
    
    public Rating()
    {
       // Id = s_lastId + 1;
        //s_lastId++;
    }
    
    public Rating(int stars, String comment)
    {
        Stars = stars;
        Comment = comment; 
        
        //Id = s_lastId + 1;
        //s_lastId++;
    }

    public void UpdateRating(int newStars, string newComment)
    {
        Stars = newStars;
        Comment = newComment;
    }
    
    private void ValidateStars(int stars)
    {
        if (!StarsAreBetween1And5(stars))
        {
            throw new InvalidStarsForRatingException("Estrellas no válida, debe estar entre 1 y 5");
        }
    }
    
    private bool StarsAreBetween1And5(int stars)
    {
        return 0 < stars && 6 > stars; 
    }
    
    private void ValidateComment(String comment)
    {
        if (CommentHasMoreThan500Characters(comment))
        {
            throw new InvalidCommentForRatingException("Comentario no valido, debe tener menos de 500 caracteres");
        }
    }
    
    private bool CommentHasMoreThan500Characters(String comment)
    {
        return comment.Length > MaxCharacters; 
    }
}