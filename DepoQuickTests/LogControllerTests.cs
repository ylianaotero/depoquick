using BusinessLogic;
using BusinessLogic.Controllers;
using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Domain;
using DepoQuick.Exceptions.UserExceptions;

namespace DepoQuickTests;

[TestClass]
public class LogControllerTests
{
    private LogController _logController;
    private UserController _userController;
    private SessionController _sessionController;
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
        IRepository<User> _userRepository = new SqlRepository<User>(context);
        
        _userController = new UserController(_userRepository);
        _logController = new LogController(new SqlRepository<LogEntry>(context));
        
        _sessionController = new SessionController(_userController, _logController);
    }
    
    [TestMethod]
    public void TestLogsId()
    {
        LogEntry log = new LogEntry();
        log.Id = 0;
        
        Assert.AreEqual(0,log.Id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(EmptyActionLogException))]
    public void TestEmptyActionLog()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        _sessionController.LogoutUser();
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        _logController.LogAction(_userController.GetUserByEmail(AdminEmail),"",DateTime.Now);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotGetLogs()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        _sessionController.LoginUser(ClientEmail,ClientPassword);
        _sessionController.LogoutUser();
        _sessionController.LoginUser(ClientEmail,ClientPassword);
        _logController.GetLogs(_userController.GetUserByEmail(ClientEmail), _sessionController.ActiveUser);
    }
    
    [TestMethod]
    public void TestGetLogs()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        DateTime now = DateTime.Now.AddSeconds(-DateTime.Now.Second);
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        _sessionController.LogoutUser();
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        List<LogEntry> logs = _logController.GetLogs(_userController.GetUserByEmail(AdminEmail), _sessionController.ActiveUser);
        
        Assert.AreEqual(3,logs.Count);
        Assert.AreEqual(logs[0].Message , UserLogInLogMessage);
        Assert.IsTrue(logs.Any(log => now.Date == log.Timestamp.Date
                                      && now.Hour == log.Timestamp.Hour && now.Minute == log.Timestamp.Minute));
        Assert.AreEqual(logs[0].UserId , _userController.GetUserByEmail(AdminEmail).Id);
        Assert.IsTrue(logs[0].Id >= 0);
    }
    
    [TestMethod]
    public void TestGetListOfAllLogs()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        _sessionController.LoginUser(ClientEmail,ClientPassword);
        _sessionController.LogoutUser();
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        
        List<LogEntry> logs = _logController.GetAllLogs(_sessionController.ActiveUser);
        
        Assert.AreEqual(3,logs.Count);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
    public void TestClientCannotGetListOfAllLogs()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
        _sessionController.LoginUser(AdminEmail,AdminPassword);
        _sessionController.LogoutUser();
        _sessionController.LoginUser(ClientEmail,ClientPassword);
        
        List<LogEntry> logs = _logController.GetAllLogs(_sessionController.ActiveUser);
    }
    
}