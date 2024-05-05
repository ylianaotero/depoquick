using DepoQuick.Domain;
using DepoQuick.Domain.Exceptions.DateRangeExceptions;
using DepoQuick.Domain.Exceptions.ReservationExceptions;

namespace DepoQuickTests;

[TestClass]
public class ReservationTest
{
    
    [TestMethod]
    [ExpectedException(typeof(InvalidDateRangeException))] 
    public void TestInvalidDateReservation()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true);
        DateTime dayIn = new DateTime(2025, 04, 09);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
    }
    
    [TestMethod]
    public void TestValidClient()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit,client,stay);

        Client expectedClient = new Client("Mario", "mario@gmail.com", "maRio.68");
        reservation.SetClient(expectedClient);
        Client actualClient = reservation.GetClient();
        
        Assert.AreEqual(expectedClient,actualClient);
    }

    [TestMethod]
    public void TestValidDeposit()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit,client,stay);

        Deposit expectedDeposit = new Deposit('A', "Grande", false);
        reservation.SetDeposit(expectedDeposit);
        Deposit actualDeposit = reservation.GetDeposit();
        
        Assert.AreEqual(expectedDeposit,actualDeposit);
    }

    [TestMethod]
    public void TestValidState()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit,client,stay);

        int expectedState = 1;
        reservation.SetState(expectedState);
        int actualState = reservation.GetState();
        
        Assert.AreEqual(expectedState,actualState);
    }

    [TestMethod]
    public void TestValidDateRange()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit,client,stay);
        
        DateTime newDayIn = new DateTime(2024, 04, 07);
        DateTime newDayOut = new DateTime(2024, 04, 08);
        DateRange newStay = new DateRange(newDayIn, newDayOut);

        reservation.SetDateRange(newStay);
        DateRange actualStay = reservation.GetDateRange();
        
        Assert.AreEqual(newStay,actualStay);

    }
    
    [TestMethod]
    public void TestValidMessage()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit,client,stay);

        String expectedMessage = "Se rechaza la reserva por el alto costo";
        reservation.SetMessage(expectedMessage);
        String actualMessage = reservation.GetMessage();
        
        Assert.AreEqual(expectedMessage,actualMessage);
    }

    [TestMethod]
    [ExpectedException(typeof(ReservationMessageHasMoreThan300CharactersException))]
    public void TestMessageWithMoreThan300Characters()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit,client,stay);

        String expectedMessage = "El precio no es el factor determinante para mí en esta reserva. Estoy más interesado en la calidad del servicio y las comodidades que se ofrecen. Valoraré la experiencia en general por encima de cualquier consideración monetaria. Quiero asegurarme de que mi estadía sea placentera y satisfactoria en todos los aspectos, independientemente del costo.";
        reservation.SetMessage(expectedMessage);
    }

    [TestMethod]
    [ExpectedException(typeof(ReservationWithEmptyMessageException))]
    public void TestEmptyMessage()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit,client,stay);

        String expectedMessage = " ";
        reservation.SetMessage(expectedMessage);
    }
    
     [TestMethod]
    public void TestValidReservation()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit, client, stay);
        Assert.AreEqual(client,reservation.GetClient());
        Assert.AreEqual(deposit,reservation.GetDeposit());
        Assert.AreEqual(stay,reservation.GetDateRange());
    }
    
    [TestMethod]
    public void TestAddIdToAValidDeposit()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        
        Reservation reservation = new Reservation(deposit, client, stay);

        int id = 2; 

        reservation.SetId(id); 
        
        Assert.AreEqual(id, reservation.GetId());
    }
    
    [TestMethod]
    public void TestTwoDepositsHaveDifferentIDs()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        
        Reservation reservation1 = new Reservation(deposit, client, stay);
        Reservation reservation2 = new Reservation(deposit, client, stay);

        Assert.AreNotEqual(reservation1.GetId(), reservation2.GetId());
    }

    [TestMethod]
    public void TestIDIsIncremental()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        
        Reservation reservation1 = new Reservation(deposit, client, stay);
        Reservation reservation2 = new Reservation(deposit, client, stay);

        Assert.IsTrue(reservation1.GetId() < reservation2.GetId());
    }
    
    //testear resrva vacia
}