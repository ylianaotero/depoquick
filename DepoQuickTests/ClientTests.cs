using DepoQuick.Domain;

namespace DepoQuickTests;

[TestClass]
public class ClientTests
{
    private string _name = "Juan Perez";
    private string _email = "nombre@dominio.es";
    private string _password = "Contrasena#1";
    
    private Deposit deposit = new Deposit("Deposit",'A', "Pequeño", true);
    private DateRange stay = new DateRange(new DateTime(2024, 04, 07), new DateTime(2024, 04, 08));
    
    [TestMethod]
    public void TestValidClientIsCreated()
    {
        Client client = new Client(_name, _email, _password);
        Assert.AreEqual(_name, client.Name);
        Assert.AreEqual(_email, client.Email);
        Assert.AreEqual(_password, client.Password);
    }
    
    [TestMethod]
    public void TestNewClientHasNoReservations()
    {
        Client client = new Client(_name, _email, _password);
        Assert.AreEqual(0, client.Reservations.Count);
    }
    
    [TestMethod]
    public void TestAddAReservationToTheClient()
    {
        Client client = new Client(_name, _email, _password);
        
        Reservation reservation = new Reservation(deposit, client, stay);

        client.AddReservation(reservation); 
        
        CollectionAssert.Contains(client.Reservations, reservation);
    }

    [TestMethod]
    public void TestClientUserIsNotAdministrator()
    {
        Client client = new Client(_name, _email, _password);
        Assert.IsFalse(client.IsAdministrator);
    }
    
    
}