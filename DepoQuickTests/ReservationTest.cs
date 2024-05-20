using DepoQuick.Domain;
using DepoQuick.Exceptions.DateRangeExceptions;
using DepoQuick.Exceptions.ReservationExceptions;

namespace DepoQuickTests;

[TestClass]
public class ReservationTest
{
    private const string ClientName1 = "Maria Perez";
    private const string ClientEmail1 = "maria@gmail.com";
    private const string ClientPassword1 = "Mariaaa1.";
    private const string ClientName2 = "Mario S";
    private const string ClientEmail2 = "mario@gmail.com";
    private const string ClientPassword2 = "maRio.68";
    private const char DepositArea1 = 'A';
    private const string DepositSize1 = "Pequeño";
    private const string DepositSize2 = "Mediano";
    private const string DepositSize3 = "Grande";
    private const bool DepositAirConditioning1 = true;
    private const bool DepositAirConditioning2 = false;
    
    [TestMethod]
    public void TestCreateReservationAndSetProperties()
    {
        Client client = new Client(ClientName1, ClientEmail1, ClientPassword1);
        Deposit deposit = new Deposit(DepositArea1, DepositSize1, DepositAirConditioning1);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        
        Reservation reservation = new Reservation();
        reservation.Client = client;
        reservation.Deposit = deposit;
        reservation.Date = stay;
        
        Assert.AreEqual(client, reservation.Client);
        Assert.AreEqual(deposit, reservation.Deposit);
        Assert.AreEqual(stay, reservation.Date);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidDateRangeException))] 
    public void TestInvalidDateReservation()
    {
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        Deposit deposit = new Deposit(DepositArea1,DepositSize1,DepositAirConditioning1);
        DateTime dayIn = new DateTime(2025, 04, 09);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
    }
    
    [TestMethod]
    public void TestValidClient()
    {
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        Deposit deposit = new Deposit(DepositArea1,DepositSize1,DepositAirConditioning1);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit,client,stay);

        Client expectedClient = new Client(ClientName2, ClientEmail2, ClientPassword2);
        reservation.Client = expectedClient;
        Client actualClient = reservation.Client;
        
        Assert.AreEqual(expectedClient,actualClient);
    }

    [TestMethod]
    public void TestValidDeposit()
    {
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        Deposit deposit = new Deposit(DepositArea1, DepositSize1, DepositAirConditioning1);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit,client,stay);

        Deposit expectedDeposit = new Deposit(DepositArea1, DepositSize3, DepositAirConditioning2);
        reservation.Deposit = expectedDeposit;
        Deposit actualDeposit = reservation.Deposit;
        
        Assert.AreEqual(expectedDeposit,actualDeposit);
    }

    [TestMethod]
    public void TestValidStatus()
    {
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        Deposit deposit = new Deposit(DepositArea1, DepositSize1, DepositAirConditioning1);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit,client,stay);

        int expectedStatus = 1;
        reservation.Status = expectedStatus;
        int actualStatus = reservation.Status;
        
        Assert.AreEqual(expectedStatus,actualStatus);
    }

    [TestMethod]
    public void TestValidDateRange()
    {
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        Deposit deposit = new Deposit(DepositArea1, DepositSize1, DepositAirConditioning1);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit,client,stay);
        
        DateTime newDayIn = new DateTime(2024, 04, 07);
        DateTime newDayOut = new DateTime(2024, 04, 08);
        DateRange newStay = new DateRange(newDayIn, newDayOut);

        reservation.Date = newStay;
        DateRange actualStay = reservation.Date;
        
        Assert.AreEqual(newStay,actualStay);

    }
    
    [TestMethod]
    public void TestValidMessage()
    {
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        Deposit deposit = new Deposit(DepositArea1, DepositSize1, DepositAirConditioning1);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit,client,stay);

        String expectedMessage = "Se rechaza la reserva por el alto costo";
        reservation.Message = expectedMessage;
        String actualMessage = reservation.Message;
        
        Assert.AreEqual(expectedMessage,actualMessage);
    }

    [TestMethod]
    [ExpectedException(typeof(ReservationMessageHasMoreThan300CharactersException))]
    public void TestMessageWithMoreThan300Characters()
    {
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        Deposit deposit = new Deposit(DepositArea1, DepositSize1, DepositAirConditioning1);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit,client,stay);

        String expectedMessage = "El precio no es el factor determinante para mí en esta reserva. Estoy más interesado en la calidad del servicio y las comodidades que se ofrecen. Valoraré la experiencia en general por encima de cualquier consideración monetaria. Quiero asegurarme de que mi estadía sea placentera y satisfactoria en todos los aspectos, independientemente del costo.";
        reservation.Message = expectedMessage;
    }

    [TestMethod]
    [ExpectedException(typeof(ReservationWithEmptyMessageException))]
    public void TestEmptyMessage()
    {
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        Deposit deposit = new Deposit(DepositArea1, DepositSize1, DepositAirConditioning1);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit,client,stay);

        String expectedMessage = " ";
        reservation.Message = expectedMessage;
    }
    
     [TestMethod]
    public void TestValidReservation()
    {
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        Deposit deposit = new Deposit(DepositArea1, DepositSize1, DepositAirConditioning1);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit, client, stay);
        Assert.AreEqual(client,reservation.Client);
        Assert.AreEqual(deposit,reservation.Deposit);
        Assert.AreEqual(stay,reservation.Date);
    }
    
    [TestMethod]
    public void TestTwoDepositsHaveDifferentIDs()
    {
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        Deposit deposit = new Deposit(DepositArea1, DepositSize1, DepositAirConditioning1);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        
        Reservation reservation1 = new Reservation(deposit, client, stay);
        Reservation reservation2 = new Reservation(deposit, client, stay);

        Assert.AreNotEqual(reservation1.Id, reservation2.Id);
    }

    [TestMethod]
    public void TestIDIsIncremental()
    {
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        Deposit deposit = new Deposit(DepositArea1, DepositSize1, DepositAirConditioning1);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        
        Reservation reservation1 = new Reservation(deposit, client, stay);
        Reservation reservation2 = new Reservation(deposit, client, stay);

        Assert.IsTrue(reservation1.Id < reservation2.Id);
    }
}