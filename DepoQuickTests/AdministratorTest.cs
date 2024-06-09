using DepoQuick.Domain;

namespace DepoQuickTests;

[TestClass]
public class AdministratorTest
{
    private Administrator _admin;
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
    public void TestAdministratorUserIsAdministrator()
    {
        Assert.IsTrue(_admin.IsAdministrator);
    }
}