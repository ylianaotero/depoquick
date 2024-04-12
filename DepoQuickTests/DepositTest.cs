using DepoQuick.Domain;
using DepoQuick.Domain.Exceptions.DepositExceptions;

namespace DepoQuickTests;

[TestClass]
public class DepositTest
{
    [TestMethod]
    [ExpectedException(typeof(DepositWithInvalidAreaException))] 
    public void TestInvalidArea()
    {
        char area = 'R';
        String size = "Grande"; 
        bool airConditioning = true;
        bool reserved = false;

        new Deposit(area, size, airConditioning, reserved);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositWithInvalidAreaException))] 
    public void TestInvalidSizeAndArea()
    {
        char area = 'a';
        String size = "Enorme";
        bool airConditioning = true;
        bool reserved = false;
        
        new Deposit(area, size, airConditioning, reserved);
    }
    
    [TestMethod]
    public void TestValidDeposit()
    {
        char area = 'A';
        String size = "Mediano";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved); 
        
        Assert.IsNotNull(newDeposit);
        Assert.AreEqual(area, newDeposit.Area);
        Assert.AreEqual(size, newDeposit.Size);
        Assert.AreEqual(airConditioning, newDeposit.AirConditioning);
        Assert.AreEqual(reserved, newDeposit.Reserved);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositWithInvalidSizeException))] 
    public void TestInvalidSize()
    {
        char area = 'A';
        String size = "Minusculo";
        bool airConditioning = true;
        bool reserved = false;
        
        new Deposit(area, size, airConditioning, reserved);
    }
    

    
    [TestMethod]
    public void TestAddRating()
    {
        int stars = 1;
        String comment = " "; 
        
        Rating newRating = new Rating(stars, comment); 
        
        char area = 'A';
        String size = "Pequeño";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved); 
        
        newDeposit.addRating(newRating);
        
        Assert.IsNotNull(newDeposit);
        Assert.AreEqual(area, newDeposit.Area);
        Assert.AreEqual(size, newDeposit.Size);
        Assert.AreEqual(airConditioning, newDeposit.AirConditioning);
        Assert.AreEqual(reserved, newDeposit.Reserved);
        CollectionAssert.Contains(newDeposit.getRating(), newRating);
    }
    
    [TestMethod]
    public void TestAddPromo()
    {
        double discountRate = 0.5;
        String label = "Ejemplo"; 
        DateTime dateFrom = new DateTime(2024, 4, 8).Date;
        DateTime dateTo  = new DateTime(2024, 4, 10).Date;
        DateRange dateRange = new DateRange(dateFrom, dateTo);
        
        Promotion newPromotion = new Promotion();
        
        newPromotion.DiscountRate = discountRate;
        newPromotion.ValidityDate = dateRange;
        newPromotion.Label = label; 
        
        char area = 'A';
        String size = "Pequeño";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved); 
        
        newDeposit.addPromotion(newPromotion);
        
        Assert.IsNotNull(newDeposit);
        Assert.AreEqual(area, newDeposit.Area);
        Assert.AreEqual(size, newDeposit.Size);
        Assert.AreEqual(airConditioning, newDeposit.AirConditioning);
        Assert.AreEqual(reserved, newDeposit.Reserved);
        CollectionAssert.Contains(newDeposit.getPromotions(), newPromotion);
    }
    
    [TestMethod]
    public void TestClculateSmallDepositPriceWithoutPromotionLessThan7DaysAndWithoutAirConditioning()
    {
        char area = 'A';
        String size = "Pequeño";
        bool airConditioning = false;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);

        int numberOfDays = 6;

        int basePrice = 50;

        int expectedDepositPrice = numberOfDays * basePrice; 
        
        Assert.AreEqual(expectedDepositPrice, newDeposit.calculatePrice(numberOfDays));

    }
    
    [TestMethod]
    public void TestClculateMediumDepositPriceWithoutPromotionLessThan7DaysAndWithoutAirConditioning()
    {
        char area = 'A';
        String size = "Mediano";
        bool airConditioning = false;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);

        int numberOfDays = 6;

        int basePrice = 75;

        int expectedDepositPrice = numberOfDays * basePrice; 
        
        Assert.AreEqual(expectedDepositPrice, newDeposit.calculatePrice(numberOfDays));

    }
    
    [TestMethod]
    public void TestClculateBigDepositPriceWithoutPromotionLessThan7DaysAndWithoutAirConditioning()
    {
        char area = 'A';
        String size = "Grande";
        bool airConditioning = false;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);

        int numberOfDays = 6;

        int basePrice = 100;

        int expectedDepositPrice = numberOfDays * basePrice; 
        
        Assert.AreEqual(expectedDepositPrice, newDeposit.calculatePrice(numberOfDays));

    }
    
    [TestMethod]
    public void TestClculateBigDepositPriceWithoutPromotionFor7DaysAndWithoutAirConditioning()
    {
        char area = 'A';
        String size = "Grande";
        bool airConditioning = false;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);

        int numberOfDays = 7;

        int basePrice = 100;

        double discountForNumberOfDays = 0.05; 

        int expectedDepositPrice = (int)(numberOfDays * basePrice * (1.0 - discountForNumberOfDays));

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.calculatePrice(numberOfDays));

    }
    
    [TestMethod]
    public void TestClculateBigDepositPriceWithoutPromotionFor14DaysAndWithoutAirConditioning()
    {
        char area = 'A';
        String size = "Grande";
        bool airConditioning = false;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);

        int numberOfDays = 14;

        int basePrice = 100;

        double discountForNumberOfDays = 0.05; 

        int expectedDepositPrice = (int)(numberOfDays * basePrice * (1.0 - discountForNumberOfDays));

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.calculatePrice(numberOfDays));

    }
    
    [TestMethod]
    public void TestClculateBigDepositPriceWithoutPromotionForMoreThan14DaysAndWithoutAirConditioning()
    {
        char area = 'A';
        String size = "Grande";
        bool airConditioning = false;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);

        int numberOfDays = 15;

        int basePrice = 100;

        double discountForNumberOfDays = 0.1; 

        int expectedDepositPrice = (int)(numberOfDays * basePrice * (1.0 - discountForNumberOfDays));

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.calculatePrice(numberOfDays));

    }
    
    [TestMethod]
    public void TestClculateBigDepositPriceWithoutPromotionForMoreThan14DaysAndWithAirConditioning()
    {
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);

        int numberOfDays = 15;

        int basePrice = 100;

        double discountForNumberOfDays = 0.1; 

        int expectedDepositPrice = (int)(numberOfDays * basePrice * (1.0 - discountForNumberOfDays));

        if (airConditioning)
        {
            expectedDepositPrice += (numberOfDays * 20); 
        }

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.calculatePrice(numberOfDays));

    }
    
    [TestMethod]
    public void TestClculateBigDepositPriceWithoutValidPromotionForMoreThan14DaysAndWithAirConditioning()
    {
        double discountRate = 0.5;
        String label = "Ejemplo"; 
        DateTime dateFrom = new DateTime(2020, 4, 8).Date;
        DateTime dateTo  = new DateTime(2020, 4, 10).Date;
        DateRange dateRange = new DateRange(dateFrom, dateTo);
        
        Promotion newPromotion = new Promotion();
        
        newPromotion.DiscountRate = discountRate;
        newPromotion.ValidityDate = dateRange;
        newPromotion.Label = label; 
        
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);
        
        newDeposit.addPromotion(newPromotion);

        int numberOfDays = 15;

        int basePrice = 100;

        double discountForNumberOfDays = 0.1; 

        int expectedDepositPrice = (int)(numberOfDays * basePrice * (1.0 - discountForNumberOfDays));

        if (airConditioning)
        {
            expectedDepositPrice += (numberOfDays * 20); 
        }

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.calculatePrice(numberOfDays));
        CollectionAssert.Contains(newDeposit.getPromotions(), newPromotion);

    }
    
    [TestMethod]
    public void TestClculateBigDepositPriceWithValidPromotionForMoreThan14DaysAndWithAirConditioning()
    {
        double discountRate = 0.5;
        String label = "Ejemplo"; 
        DateTime dateFrom = new DateTime(2020, 4, 8).Date;
        DateTime dateTo  = new DateTime(2030, 4, 10).Date;
        DateRange dateRange = new DateRange(dateFrom, dateTo);
        
        Promotion newPromotion = new Promotion();
        
        newPromotion.DiscountRate = discountRate;
        newPromotion.ValidityDate = dateRange;
        newPromotion.Label = label; 
        
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);
        
        newDeposit.addPromotion(newPromotion);

        int numberOfDays = 15;

        int basePrice = 100;

        double discountForNumberOfDays = 0.1; 

        int expectedDepositPrice = (int)(numberOfDays * basePrice * (1.0 - discountForNumberOfDays));

        if (airConditioning)
        {
            expectedDepositPrice += (numberOfDays * 20); 
        }

        if (dateFrom <= DateTime.Now.Date && dateTo >= DateTime.Now.Date)
        {
            expectedDepositPrice = (int)(expectedDepositPrice * (1.0 - discountRate));
        }

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.calculatePrice(numberOfDays));

    }
    
    

}