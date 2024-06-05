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
        
        _deposit = new Deposit("Deposito",'A', "Mediano", true);
        
        
        DateRange dateRange = new DateRange(DateTime.Now, DateTime.Now.AddDays(5));

        _reservation = new Reservation(_deposit, _client, dateRange);
        
        _client.AddReservation(_reservation);

    }
    
    [TestMethod]
    public void TestValidAdministratorIsCreated()
    {
        Administrator user = new Administrator(_adminName, _adminEmail, _adminPassword);
        Assert.AreEqual(_adminName, user.Name);
        Assert.AreEqual(_adminEmail, user.Email);
        Assert.AreEqual(_adminPassword, user.Password);
    }
    
    [TestMethod]
    public void TestAdministratorCanApproveReservations()
    {
        Assert.IsTrue(_reservation.Status == 0);
        
        _admin.ApproveReservation(_reservation);

        Assert.IsTrue(_reservation.Status == 1 && _deposit.IsReserved());
    }

    [TestMethod]
    public void TestAdministratorCanRejectReservations()
    {
        Assert.IsTrue(_reservation.Status == 0 && _reservation.Message == "-");

        string reason = "No hay disponibilidad.";

        _admin.RejectReservation(_reservation, reason);

        Assert.IsTrue(_reservation.Status == -1 && _reservation.Message == reason && !_deposit.IsReserved());
    }

    [TestMethod]
    public void TestAdministratorUserIsAdministrator()
    {
        Assert.IsTrue(_admin.IsAdministrator);
    }
}