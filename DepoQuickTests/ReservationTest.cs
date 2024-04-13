using DepoQuick.Domain;
using DepoQuick.Domain.Exceptions.DateRangeExceptions;

namespace DepoQuickTests;

[TestClass]
public class ReservationTest
{
    
    [TestMethod]
    public void TestValidReservation()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true, false);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit, client, stay);
        Assert.AreEqual(client,reservation.getClient());
        Assert.AreEqual(deposit,reservation.getDeposit());
        Assert.AreEqual(stay,reservation.getDateRange());
    }
    
    
    [TestMethod]
    [ExpectedException(typeof(InvalidDateRangeException))] 
    public void TestInvalidDateReservation()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true, false);
        DateTime dayIn = new DateTime(2025, 04, 09);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit, client, stay);
    }
}