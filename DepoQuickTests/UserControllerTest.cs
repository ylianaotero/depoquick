using BusinessLogic;
using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;
using DepoQuick.Exceptions.UserExceptions;

namespace DepoQuickTests;

[TestClass]
public class UserControllerTest
{
    private UserController _userController;
    private Session _session;
    private const string AdminName = "Administrator";
    private const string AdminEmail = "administrator@domain.com";
    private const string AdminPassword = "Password1#";
    private const string ClientName = "Client";
    private const string ClientEmail = "client@domain.com";
    private const string ClientPassword = "Password2#";
    
    private const string UserLogInLogMessage = "Ingresó al sistema";

    [TestInitialize]
    public void Initialize()
    {
        var context = TestContextFactory.CreateContext();
        _userController = new UserController(context);
        _session = new Session(_userController);
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
    [ExpectedException(typeof(AdministratorAlreadyExistsException))]
    public void TestAdministratorAlreadyExistsException()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
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
    [ExpectedException(typeof(EmptyAdministratorException))]
    public void TestCannotGetAdministrator()
    {
        _userController.GetAdministrator();
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotGetLogs()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        _session.LoginUser(ClientEmail,ClientPassword);
        _session.LogoutUser();
        _session.LoginUser(ClientEmail,ClientPassword);
        _userController.GetLogs(_userController.Get(ClientEmail), _session.ActiveUser);
    }
    
    [TestMethod]
    public void TestGetLogs()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        DateTime now = DateTime.Now.AddSeconds(-DateTime.Now.Second);
        _session.LoginUser(AdminEmail,AdminPassword);
        _session.LogoutUser();
        _session.LoginUser(AdminEmail,AdminPassword);
        List<LogEntry> logs = _userController.GetLogs(_userController.Get(AdminEmail), _session.ActiveUser);
        
        Assert.AreEqual(3,logs.Count);
        Assert.AreEqual(logs[0].Message , UserLogInLogMessage);
        Assert.IsTrue(logs.Any(log => now.Date == log.Timestamp.Date
                              && now.Hour == log.Timestamp.Hour && now.Minute == log.Timestamp.Minute));
        Assert.AreEqual(logs[0].UserId , _userController.Get(AdminEmail).Id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(EmptyActionLogException))]
    public void TestEmptyActionLog()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _session.LoginUser(AdminEmail,AdminPassword);
        _session.LogoutUser();
        _session.LoginUser(AdminEmail,AdminPassword);
        _userController.LogAction(_userController.Get(AdminEmail),"",DateTime.Now);
    }

}
