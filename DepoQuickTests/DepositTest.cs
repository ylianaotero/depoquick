using DepoQuick.Domain;
using DepoQuick.Exceptions.DepositExceptions;

namespace DepoQuickTests;

[TestClass]
public class DepositTest
{
    [TestMethod]
    public void TestDepositConstructor()
    {
        char area = 'A';
        String size = "Pequeño";
        bool airConditioning = true;
        
        Deposit newDeposit = new Deposit(); 
        newDeposit.Area = area;
        newDeposit.Size = size;
        newDeposit.AirConditioning = airConditioning;
        
        Assert.IsNotNull(newDeposit);
        Assert.AreEqual(char.ToUpper(area), newDeposit.Area);
        Assert.AreEqual(size.ToUpper(), newDeposit.Size);
        Assert.AreEqual(airConditioning, newDeposit.AirConditioning);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositWithInvalidAreaException))] 
    public void TestInvalidArea()
    {
        char area = 'R';
        String size = "Grande"; 
        bool airConditioning = true;
        

        new Deposit(area, size, airConditioning);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositWithInvalidAreaException))] 
    public void TestInvalidSizeAndArea()
    {
        char area = 'l';
        String size = "Enorme";
        bool airConditioning = true;
        
        
        new Deposit(area, size, airConditioning);
    }
    
    [TestMethod]
    public void TestValidDeposit()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning); 
        
        Assert.IsNotNull(newDeposit);
        Assert.AreEqual(char.ToUpper(area), newDeposit.Area);
        Assert.AreEqual(size.ToUpper(), newDeposit.Size);
        Assert.AreEqual(airConditioning, newDeposit.AirConditioning);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DepositWithInvalidSizeException))] 
    public void TestInvalidSize()
    {
        char area = 'A';
        String size = "Minusculo";
        bool airConditioning = true;
        
        
        new Deposit(area, size, airConditioning);
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
        
        
        Deposit newDeposit = new Deposit(area, size, airConditioning); 
        
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
        
        char area = 'A';
        String size = "Pequeño";
        bool airConditioning = true;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning); 
        
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
        char area = 'A';
        String size = "Pequeño";
        bool airConditioning = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning);

        int numberOfDays = 6;

        int basePrice = 50;

        int expectedDepositPrice = numberOfDays * basePrice; 
        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

    }

    [TestMethod]
    public void TestCalculateMediumDepositPriceWithoutPromotionLessThan7DaysAndWithoutAirConditioning()
    {
        char area = 'A';
        String size = "Mediano";
        bool airConditioning = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning);

        int numberOfDays = 6;

        int basePrice = 75;

        int expectedDepositPrice = numberOfDays * basePrice; 
        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

    }

    [TestMethod]
    public void TestCalculateBigDepositPriceWithoutPromotionLessThan7DaysAndWithoutAirConditioning()
    {
        char area = 'A';
        String size = "Grande";
        bool airConditioning = false;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning);

        int numberOfDays = 6;

        int basePrice = 100;

        int expectedDepositPrice = numberOfDays * basePrice; 
        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

    }

    [TestMethod]
    public void TestCalculateBigDepositPriceWithoutPromotionFor7DaysAndWithoutAirConditioning()
    {
        char area = 'A';
        String size = "Grande";
        bool airConditioning = false;
        
        
        Deposit newDeposit = new Deposit(area, size, airConditioning);

        int numberOfDays = 7;

        int basePrice = 100;

        double discountForNumberOfDays = 0.05; 

        int expectedDepositPrice = (int)(numberOfDays * basePrice * (1.0 - discountForNumberOfDays));

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

    }

    [TestMethod]
    public void TestCalculateBigDepositPriceWithoutPromotionFor14DaysAndWithoutAirConditioning()
    {
        char area = 'A';
        String size = "Grande";
        bool airConditioning = false;
        
        
        Deposit newDeposit = new Deposit(area, size, airConditioning);

        int numberOfDays = 14;

        int basePrice = 100;

        double discountForNumberOfDays = 0.05; 

        int expectedDepositPrice = (int)(numberOfDays * basePrice * (1.0 - discountForNumberOfDays));

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

    }

    [TestMethod]
    public void TestCalculateBigDepositPriceWithoutPromotionForMoreThan14DaysAndWithoutAirConditioning()
    {
        char area = 'A';
        String size = "Grande";
        bool airConditioning = false;
        
        
        Deposit newDeposit = new Deposit(area, size, airConditioning);

        int numberOfDays = 15;

        int basePrice = 100;

        double discountForNumberOfDays = 0.1; 

        int expectedDepositPrice = (int)(numberOfDays * basePrice * (1.0 - discountForNumberOfDays));

        
        Assert.AreEqual(expectedDepositPrice, newDeposit.CalculatePrice(numberOfDays));

    }

    [TestMethod]
    public void TestCalculateBigDepositPriceWithoutPromotionForMoreThan14DaysAndWithAirConditioning()
    {
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        
        
        Deposit newDeposit = new Deposit(area, size, airConditioning);

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
        
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        
        
        Deposit newDeposit = new Deposit(area, size, airConditioning);
        
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
        
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        
        
        Deposit newDeposit = new Deposit(area, size, airConditioning);
        
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
        
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        
        
        Deposit newDeposit = new Deposit(area, size, airConditioning);
        
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
        
        char area = 'A';
        String size = "Grande";
        bool airConditioning = true;
        
        
        Deposit newDeposit = new Deposit(area, size, airConditioning);
        
        newDeposit.AddPromotion(newPromotion);
        newDeposit.AddPromotion(newPromotion2);

        int numberOfDays = 15;

        
        Assert.AreEqual(0, newDeposit.CalculatePrice(numberOfDays));

    }
    
   /* [TestMethod]
    public void TestTwoDepositsHaveDifferentIDs()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        
        
        Deposit newDeposit1 = new Deposit(area, size, airConditioning);
        Deposit newDeposit2 = new Deposit(area, size, airConditioning); 

        Assert.AreNotEqual(newDeposit1.Id, newDeposit2.Id);
    }

    [TestMethod]
    public void TestIDIsIncremental()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        
        
        Deposit newDeposit1 = new Deposit(area, size, airConditioning);
        Deposit newDeposit2 = new Deposit(area, size, airConditioning);

        Assert.IsTrue(newDeposit1.Id < newDeposit2.Id);
    }*/

    [TestMethod]
    public void TestRemovePromotionFromDeposit()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        
        
        Deposit newDeposit1 = new Deposit(area, size, airConditioning);
        
        Promotion newPromotion = new Promotion();
        List<Promotion> promotionsLinkedToDeposit = new List<Promotion>();
        promotionsLinkedToDeposit.Add(newPromotion);
        
        newDeposit1.AddPromotion(newPromotion);
        
        newDeposit1.RemovePromotion(newPromotion);
        
        CollectionAssert.DoesNotContain(newDeposit1.Promotions, newPromotion);
    }

    [TestMethod]
    public void TestGetReservations()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        
        
        Deposit newDeposit = new Deposit(area, size, airConditioning);

        Client client = new Client("Maria Perez", "mariaperez@gmail.com", "Contrasena1#");
        
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        
        Reservation reservation = new Reservation(newDeposit, client, stay);
        
        newDeposit.AddReservation(reservation);
        
        CollectionAssert.Contains(newDeposit.Reservations, reservation);
    }


    [TestMethod]
    public void TestRemoveReservation()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        

        Deposit newDeposit = new Deposit(area, size, airConditioning);

        Client client = new Client("Maria Perez", "mariaperez@gmail.com", "Contrasena1#");

        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);

        Reservation reservation = new Reservation(newDeposit, client, stay);

        newDeposit.AddReservation(reservation);

        newDeposit.RemoveReservation(reservation);

        CollectionAssert.DoesNotContain(newDeposit.Reservations, reservation);
    }

    [TestMethod]
    public void TestDepositIsReserved()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        

        Deposit newDeposit = new Deposit(area, size, airConditioning);

        Administrator admin = new Administrator("Juan Perez", "juanperez@gmail.com", "Contrasena1#");

        Client client = new Client("Maria Perez", "mariaperez@gmail.com", "Contrasena1#");

        DateTime expiredDayIn = new DateTime(2024, 04, 07);
        DateTime expiredDayOut = new DateTime(2024, 04, 08);
        DateRange expiredStay = new DateRange(expiredDayIn, expiredDayOut);

        Reservation expiredReservation = new Reservation(newDeposit, client, expiredStay);

        newDeposit.AddReservation(expiredReservation);
        
        Assert.IsFalse(newDeposit.IsReserved());
        
        DateTime dayIn = DateTime.Now;
        DateTime dayOut = DateTime.Now.AddDays(10);
        DateRange stay = new DateRange(dayIn, dayOut);
        
        Reservation reservation = new Reservation(newDeposit, client, stay);
        
        newDeposit.AddReservation(reservation);
        
        admin.ApproveReservation(reservation);
        
        Assert.IsTrue(newDeposit.IsReserved());
    }

    [TestMethod]
    public void TestDepositIsReservedInDateRange()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;
        
        Deposit newDeposit = new Deposit(area, size, airConditioning);

        Client client = new Client("Maria Perez", "mariaperez@gmail.com", "Contrasena1#");

        DateTime dayIn = DateTime.Now.AddDays(10);
        DateTime dayOut = DateTime.Now.AddDays(20);
        DateRange stay = new DateRange(dayIn, dayOut);

        Reservation reservation = new Reservation(newDeposit, client, stay);

        newDeposit.AddReservation(reservation);

        Administrator admin = new Administrator("Juan Perez", "juanperez@gmail.com", "Contrasena1#");

        admin.ApproveReservation(reservation);
        Assert.IsFalse(newDeposit.IsReserved());
        Assert.IsTrue(newDeposit.IsReserved(stay));
    }

    [TestMethod]
    public void TestDepositIsNotReservedInDateRange()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;

        Deposit newDeposit = new Deposit(area, size, airConditioning);

        Client client = new Client("Maria Perez", "mariaperez@gmail.com", "Contrasena1#");

        DateTime dayIn = DateTime.Now.AddDays(10);
        DateTime dayOut = DateTime.Now.AddDays(20);
        DateRange stay = new DateRange(dayIn, dayOut);
        
        DateRange otherStay = new DateRange(DateTime.Now.AddDays(5), DateTime.Now.AddDays(8));

        Reservation reservation = new Reservation(newDeposit, client, stay);

        newDeposit.AddReservation(reservation);
        
        Administrator admin = new Administrator("Juan Perez", "juanperez@gmail.com", "Contrasena1#");
        admin.ApproveReservation(reservation);

        Assert.IsFalse(newDeposit.IsReserved(otherStay));
    }

    [TestMethod]
    public void TestDepositHasUpcomingReservations()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;

        Deposit newDeposit = new Deposit(area, size, airConditioning);

        Client client = new Client("Maria Perez", "mariaperez@gmail.com", "Contrasena1#");
        
        DateRange stay = new DateRange(DateTime.Now.AddDays(5), DateTime.Now.AddDays(8));
        
        Reservation reservation = new Reservation(newDeposit, client, stay);

        newDeposit.AddReservation(reservation);
        
        Assert.IsTrue(newDeposit.HasUpcomingReservations());
    }

    [TestMethod]
    public void TestDepositDoesNotHaveUpcomingReservations()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;

        Deposit newDeposit = new Deposit(area, size, airConditioning);

        Client client = new Client("Maria Perez", "mariaperez@gmail.com", "Contrasena1#");

        DateTime expiredDayIn = new DateTime(2024, 04, 07);
        DateTime expiredDayOut = new DateTime(2024, 04, 08);
        DateRange expiredStay = new DateRange(expiredDayIn, expiredDayOut);

        Reservation expiredReservation = new Reservation(newDeposit, client, expiredStay);

        newDeposit.AddReservation(expiredReservation);

        Assert.IsFalse(newDeposit.HasUpcomingReservations());
    }

    [TestMethod]
    public void TestGetAverageRating()
    {
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;

        Deposit newDeposit = new Deposit(area, size, airConditioning);
        
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
        char area = 'a';
        String size = "Mediano";
        bool airConditioning = true;

        Deposit newDeposit = new Deposit(area, size, airConditioning);
        
        Assert.AreEqual(0, newDeposit.GetAverageRating());
    }

}