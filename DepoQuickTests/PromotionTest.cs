using DepoQuick.Domain;
using DepoQuick.Domain.Exceptions.DateRangeExceptions;
using DepoQuick.Domain.Exceptions.PromotionExceptions;

namespace DepoQuickTests;

[TestClass]
public class PromotionTest
{
    [TestMethod]
    [ExpectedException(typeof(PromotionWithEmptyLabelException))] 
    public void TestEmptyLabel()
    {
        Promotion newPromotion = new Promotion();

        newPromotion.SetLabel(" "); 
        
    }
    
    [TestMethod]
    public void TestLabelWithSpaces()
    {
        Promotion newPromotion = new Promotion();

        newPromotion.SetLabel("12 12"); 
        
        Assert.AreEqual("12 12", newPromotion.GetLabel());
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(PromotionLabelHasMoreThan20CharactersException))] 
    public void TestLabelWithMoreThan20Characters()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.SetLabel("acahay21caracteressss"); 

    }
    
    [TestMethod]
    public void TestLabelWith20Characters()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.SetLabel( "acahay20caracteresss");  
        
        Assert.AreEqual("acahay20caracteresss", newPromotion.GetLabel());
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidPercentageForPromotionException))] 
    public void TestPromotionWithLessThan5Percent()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.SetDiscountRate(0); 
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidPercentageForPromotionException))] 
    public void TestPromotionWithNegativeNumbers()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.SetDiscountRate(-100); 
        
    }
    
    [TestMethod]
    public void Test5PercentPromotion()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.SetDiscountRate(0.05); 
        
        Assert.AreEqual(0.05, newPromotion.GetDiscountRate());
        
    }
    
    [TestMethod]
    public void Test75PercentPromotion()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.SetDiscountRate(0.75);
        
        Assert.AreEqual(0.75, newPromotion.GetDiscountRate());
        
    }
    
    [TestMethod]
    public void TestValidPromotion()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.SetDiscountRate(0.55);
        
        Assert.AreEqual(0.55,newPromotion.GetDiscountRate());
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidPercentageForPromotionException))] 
    public void TestPromotionGreaterThan75Percent()
    {
        Promotion newPromotion = new Promotion();
        
        newPromotion.SetDiscountRate(4);
        
    }
    
    [TestMethod]
    public void TestValidValidityDate()
    {
        Promotion newPromotion = new Promotion();
        
        DateTime dateFrom = new DateTime(2024, 4, 8).Date;
        DateTime dateTo  = new DateTime(2024, 4, 10).Date;

        DateRange dateRange = new DateRange(dateFrom, dateTo);

        newPromotion.SetValidityDate(dateRange);
        
        Assert.AreEqual(dateRange, newPromotion.GetValidityDate());
        
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
    public void TestGetDepositsWithPromotion()
    {
        Promotion newPromotion = new Promotion();
        
        Deposit smallDeposit = new Deposit('A', "pequeño", false, false);
        Deposit bigDeposit = new Deposit('B', "grande", true, false);
        
        smallDeposit.AddPromotion(newPromotion);
        bigDeposit.AddPromotion(newPromotion);
        
        newPromotion.AddDeposit(smallDeposit);
        newPromotion.AddDeposit(bigDeposit);
        
        CollectionAssert.Contains(newPromotion.GetDeposits(), smallDeposit);
        CollectionAssert.Contains(newPromotion.GetDeposits(), bigDeposit);
    }
    
    [TestMethod]
    public void TestRemoveDepositFromPromotion()
    {
        Promotion newPromotion = new Promotion();
        
        Deposit smallDeposit = new Deposit('A', "pequeño", false, false);
        
        newPromotion.AddDeposit(smallDeposit);
        
        newPromotion.RemoveDeposit(smallDeposit);
        
        CollectionAssert.DoesNotContain(newPromotion.GetDeposits(), smallDeposit);
    }
    
} 