using DepoQuick.Domain;

namespace DepoQuickTests;

[TestClass]
public class ClientTests
{
    private string _name = "Juan Perez";
    private string _email = "nombre@dominio.es";
    private string _password = "Contrasena#1";
    
    [TestMethod]
    public void TestValidClientIsCreated()
    {
        Client client = new Client(_name, _email, _password);
        Assert.AreEqual(_name, client.GetName());
        Assert.AreEqual(_email, client.GetEmail());
        Assert.AreEqual(_password, client.GetPassword());
    }
    
    [TestMethod]
    public void TestNewClientHasNoReservations()
    {
        Client client = new Client(_name, _email, _password);
        Assert.AreEqual(0, client.GetReservations().Count);
    }
    
    
}