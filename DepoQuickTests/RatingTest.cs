using DepoQuick.Domain;
using DepoQuick.Exceptions.RatingException;

namespace DepoQuickTests;

[TestClass]
public class RatingTest
{
    [TestMethod]
    public void TestRating()
    {
        int stars = 5;
        String comment = "Excelente servicio"; 
        
        Rating newRating = new Rating(); 
        newRating.Stars = stars;
        newRating.Comment = comment;
        
        Assert.IsNotNull(newRating);
        Assert.AreEqual(stars, newRating.Stars);
        Assert.AreEqual(comment, newRating.Comment);
        Assert.IsTrue(0 >= newRating.Id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidStarsForRatingException))] 
    public void TestRatingInvalidStars()
    {
        int stars = 0;
        String comment = "this is a comment"; 
        
        new Rating(stars,comment);
    }
    
    [TestMethod]
    public void Test1Star()
    {
        int stars = 1;
        String comment = " "; 
        
        Rating newRating = new Rating(stars, comment); 
        
        Assert.IsNotNull(newRating);
        Assert.AreEqual(stars, newRating.Stars);
        Assert.AreEqual(comment, newRating.Comment);
    }
    
    [TestMethod]
    public void Test5Star()
    {
        int stars = 5;
        String comment = " "; 
        
        Rating newRating = new Rating(stars, comment); 
        
        Assert.IsNotNull(newRating);
        Assert.AreEqual(stars, newRating.Stars);
        Assert.AreEqual(comment, newRating.Comment);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidCommentForRatingException))] 
    public void TestRatingCommentWith501Characters()
    {
        int stars = 1;
        String comment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam vel sem ligula. Vivamus nec " +
                         "arcu non velit tempus euismod. Mauris suscipit arcu non ex ultrices vehicula. Integer vehicula nulla " +
                         "a suscipit venenatis. Fusce vel felis vel enim vehicula scelerisque. Aliquam quis massa eget magna semper " +
                         "blandit. Ut a quam quis felis vulputate hendrerit. Aenean sagittis felis ac mauris mattis, sit amet luctus " +
                         "eros gravida. Nunc quis leo quis purus consectetur efficitur id at libero. Pellentesque dapibut";
        
        Assert.AreEqual(501, comment.Length);
        
        new Rating(stars, comment);
    }
    
    [TestMethod]
    public void TestRatingCommentWith500Characters()
    {
        int stars = 1;
        String comment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam vel sem ligula. Vivamus nec " +
                         "arcu non velit tempus euismod. Mauris suscipit arcu non ex ultrices vehicula. Integer vehicula nulla " +
                         "a suscipit venenatis. Fusce vel felis vel enim vehicula scelerisque. Aliquam quis massa eget magna semper " +
                         "blandit. Ut a quam quis felis vulputate hendrerit. Aenean sagittis felis ac mauris mattis, sit amet luctus " +
                         "eros gravida. Nunc quis leo quis purus consectetur efficitur id at libero. Pellentesque dapibu"; 
        
        Assert.AreEqual(500, comment.Length);
        
        Rating newRating = new Rating(stars, comment); 
        
        
        Assert.IsNotNull(newRating);
        Assert.AreEqual(stars, newRating.Stars);
        Assert.AreEqual(comment, newRating.Comment);
    }
    
    [TestMethod]
    public void TestUpdateRating()
    {
        int stars = 1;
        String comment = " "; 
        
        Rating newRating = new Rating(stars, comment); 
        
        Assert.IsNotNull(newRating);
        Assert.AreEqual(stars, newRating.Stars);
        Assert.AreEqual(comment, newRating.Comment);
        
        int newStars = 5;
        String newComment = "new comment"; 
        
        newRating.UpdateRating(newStars, newComment);
        
        Assert.AreEqual(newStars, newRating.Stars);
        Assert.AreEqual(newComment, newRating.Comment);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidStarsForRatingException))]
    public void TestUpdateRatingWithInvalidStars()
    {
        int stars = 1;
        String comment = " "; 
        
        Rating newRating = new Rating(stars, comment); 
        
        Assert.IsNotNull(newRating);
        Assert.AreEqual(stars, newRating.Stars);
        Assert.AreEqual(comment, newRating.Comment);
        
        int newStars = 6;
        String newComment = "new comment"; 
        
        newRating.UpdateRating(newStars, newComment);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidCommentForRatingException))]
    public void TestUpdateRatingWithInvalidComment()
    {
        int stars = 1;
        String comment = " "; 
        
        Rating newRating = new Rating(stars, comment); 
        
        Assert.IsNotNull(newRating);
        Assert.AreEqual(stars, newRating.Stars);
        Assert.AreEqual(comment, newRating.Comment);
        
        int newStars = 5;
        String newComment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam vel sem ligula. Vivamus nec " +
                         "arcu non velit tempus euismod. Mauris suscipit arcu non ex ultrices vehicula. Integer vehicula nulla " +
                         "a suscipit venenatis. Fusce vel felis vel enim vehicula scelerisque. Aliquam quis massa eget magna semper " +
                         "blandit. Ut a quam quis felis vulputate hendrerit. Aenean sagittis felis ac mauris mattis, sit amet luctus " +
                         "eros gravida. Nunc quis leo quis purus consectetur efficitur id at libero. Pellentesque dapibut";
        
        Assert.AreEqual(501, newComment.Length);
        
        newRating.UpdateRating(newStars, newComment);
    }
}