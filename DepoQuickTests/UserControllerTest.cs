using BusinessLogic;
using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;
using DepoQuick.Exceptions.UserExceptions;

namespace DepoQuickTests;

[TestClass]
public class UserControllerDeprecatedTest
{
    private UserControllerDeprecated _userControllerDeprecated;
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
        _userControllerDeprecated = new UserControllerDeprecated(context);
        _session = new Session(_userControllerDeprecated);
    }
    
    [TestMethod]
    public void TestRegisterAdministrator()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        Administrator result = (Administrator)_userControllerDeprecated.Get(AdminEmail);
        Assert.IsNotNull(result);
        Assert.AreEqual(result.Email,AdminEmail);
    }
    
    [TestMethod]
    [ExpectedException(typeof(AdministratorAlreadyExistsException))]
    public void TestAdministratorAlreadyExistsException()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
    }
    
     [TestMethod]
    public void TestRegisterClient()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userControllerDeprecated.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
        Client result = (Client)_userControllerDeprecated.Get(ClientEmail);
        Assert.IsNotNull(result);
        Assert.AreEqual(result.Email,ClientEmail);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotCreateClientBeforeAdminException))]
    public void TestCannotCreateClientBeforeAdministrator()
    {
        _userControllerDeprecated.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
    }
    
    [TestMethod]
    [ExpectedException(typeof(UserAlreadyExistsException))]
    public void TestEmailIsAlreadyInUse()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userControllerDeprecated.RegisterClient(ClientName,AdminEmail,ClientPassword,ClientPassword);
    }
    
    [TestMethod]
    public void TestGetUser()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userControllerDeprecated.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);

        User newUser = _userControllerDeprecated.Get(ClientEmail);
        int id = newUser.Id;
        
        _userControllerDeprecated.Get(id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(UserDoesNotExistException))]
    public void TestCannotFindUserById()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);

        User newUser = _userControllerDeprecated.Get(AdminEmail);
        int id = newUser.Id;

        _userControllerDeprecated.Remove(id);
        _userControllerDeprecated.Get(id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(UserDoesNotExistException))]
    public void TestCannotFindUserByEmail()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);

        User newUser = _userControllerDeprecated.Get(AdminEmail);
        int id = newUser.Id;

        _userControllerDeprecated.Remove(id);
        _userControllerDeprecated.Get(AdminEmail);
    }

    [TestMethod]
    public void TestRemoveUser()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userControllerDeprecated.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);

        User newUser = _userControllerDeprecated.Get(ClientEmail);
        int id = newUser.Id;

        _userControllerDeprecated.Remove(id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(UserDoesNotExistException))]
    public void TestCannotRemoveUser()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userControllerDeprecated.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);

        User newUser = _userControllerDeprecated.Get(ClientEmail);
        int id = newUser.Id;

        _userControllerDeprecated.Remove(id);
        _userControllerDeprecated.Remove(id);
    }
    
    [TestMethod]
    public void TestGetAdministrator()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        
        Administrator admin = _userControllerDeprecated.GetAdministrator();
        
        Assert.IsNotNull(admin);
        Assert.AreEqual(AdminName, admin.Name);
        Assert.AreEqual(AdminEmail, admin.Email);
    }
    
    [TestMethod]
    [ExpectedException(typeof(EmptyAdministratorException))]
    public void TestCannotGetAdministrator()
    {
        _userControllerDeprecated.GetAdministrator();
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotGetLogs()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userControllerDeprecated.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        _session.LoginUser(ClientEmail,ClientPassword);
        _session.LogoutUser();
        _session.LoginUser(ClientEmail,ClientPassword);
        _userControllerDeprecated.GetLogs(_userControllerDeprecated.Get(ClientEmail), _session.ActiveUser);
    }
    
    [TestMethod]
    public void TestGetLogs()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userControllerDeprecated.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        DateTime now = DateTime.Now.AddSeconds(-DateTime.Now.Second);
        _session.LoginUser(AdminEmail,AdminPassword);
        _session.LogoutUser();
        _session.LoginUser(AdminEmail,AdminPassword);
        List<LogEntry> logs = _userControllerDeprecated.GetLogs(_userControllerDeprecated.Get(AdminEmail), _session.ActiveUser);
        
        Assert.AreEqual(3,logs.Count);
        Assert.AreEqual(logs[0].Message , UserLogInLogMessage);
        Assert.IsTrue(logs.Any(log => now.Date == log.Timestamp.Date
                              && now.Hour == log.Timestamp.Hour && now.Minute == log.Timestamp.Minute));
        Assert.AreEqual(logs[0].UserId , _userControllerDeprecated.Get(AdminEmail).Id);
        Assert.IsTrue(logs[0].Id >= 0);
    }
    
    [TestMethod]
    [ExpectedException(typeof(EmptyActionLogException))]
    public void TestEmptyActionLog()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _session.LoginUser(AdminEmail,AdminPassword);
        _session.LogoutUser();
        _session.LoginUser(AdminEmail,AdminPassword);
        _userControllerDeprecated.LogAction(_userControllerDeprecated.Get(AdminEmail),"",DateTime.Now);
    }

    [TestMethod]
    public void TestGetListOfUsers()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userControllerDeprecated.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        List<User> users = _userControllerDeprecated.GetAll();
        Assert.AreEqual(2,users.Count);
    }

    [TestMethod]
    public void TestGetListOfAllLogs()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userControllerDeprecated.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        _session.LoginUser(ClientEmail,ClientPassword);
        _session.LogoutUser();
        _session.LoginUser(AdminEmail,AdminPassword);
        
        List<LogEntry> logs = _userControllerDeprecated.GetAllLogs(_session.ActiveUser);
        
        Assert.AreEqual(3,logs.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotGetListOfAllLogs()
    {
        _userControllerDeprecated.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userControllerDeprecated.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        _session.LoginUser(AdminEmail,AdminPassword);
        _session.LogoutUser();
        _session.LoginUser(ClientEmail,ClientPassword);
        
        List<LogEntry> logs = _userControllerDeprecated.GetAllLogs(_session.ActiveUser);
    }
}
