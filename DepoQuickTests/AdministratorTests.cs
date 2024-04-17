using DepoQuick.Domain;

namespace DepoQuickTests;

[TestClass]
public class AdministratorTests
{
    private Administrator _admin;
    private string _name = "Gabriela Pereira";
    private string _email = "gpereira@depoquick.com";
    private string _password = "123GabrielaP#";
    
    [TestMethod]
    public void TestValidAdministratorIsCreated()
    {
        Administrator user = new Administrator(_name, _email, _password);
        Assert.AreEqual(_name, user.GetName());
        Assert.AreEqual(_email, user.GetEmail());
        Assert.AreEqual(_password, user.GetPassword());
    }
    
    [TestMethod]
    public void TestAdministratorCanApproveReservations()
    {
        Administrator admin = new Administrator(_name, _email, _password);
        
        Client client = new Client("Juan Perez", "juanperez@gmail.com", "JuanP123!Zerep");
        Deposit deposit = new Deposit('A', "Mediano", true, false);
        DateRange dateRange = new DateRange(DateTime.Now.AddDays(2), DateTime.Now.AddDays(5));
        
        Reservation reservation = new Reservation(deposit, client, dateRange);
        
        Assert.IsTrue(reservation.GetSate() == 0);
        
        admin.ApproveReservation(reservation);
        
        Assert.IsTrue(reservation.GetSate() == 1);
    }
}