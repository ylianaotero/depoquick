using DepoQuick.Domain;

namespace DepoQuickTests;

[TestClass]
public class AdministratorTests
{
    private Administrator _admin;
    private Client _client;
    private Deposit _deposit;
    private Reservation _reservation;
    
    private string _adminName = "Gabriela Pereira";
    private string _adminEmail = "gpereira@depoquick.com";
    private string _adminPassword = "123GabrielaP#";
    
    private string _clientName = "Juan Perez";
    private string _clientEmail = "jperez@depoquick.com";
    private string _clientPassword = "321PerezJuan$";
    
    
    [TestInitialize]
    public void TestInitialize()
    {
        _admin = new Administrator(_adminName, _adminEmail, _adminPassword);
        _client = new Client(_clientName, _clientEmail, _clientPassword);
        
        _deposit = new Deposit('A', "Mediano", true, false);
        
        DateRange dateRange = new DateRange(DateTime.Now.AddDays(2), DateTime.Now.AddDays(5));

        _reservation = new Reservation(_deposit, _client, dateRange);

    }
    
    
    
    
    [TestMethod]
    public void TestValidAdministratorIsCreated()
    {
        Administrator user = new Administrator(_adminName, _adminEmail, _adminPassword);
        Assert.AreEqual(_adminName, user.GetName());
        Assert.AreEqual(_adminEmail, user.GetEmail());
        Assert.AreEqual(_adminPassword, user.GetPassword());
    }
    
    [TestMethod]
    public void TestAdministratorCanApproveReservations()
    {
        Administrator admin = new Administrator(_adminName, _adminEmail, _adminPassword);
        
        Client client = new Client("Juan Perez", "juanperez@gmail.com", _adminPassword);
        Deposit deposit = new Deposit('A', "Mediano", true, false);
        DateRange dateRange = new DateRange(DateTime.Now.AddDays(2), DateTime.Now.AddDays(5));
        
        Reservation reservation = new Reservation(deposit, client, dateRange);
        
        Assert.IsTrue(reservation.GetSate() == 0);
        
        admin.ApproveReservation(reservation);
        
        Assert.IsTrue(reservation.GetSate() == 1);
    }

    [TestMethod]
    public void TestAdministratorCanRejectReservations()
    {
        Assert.IsTrue(_reservation.GetSate() == 0 && _reservation.GetMessage() == "");

        string reason = "No hay disponibilidad.";

        _admin.RejectReservation(_reservation, reason);

        Assert.IsTrue(_reservation.GetSate() == -1 && _reservation.GetMessage() == reason);
    }
}