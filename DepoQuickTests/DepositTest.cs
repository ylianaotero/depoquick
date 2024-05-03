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
        char area = 'l';
        String size = "Enorme";
        bool airConditioning = true;
        bool reserved = false;
        
        new Deposit(area, size, airConditioning, reserved);
    }
    
    [TestMethod]
    public void TestValidDeposit()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved); 
        
        Assert.IsNotNull(newDeposit);
        Assert.AreEqual(char.ToUpper(area), newDeposit.GetArea());
        Assert.AreEqual(size.ToUpper(), newDeposit.GetSize());
        Assert.AreEqual(airConditioning, newDeposit.GetAirConditioning());
        Assert.AreEqual(reserved, newDeposit.IsReserved());
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
        
        newDeposit.AddRating(newRating);
        
        Assert.IsNotNull(newDeposit);
        Assert.AreEqual(char.ToUpper(area), newDeposit.GetArea());
        Assert.AreEqual(size.ToUpper(), newDeposit.GetSize());
        Assert.AreEqual(airConditioning, newDeposit.GetAirConditioning());
        Assert.AreEqual(reserved, newDeposit.IsReserved());
        CollectionAssert.Contains(newDeposit.GetRatings(), newRating);
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
        
        newPromotion.SetDiscountRate(discountRate); 
        newPromotion.SetValidityDate(dateRange);
        newPromotion.SetLabel(label);
        
        char area = 'A';
        String size = "Pequeño";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved); 
        
        newDeposit.AddPromotion(newPromotion);
        
        Assert.IsNotNull(newDeposit);
        Assert.AreEqual(char.ToUpper(area), newDeposit.GetArea());
        Assert.AreEqual(size.ToUpper(), newDeposit.GetSize());
        Assert.AreEqual(airConditioning, newDeposit.GetAirConditioning());
        Assert.AreEqual(reserved, newDeposit.IsReserved());
        CollectionAssert.Contains(newDeposit.GetPromotions(), newPromotion);
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
        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

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
        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

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
        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

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

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

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

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

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

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

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

        int basePrice = 100*15;

        double discountForNumberOfDays = 0.1; 

        if (airConditioning)
        {
            basePrice += (numberOfDays * 20); 
        }
        
        basePrice = (int)(basePrice * (1.0 - discountForNumberOfDays));

        
        Assert.AreEqual(basePrice, newDeposit.CalculatePrice(numberOfDays));

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
        
        newPromotion.SetDiscountRate(discountRate); 
        newPromotion.SetValidityDate(dateRange);
        newPromotion.SetLabel(label);
        
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);
        
        newDeposit.AddPromotion(newPromotion);

        int numberOfDays = 15;

        int basePrice = 100*15;

        double discountForNumberOfDays = 0.1; 

        if (airConditioning)
        {
            basePrice += (numberOfDays * 20); 
        }
        
        basePrice = (int)(basePrice * (1.0 - discountForNumberOfDays));

        
        Assert.AreEqual(basePrice, newDeposit.CalculatePrice(numberOfDays));
        CollectionAssert.Contains(newDeposit.GetPromotions(), newPromotion);

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
        
        newPromotion.SetDiscountRate(discountRate); 
        newPromotion.SetValidityDate(dateRange);
        newPromotion.SetLabel(label);
        
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);
        
        newDeposit.AddPromotion(newPromotion);

        int numberOfDays = 15;

        int basePrice = 100*15;

        double discount = 0.1; 

        if (airConditioning)
        {
            basePrice += (numberOfDays * 20); 
        }
        
        if (dateFrom <= DateTime.Now.Date && dateTo >= DateTime.Now.Date)
        {
            discount += discountRate; 
        }
        
        basePrice = (int)(basePrice * (1.0 - discount));

        
        Assert.AreEqual(basePrice, newDeposit.CalculatePrice(numberOfDays));

    }

    [TestMethod]
    public void TestClculateBigDepositPriceWithTwoPromotionsOfForMoreThan14DaysAndWithAirConditioning()
    {
        double discountRate = 0.5;
        String label = "Ejemplo"; 
        DateTime dateFrom = new DateTime(2020, 4, 8).Date;
        DateTime dateTo  = new DateTime(2030, 4, 10).Date;
        DateRange dateRange = new DateRange(dateFrom, dateTo);
        
        Promotion newPromotion = new Promotion();
        Promotion newPromotion2 = new Promotion();
        
        newPromotion.SetDiscountRate(discountRate); 
        newPromotion.SetValidityDate(dateRange);
        newPromotion.SetLabel(label);
        
        newPromotion2.SetDiscountRate(discountRate); 
        newPromotion2.SetValidityDate(dateRange);
        newPromotion2.SetLabel(label);
        
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);
        
        newDeposit.AddPromotion(newPromotion);
        newDeposit.AddPromotion(newPromotion2);

        int numberOfDays = 15;

        int basePrice = 100*15;

        double discount = 0.1 + 0.5; 

        if (airConditioning)
        {
            basePrice += (numberOfDays * 20); 
        }
        
        basePrice = (int)(basePrice * (1.0 - discount));

        
        Assert.AreEqual(basePrice, newDeposit.CalculatePrice(numberOfDays));

    }


    [TestMethod]
    public void TestClculateBigDepositPriceWithTwoPromotionsOfForMoreThan14DaysAndWithAirConditioningWithADiscountOf100()
    {
        double discountRate = 0.45;
        String label = "Ejemplo"; 
        DateTime dateFrom = new DateTime(2020, 4, 8).Date;
        DateTime dateTo  = new DateTime(2030, 4, 10).Date;
        DateRange dateRange = new DateRange(dateFrom, dateTo);
        
        Promotion newPromotion = new Promotion();
        Promotion newPromotion2 = new Promotion();
        
        newPromotion.SetDiscountRate(discountRate); 
        newPromotion.SetValidityDate(dateRange);
        newPromotion.SetLabel(label);
        
        newPromotion2.SetDiscountRate(discountRate); 
        newPromotion2.SetValidityDate(dateRange);
        newPromotion2.SetLabel(label);
        
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);
        
        newDeposit.AddPromotion(newPromotion);
        newDeposit.AddPromotion(newPromotion2);

        int numberOfDays = 15;

        
        Assert.AreEqual(0, newDeposit.CalculatePrice(numberOfDays));

    }

    
    [TestMethod]
    public void TestAddIdToAValidDeposit()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning, reserved);

        int id = 2; 

        newDeposit.SetId(id); 
        
        Assert.AreEqual(id, newDeposit.GetId());
    }
    
    [TestMethod]
    public void TestTwoDepositsHaveDifferentIDs()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit1 = new Deposit(area, size, airConditioning, reserved);
        Deposit newDeposit2 = new Deposit(area, size, airConditioning, reserved); 

        Assert.AreNotEqual(newDeposit1.GetId(), newDeposit2.GetId());
    }

    [TestMethod]
    public void TestIDIsIncremental()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        bool reserved = false;
        
        Deposit newDeposit1 = new Deposit(area, size, airConditioning, reserved);
        Deposit newDeposit2 = new Deposit(area, size, airConditioning, reserved);

        Assert.IsTrue(newDeposit1.GetId() < newDeposit2.GetId());
    }
    
}