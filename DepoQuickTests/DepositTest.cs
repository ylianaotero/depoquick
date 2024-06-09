using DepoQuick.Domain;
using DepoQuick.Exceptions.DepositExceptions;

namespace DepoQuickTests;

[TestClass]
public class DepositTest
{
    private const string ClientName1 = "Maria Perez";
    private const string ClientEmail1 = "maria@gmail.com";
    private const string ClientPassword1 = "Mariaaa1.";
    
    [TestMethod]
    public void TestDepositConstructor()
    {
        string name = "Deposito";
        char area = 'A';
        String size = "Pequeño";
        bool airConditioning = true;
        
        Deposit newDeposit = new Deposit(); 
        newDeposit.Area = area;
        newDeposit.Size = size;
        newDeposit.AirConditioning = airConditioning;
        newDeposit.Id = 0;
        
        Assert.IsNotNull(newDeposit);
        Assert.AreEqual(char.ToUpper(area), newDeposit.Area);
        Assert.AreEqual(size.ToUpper(), newDeposit.Size);
        Assert.AreEqual(airConditioning, newDeposit.AirConditioning);
        Assert.AreEqual(0, newDeposit.Id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositWithInvalidAreaException))] 
    public void TestInvalidArea()
    {
        string name = "Deposito";
        char area = 'R';
        String size = "Grande"; 
        bool airConditioning = true;
        
        new Deposit(name,area, size, airConditioning);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositWithInvalidAreaException))] 
    public void TestInvalidSizeAndArea()
    {
        string name = "Deposito";
        char area = 'l';
        String size = "Enorme";
        bool airConditioning = true;
        
        
        new Deposit(name,area, size, airConditioning);
    }
    
    [TestMethod]
    public void TestValidDeposit()
    {
        string name = "Deposito";
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        
        Deposit newDeposit = new Deposit(name,area, size, airConditioning); 
        
        Assert.IsNotNull(newDeposit);
        Assert.AreEqual(char.ToUpper(area), newDeposit.Area);
        Assert.AreEqual(size.ToUpper(), newDeposit.Size);
        Assert.AreEqual(airConditioning, newDeposit.AirConditioning);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositWithInvalidSizeException))] 
    public void TestInvalidSize()
    {
        string name = "Deposito";
        char area = 'A';
        String size = "Minusculo";
        bool airConditioning = true;
        
        
        new Deposit(name,area, size, airConditioning);
    }
    
    [TestMethod]
    public void TestAddRating()
    {
        string name = "Deposito";
        int stars = 1;
        String comment = " "; 
        
        Rating newRating = new Rating(stars, comment); 
        
        char area = 'A';
        String size = "Pequeño";
        bool airConditioning = true;
        
        
        Deposit newDeposit = new Deposit(name,area, size, airConditioning); 
        
        newDeposit.AddRating(newRating);
        
        Assert.IsNotNull(newDeposit);
        Assert.AreEqual(char.ToUpper(area), newDeposit.Area);
        Assert.AreEqual(size.ToUpper(), newDeposit.Size);
        Assert.AreEqual(airConditioning, newDeposit.AirConditioning);
        CollectionAssert.Contains(newDeposit.Ratings, newRating);
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
        
        string name = "Deposito";
        char area = 'A';
        String size = "Pequeño";
        bool airConditioning = true;
        
        Deposit newDeposit = new Deposit(name,area, size, airConditioning); 
        
        newDeposit.AddPromotion(newPromotion);
        
        Assert.IsNotNull(newDeposit);
        Assert.AreEqual(char.ToUpper(area), newDeposit.Area);
        Assert.AreEqual(size.ToUpper(), newDeposit.Size);
        Assert.AreEqual(airConditioning, newDeposit.AirConditioning);
        CollectionAssert.Contains(newDeposit.Promotions, newPromotion);
    }
        
    [TestMethod]
    public void TestCalculateSmallDepositPriceWithoutPromotionLessThan7DaysAndWithoutAirConditioning()
    {
        string name = "Deposito";
        char area = 'A';
        String size = "Pequeño";
        bool airConditioning = false;
        
        Deposit newDeposit = new Deposit(name,area, size, airConditioning);

        int numberOfDays = 6;

        int basePrice = 50;

        int expectedDepositPrice = numberOfDays * basePrice; 
        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

    }

    [TestMethod]
    public void TestCalculateMediumDepositPriceWithoutPromotionLessThan7DaysAndWithoutAirConditioning()
    {
        string name = "Deposito";
        char area = 'A';
        String size = "Mediano";
        bool airConditioning = false;
        
        Deposit newDeposit = new Deposit(name,area, size, airConditioning);

        int numberOfDays = 6;

        int basePrice = 75;

        int expectedDepositPrice = numberOfDays * basePrice; 
        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

    }

    [TestMethod]
    public void TestCalculateBigDepositPriceWithoutPromotionLessThan7DaysAndWithoutAirConditioning()
    {
        string name = "Deposito";
        char area = 'A';
        String size = "Grande";
        bool airConditioning = false;
        
        Deposit newDeposit = new Deposit(name,area, size, airConditioning);

        int numberOfDays = 6;

        int basePrice = 100;

        int expectedDepositPrice = numberOfDays * basePrice; 
        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

    }

    [TestMethod]
    public void TestCalculateBigDepositPriceWithoutPromotionFor7DaysAndWithoutAirConditioning()
    {
        string name = "Deposito";
        char area = 'A';
        String size = "Grande";
        bool airConditioning = false;
        
        
        Deposit newDeposit = new Deposit(name,area, size, airConditioning);

        int numberOfDays = 7;

        int basePrice = 100;

        double discountForNumberOfDays = 0.05; 

        int expectedDepositPrice = (int)(numberOfDays * basePrice * (1.0 - discountForNumberOfDays));

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

    }

    [TestMethod]
    public void TestCalculateBigDepositPriceWithoutPromotionFor14DaysAndWithoutAirConditioning()
    {
        string name = "Deposito";
        char area = 'A';
        String size = "Grande";
        bool airConditioning = false;
        
        
        Deposit newDeposit = new Deposit(name,area, size, airConditioning);

        int numberOfDays = 14;

        int basePrice = 100;

        double discountForNumberOfDays = 0.05; 

        int expectedDepositPrice = (int)(numberOfDays * basePrice * (1.0 - discountForNumberOfDays));

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

    }

    [TestMethod]
    public void TestCalculateBigDepositPriceWithoutPromotionForMoreThan14DaysAndWithoutAirConditioning()
    {
        string name = "Deposito";
        char area = 'A';
        String size = "Grande";
        bool airConditioning = false;
        
        
        Deposit newDeposit = new Deposit(name,area, size, airConditioning);

        int numberOfDays = 15;

        int basePrice = 100;

        double discountForNumberOfDays = 0.1; 

        int expectedDepositPrice = (int)(numberOfDays * basePrice * (1.0 - discountForNumberOfDays));

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

    }

    [TestMethod]
    public void TestCalculateBigDepositPriceWithoutPromotionForMoreThan14DaysAndWithAirConditioning()
    {
        string name = "Deposito";
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        
        
        Deposit newDeposit = new Deposit(name,area, size, airConditioning);

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
    public void TestCalculateBigDepositPriceWithoutValidPromotionForMoreThan14DaysAndWithAirConditioning()
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
        
        string name = "Deposito";
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        
        
        Deposit newDeposit = new Deposit(name,area, size, airConditioning);
        
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
        CollectionAssert.Contains(newDeposit.Promotions, newPromotion);

    }

    [TestMethod]
    public void TestCalculateBigDepositPriceWithValidPromotionForMoreThan14DaysAndWithAirConditioning()
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
        
        string name = "Deposito";
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        
        
        Deposit newDeposit = new Deposit(name,area, size, airConditioning);
        
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
    public void TestCalculateBigDepositPriceWithTwoPromotionsOfForMoreThan14DaysAndWithAirConditioning()
    {
        double discountRate = 0.5;
        String label = "Ejemplo"; 
        DateTime dateFrom = new DateTime(2020, 4, 8).Date;
        DateTime dateTo  = new DateTime(2030, 4, 10).Date;
        DateRange dateRange = new DateRange(dateFrom, dateTo);
        
        Promotion newPromotion = new Promotion();
        Promotion newPromotion2 = new Promotion();
        
        newPromotion.DiscountRate = discountRate; 
        newPromotion.ValidityDate = dateRange;
        newPromotion.Label = label;
        
        newPromotion2.DiscountRate = discountRate; 
        newPromotion2.ValidityDate = dateRange;
        newPromotion2.Label = label;
        
        string name = "Deposito";
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        
        
        Deposit newDeposit = new Deposit(name,area, size, airConditioning);
        
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
    public void TestCalculateBigDepositPriceWithTwoPromotionsOfForMoreThan14DaysAndWithAirConditioningWithADiscountOf100()
    {
        double discountRate = 0.45;
        String label = "Ejemplo"; 
        DateTime dateFrom = new DateTime(2020, 4, 8).Date;
        DateTime dateTo  = new DateTime(2030, 4, 10).Date;
        DateRange dateRange = new DateRange(dateFrom, dateTo);
        
        Promotion newPromotion = new Promotion();
        Promotion newPromotion2 = new Promotion();
        
        newPromotion.DiscountRate = discountRate; 
        newPromotion.ValidityDate = dateRange;
        newPromotion.Label = label;
        
        newPromotion2.DiscountRate = discountRate; 
        newPromotion2.ValidityDate = dateRange;
        newPromotion2.Label = label;
        
        string name = "Deposito";
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        
        
        Deposit newDeposit = new Deposit(name,area, size, airConditioning);
        
        newDeposit.AddPromotion(newPromotion);
        newDeposit.AddPromotion(newPromotion2);

        int numberOfDays = 15;

        
        Assert.AreEqual(0, newDeposit.CalculatePrice(numberOfDays));

    }

    [TestMethod]
    public void TestRemovePromotionFromDeposit()
    {
        string name = "Deposito";
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        
        
        Deposit newDeposit1 = new Deposit(name,area, size, airConditioning);
        
        Promotion newPromotion = new Promotion();
        List<Promotion> promotionsLinkedToDeposit = new List<Promotion>();
        promotionsLinkedToDeposit.Add(newPromotion);
        
        newDeposit1.AddPromotion(newPromotion);
        
        newDeposit1.RemovePromotion(newPromotion);
        
        CollectionAssert.DoesNotContain(newDeposit1.Promotions, newPromotion);
    }
    

    [TestMethod]
    public void TestGetAverageRating()
    {
        string name = "Deposito";
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;

        Deposit newDeposit = new Deposit(name,area, size, airConditioning);
        
        Rating rating1 = new Rating(5, "Excelente");
        Rating rating2 = new Rating(3, "Regular");
        Rating rating3 = new Rating(1, "Malo");
        Rating rating4 = new Rating(4, "Bueno");
        Rating rating5 = new Rating(2, "Malo");
        
        newDeposit.AddRating(rating1);
        newDeposit.AddRating(rating2);
        newDeposit.AddRating(rating3);
        newDeposit.AddRating(rating4);
        newDeposit.AddRating(rating5);
        
        double expectedAverageRating = (5 + 3 + 1 + 4 + 2) / 5.0;
        
        Assert.AreEqual(expectedAverageRating, newDeposit.GetAverageRating());
    }
    
    [TestMethod]
    public void TestGetAverageRatingWithNoRatings()
    {
        string name = "Deposito";
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;

        Deposit newDeposit = new Deposit(name,area, size, airConditioning);
        
        Assert.AreEqual(0, newDeposit.GetAverageRating());
    }
    
    [TestMethod]
    public void TestDepositIsReserved()
    {
        string name = "Deposito";
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        DateTime dayIn = DateTime.Now;
        DateTime dayOut = new DateTime(2028, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);

        Deposit newDeposit = new Deposit(name,area, size, airConditioning);
        
        Reservation _reservation = new Reservation(newDeposit,client,stay);

        _reservation.Status = 1; 
        
        List<Reservation> listOfReservations = new List<Reservation>(); 
        listOfReservations.Add(_reservation);

        newDeposit.Reservations = listOfReservations; 
        
        Assert.IsTrue(newDeposit.IsReserved());
    }
    
    [TestMethod]
    public void TestDepositIsNotReserved()
    {
        string name = "Deposito";
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        DateTime dayIn = DateTime.Now;
        DateTime dayOut = new DateTime(2028, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);

        Deposit newDeposit = new Deposit(name,area, size, airConditioning);
        
        Reservation _reservation = new Reservation(newDeposit,client,stay);

        _reservation.Status = -1; 
        
        List<Reservation> listOfReservations = new List<Reservation>(); 
        listOfReservations.Add(_reservation);

        newDeposit.Reservations = listOfReservations; 
        
        Assert.IsFalse(newDeposit.IsReserved());
    }
    
    [TestMethod]
    public void TestDepositHasUppcomingReservations()
    {
        string name = "Deposito";
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        DateTime dayIn = DateTime.Now.AddDays(7);
        DateTime dayOut = new DateTime(2028, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);

        Deposit newDeposit = new Deposit(name,area, size, airConditioning);
        
        Reservation _reservation = new Reservation(newDeposit,client,stay);

        _reservation.Status = 1; 
        
        List<Reservation> listOfReservations = new List<Reservation>(); 
        listOfReservations.Add(_reservation);

        newDeposit.Reservations = listOfReservations; 
        
        Assert.IsTrue(newDeposit.HasUpcomingReservations());
    }
    
    [TestMethod]
    public void TestDepositHasNotUppcomingReservations()
    {
        string name = "Deposito";
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        DateTime dayIn = new DateTime(2020, 04, 08);
        DateTime dayOut = new DateTime(2028, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);

        Deposit newDeposit = new Deposit(name,area, size, airConditioning);
        
        Reservation _reservation = new Reservation(newDeposit,client,stay);

        _reservation.Status = 1; 
        
        List<Reservation> listOfReservations = new List<Reservation>(); 
        listOfReservations.Add(_reservation);

        newDeposit.Reservations = listOfReservations; 
        
        Assert.IsFalse(newDeposit.HasUpcomingReservations());
    }

    
    
    



}