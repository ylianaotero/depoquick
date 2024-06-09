using System.ComponentModel.DataAnnotations;
using DepoQuick.Exceptions.RatingException;

namespace DepoQuick.Domain;

public class Rating
{
    private const string InvalidStarsMessage = "Número de estrellas no válido, debe estar entre 1 y 5";
    private const string InvalidCommentMessage = "Comentario no válido, debe tener entre 1 y 500 caracteres";
    
    private const int MaxCharacters = 500;
    private const int MinCharacters = 1;
    
    private const int MinStars = 1;
    private const int MaxStars = 5;
    
    [Key]
    public int Id { get; set; }
    public Reservation Reservation { get; set; }
    
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
    
    public Rating() {}
    
    public Rating(int stars, String comment)
    {
        Stars = stars;
        Comment = comment; 
    }

    public void UpdateRating(int newStars, string newComment)
    {
        Stars = newStars;
        Comment = newComment;
    }
    
    private void ValidateStars(int stars)
    {
        if (!StarsAreBetweenMinAndMax(stars))
        {
            throw new InvalidStarsForRatingException(InvalidStarsMessage);
        }
    }
    
    private bool StarsAreBetweenMinAndMax(int stars)
    {
        return MinStars <= stars && MaxStars >= stars; 
    }
    
    private void ValidateComment(String comment)
    {
        if (CommentHasMoreThanMaxCharacters(comment) || CommentHasLessThanMinCharacters(comment))
        {
            throw new InvalidCommentForRatingException(InvalidCommentMessage);
        }
    }
    
    private bool CommentHasMoreThanMaxCharacters(String comment)
    {
        return comment.Length > MaxCharacters; 
    }
    
    private bool CommentHasLessThanMinCharacters(String comment)
    {
        return comment.Length < MinCharacters;
    }
}