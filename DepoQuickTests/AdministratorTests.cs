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
        
        _deposit = new Deposit('A', "Mediano", true);
        
        
        DateRange dateRange = new DateRange(DateTime.Now, DateTime.Now.AddDays(5));

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
        Assert.IsTrue(_reservation.GetState() == 0);
        
        _admin.ApproveReservation(_reservation);

        Assert.IsTrue(_reservation.GetState() == 1 && _deposit.IsReserved());
    }

    [TestMethod]
    public void TestAdministratorCanRejectReservations()
    {
        Assert.IsTrue(_reservation.GetState() == 0 && _reservation.GetMessage() == "");

        string reason = "No hay disponibilidad.";

        _admin.RejectReservation(_reservation, reason);

        Assert.IsTrue(_reservation.GetState() == -1 && _reservation.GetMessage() == reason && !_deposit.IsReserved());
    }

    [TestMethod]
    public void TestAdministratorUserIsAdministrator()
    {
        Assert.IsTrue(_admin.IsAdministrator());
    }
}