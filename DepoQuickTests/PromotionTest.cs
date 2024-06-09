using DepoQuick.Domain;
using DepoQuick.Exceptions.DateRangeExceptions;
using DepoQuick.Exceptions.PromotionExceptions;

namespace DepoQuickTests;

[TestClass]
public class PromotionTest
{
    [TestMethod]
    [ExpectedException(typeof(PromotionWithEmptyLabelException))] 
    public void TestEmptyLabel()
    {
        Promotion newPromotion = new Promotion();
        newPromotion.Label = "";
    }
    
    [TestMethod]
    public void TestLabelWithSpaces()
    {
        Promotion newPromotion = new Promotion();

        newPromotion.Label = "12 12"; 
        
        Assert.AreEqual("12 12", newPromotion.Label);
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(PromotionLabelHasMoreThan20CharactersException))] 
    public void TestLabelWithMoreThan20Characters()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.Label = "acahay21caracteressss"; 

    }
    
    [TestMethod]
    public void TestLabelWith20Characters()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.Label = "acahay20caracteresss";  
        
        Assert.AreEqual("acahay20caracteresss", newPromotion.Label);
        
    }
    
    [TestMethod]
    public void TestValidPromotionWithEmptyListOfDeposits()
    {
        Promotion newPromotion = new Promotion();
        List<Deposit> emptyListOfDeposits = new List<Deposit>(); 
        
        newPromotion.Label = "label";
        newPromotion.Id = 0;
        newPromotion.Deposits = emptyListOfDeposits; 
        
        Assert.AreEqual("label", newPromotion.Label);
        Assert.AreEqual(0, newPromotion.Id);
        Assert.AreEqual(emptyListOfDeposits, newPromotion.Deposits);
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidPercentageForPromotionException))] 
    public void TestPromotionWithLessThan5Percent()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.DiscountRate = 0; 
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidPercentageForPromotionException))] 
    public void TestPromotionWithNegativeNumbers()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.DiscountRate = -100; 
        
    }
    
    [TestMethod]
    public void Test5PercentPromotion()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.DiscountRate = 0.05; 
        
        Assert.AreEqual(0.05, newPromotion.DiscountRate);
        
    }
    
    [TestMethod]
    public void Test75PercentPromotion()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.DiscountRate = 0.75;
        
        Assert.AreEqual(0.75, newPromotion.DiscountRate);
        
    }
    
    [TestMethod]
    public void TestValidPromotion()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.DiscountRate = 0.55;
        
        Assert.AreEqual(0.55,newPromotion.DiscountRate);
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidPercentageForPromotionException))] 
    public void TestPromotionGreaterThan75Percent()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.DiscountRate = 4;
        
    }
    
    [TestMethod]
    public void TestValidValidityDate()
    {
        Promotion newPromotion = new Promotion();
        
        DateTime dateFrom = new DateTime(2024, 4, 8).Date;
        DateTime dateTo  = new DateTime(2024, 4, 10).Date;

        DateRange dateRange = new DateRange(dateFrom, dateTo);

        newPromotion.ValidityDate = dateRange;
        
        Assert.AreEqual(dateRange, newPromotion.ValidityDate);
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidDateRangeException))] 
    public void TestDateFromGreaterToDateTo()
    {
        Promotion newPromotion = new Promotion();
        
        DateTime dateFrom = new DateTime(2024, 4, 10).Date;
        DateTime dateTo  = new DateTime(2024, 4, 8).Date;
        
        new DateRange(dateFrom, dateTo );
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(EmptyDateRangeException))] 
    public void TestEmptyDate()
    {
        Promotion newPromotion = new Promotion();
        
        DateTime dateFrom = new DateTime().Date;
        DateTime dateTo  = new DateTime(2024, 4, 8).Date;
        
        new DateRange(dateFrom, dateTo );
        
    }

    [TestMethod]
    public void TestPromotionIsCurrentlyAvailable()
    {
        Promotion newPromotion = new Promotion();
        DateRange dateRange = new DateRange(new DateTime(2024, 3, 8).Date, new DateTime(2024, 3, 10).Date);
        newPromotion.ValidityDate = dateRange;
        
        Assert.IsFalse(newPromotion.IsCurrentlyAvailable());
    }
    
} 