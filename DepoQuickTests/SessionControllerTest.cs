using BusinessLogic;
using BusinessLogic.Controllers;
using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Domain;

namespace DepoQuickTests;

[TestClass]
public class SessionControllerTest
{
    private DepoQuickContext _context;
    private LogController _logController;
    private SessionController _sessionController;
    private UserController _userController;

    private const string AdminName = "Administrator";
    private const string AdminEmail = "administrator@domain.com";
    private const string AdminPassword = "Password1#";
    private const string ClientName = "Client";
    private const string ClientEmail = "client@domain.com";
    private const string ClientPassword = "Password2#";
    private const string UserLogInLogMessage = "Ingresó al sistema";
    private const string UserLogOutLogMessage = "Cerró sesión";

    [TestInitialize]
    public void Initialize()
    {
        _context = TestContextFactory.CreateContext();
        _userController = new UserController(new SqlRepository<User>(_context));
        _logController = new LogController(new SqlRepository<LogEntry>(_context));
        _sessionController = new SessionController(_userController, _logController);
    }

    [TestMethod]
    public void TestUserLoggedIn()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _sessionController.LoginUser(AdminEmail, AdminPassword);

        Assert.AreEqual(true, _sessionController.UserLoggedIn());
    }
    
    [TestMethod]
    public void TestLoginValidAdministrator()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _sessionController.LoginUser(AdminEmail, AdminPassword);
         
        Assert.AreEqual(_userController.GetAdministrator(), _sessionController.ActiveUser);
    }
    
    [TestMethod]
    [ExpectedException(typeof(UserDoesNotExistException))]
    public void TestInvalidLogin()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
 
        _sessionController.LoginUser(ClientEmail,ClientPassword);
    }
    
    [TestMethod]
    [ExpectedException(typeof(UserPasswordIsInvalidException))]
    public void TestInvalidLoginBecauseOfWrongPassword()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
 
        _sessionController.LoginUser(AdminEmail,ClientPassword);
    }
    
    [TestMethod]
    public void TestLogoutUser()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _sessionController.LoginUser(AdminEmail, AdminPassword);
        
        _sessionController.LogoutUser();
         
        Assert.AreEqual(null, _sessionController.ActiveUser);
    }
    
    [TestMethod]
    public void TestValidUserLogInLogin()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _sessionController.LoginUser(AdminEmail, AdminPassword);
        
        DateTime now = DateTime.Now.AddSeconds(-DateTime.Now.Second);
        List<LogEntry> logs = _sessionController.ActiveUser.Logs;
         
        Assert.IsTrue(logs.Any(log => log.Message == UserLogInLogMessage));
        Assert.IsTrue(logs.Any(log => now.Date == log.Timestamp.Date 
                                      && now.Hour == log.Timestamp.Hour && now.Minute == log.Timestamp.Minute));
    }
    
    [TestMethod]
    public void TestValidLogInLogout()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _sessionController.LoginUser(AdminEmail, AdminPassword);


        List<LogEntry> logs = _sessionController.ActiveUser.Logs;
        _sessionController.LogoutUser();
        DateTime now = DateTime.Now.AddSeconds(-DateTime.Now.Second);
         
        Assert.IsTrue(logs.Any(log => log.Message == UserLogOutLogMessage));
        Assert.IsTrue(logs.Any(log => now.Date == log.Timestamp.Date && 
                                      now.Hour == log.Timestamp.Hour && now.Minute == log.Timestamp.Minute));
    }

}