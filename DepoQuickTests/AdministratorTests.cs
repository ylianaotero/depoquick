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
    
    
}