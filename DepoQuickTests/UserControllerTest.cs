using BusinessLogic;
using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;

namespace DepoQuickTests;

[TestClass]
public class UserControllerTest
{
    private UserController _userController;
    private const string AdminName = "Administrator";
    private const string AdminEmail = "administrator@domain.com";
    private const string AdminPassword = "Password1#";
    private const string ClientName = "Client";
    private const string ClientEmail = "client@domain.com";
    private const string ClientPassword = "Password2#";

    [TestInitialize]
    public void Initialize()
    {
        var context = TestContextFactory.CreateContext();
        _userController = new UserController(context);
    }
    
    [TestMethod]
    public void TestRegisterAdministrator()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        Administrator result = (Administrator)_userController.Get(AdminEmail);
        Assert.IsNotNull(result);
        Assert.AreEqual(result.Email,AdminEmail);
    }
    
     [TestMethod]
    public void TestRegisterClient()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        Client result = (Client)_userController.Get(ClientEmail);
        Assert.IsNotNull(result);
        Assert.AreEqual(result.Email,ClientEmail);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotCreateClientBeforeAdminException))]
    public void TestCannotCreateClientBeforeAdministrator()
    {
        _userController.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
    }
    
    [TestMethod]
    [ExpectedException(typeof(UserAlreadyExistsException))]
    public void TestEmailIsAlreadyInUse()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterClient(ClientName,AdminEmail,ClientPassword,ClientPassword);
    }
    
    [TestMethod]
    public void TestGetUser()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);

        User newUser = _userController.Get(ClientEmail);
        int id = newUser.Id;
        
        _userController.Get(id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(UserDoesNotExistException))]
    public void TestCannotFindUserById()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);

        User newUser = _userController.Get(AdminEmail);
        int id = newUser.Id;

        _userController.Remove(id);
        _userController.Get(id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(UserDoesNotExistException))]
    public void TestCannotFindUserByEmail()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);

        User newUser = _userController.Get(AdminEmail);
        int id = newUser.Id;

        _userController.Remove(id);
        _userController.Get(AdminEmail);
    }

    [TestMethod]
    public void TestRemoveUser()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);

        User newUser = _userController.Get(ClientEmail);
        int id = newUser.Id;

        _userController.Remove(id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(UserDoesNotExistException))]
    public void TestCannotRemoveUser()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);

        User newUser = _userController.Get(ClientEmail);
        int id = newUser.Id;

        _userController.Remove(id);
        _userController.Remove(id);
    }
    
    [TestMethod]
    public void TestGetAdministrator()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        Administrator admin = _userController.GetAdministrator();
        
        Assert.IsNotNull(admin);
        Assert.AreEqual(AdminName, admin.Name);
        Assert.AreEqual(AdminEmail, admin.Email);
    }
    
    [TestMethod]
    [ExpectedException(typeof(UserDoesNotExistException))]
    public void TestCannotGetAdministrator()
    {
        _userController.GetAdministrator();
    }
}
