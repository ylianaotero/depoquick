﻿using BusinessLogic;
using BusinessLogic.Controllers;
using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Domain;
using DepoQuick.Exceptions.UserExceptions;

namespace DepoQuickTests;

[TestClass]
public class UserControllerTests
{
    private UserController _userController;
    private LogController _logController;
    private SessionController _sessionController;
    
    private const string AdminName = "Administrator";
    private const string AdminEmail = "administrator@domain.com";
    private const string AdminPassword = "Password1#";
    private const string ClientName = "Client";
    private const string ClientEmail = "client@domain.com";
    private const string ClientPassword = "Password2#";
    
    private const string InvalidEmail = "invalidemail";
    private const int InvalidId = -1;
    
    private const string UserLogInLogMessage = "Ingresó al sistema";

    [TestInitialize]
    public void Initialize()
    {
        var context = TestContextFactory.CreateContext();
        IRepository<User> userRepository = new SqlRepository<User>(context);
        IRepository<LogEntry> logRepository = new SqlRepository<LogEntry>(context);
        
        _userController = new UserController(userRepository);
        _logController = new LogController(logRepository);
        
        _sessionController = new SessionController(_userController, _logController);
    }
    
    [TestMethod]
    public void TestRegisterAdministrator()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);
        Administrator result = (Administrator)_userController.GetUserByEmail(AdminEmail);
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
        Client result = (Client)_userController.GetUserByEmail(ClientEmail);
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

        User newUser = _userController.GetUserByEmail(ClientEmail);
        int id = newUser.Id;
        
        _userController.Get(id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(UserDoesNotExistException))]
    public void TestCannotFindUserById()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);

        User newUser = _userController.GetUserByEmail(AdminEmail);
        int id = newUser.Id;
        
        _userController.Get(InvalidId);
    }
    
    [TestMethod]
    [ExpectedException(typeof(UserDoesNotExistException))]
    public void TestCannotFindUserByEmail()
    {
        _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);

        User newUser = _userController.GetUserByEmail(AdminEmail);
        
        _userController.GetUserByEmail(ClientEmail);
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
