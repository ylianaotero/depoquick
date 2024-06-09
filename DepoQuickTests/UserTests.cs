using DepoQuick.Domain;
using DepoQuick.Exceptions.UserExceptions;

namespace DepoQuickTests;

[TestClass]
public class UserTests
{
    private User _user;
    private string _name = "Juan Perez";
    private string _email = "nombre@dominio.es";
    private string _password = "Contrasena#1";
    
    [TestMethod]
    public void TestValidUserIsCreated()
    {
        Assert.IsFalse(ThrowsException(() => new User(_name, _email, _password)));
    }

    [TestMethod]
    public void TestValidUserIsCreatedWithTheirAttributes()
    {
        _user = new User(_name, _email, _password);
        Assert.AreEqual(_name, _user.Name);
        Assert.AreEqual(_email, _user.Email);
        Assert.AreEqual(_password, _user.Password);
    }
    
    [TestMethod]
    public void TestUserIsCreatedAndPropertiesAreSet()
    {
        _user = new User();
        _user.Name = _name;
        _user.Email = _email;
        _user.Password = _password;
        Assert.AreEqual(_name, _user.Name);
        Assert.AreEqual(_email, _user.Email);
        Assert.AreEqual(_password, _user.Password);
    }

    /*[TestMethod]
    public void TestTwoUsersHaveDifferentIDs()
    {
        User user1 = new User(_name, _email, _password);
        User user2 = new User(_name, _email, _password);

        Assert.AreNotEqual(user1.Id, user2.Id);
    }

    [TestMethod]
    public void TestIDIsIncremental()
    {
        User user1 = new User(_name, _email, _password);
        User user2 = new User(_name, _email, _password);

        Assert.IsTrue(user1.Id < user2.Id);
    }*/

    [TestMethod]
    [ExpectedException(typeof(EmptyUserNameException))]
    public void TestEmptyUserNameIsInvalid()
    {
        string name = "";
        _user = new User(name, _email, _password);
    }

    [TestMethod]
    [ExpectedException(typeof(UserNameTooLongException))]
    public void TestUserNameWithOver100CharactersIsInvalid()
    {
        string name = new string('a', 101);
        _user = new User(name, _email, _password);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidUserNameException))]
    public void TestUserNameWithInvalidCharactersIsInvalid()
    {
        string name = "Juan Pérez1";
        _user = new User(name, _email, _password);
    }

    [TestMethod]
    [ExpectedException(typeof(EmptyUserEmailException))]
    public void TestEmptyUserEmailIsInvalid()
    {
        string email = "";
        _user = new User(_name, email, _password);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidUserEmailException))]
    public void TestUserEmailWithInvalidFormatIsInvalid()
    {
        string email = "nombre@dominio";
        _user = new User(_name, email, _password);
    }

    [TestMethod]
    [ExpectedException(typeof(EmptyUserPasswordException))]
    public void TestEmptyUserPasswordIsInvalid()
    {
        string password = "";
        _user = new User(_name, _email, password);
    }

    [TestMethod]
    [ExpectedException(typeof(PasswordTooShortException))]
    public void TestUserPasswordWithLessThan8CharactersIsInvalid()
    {
        string password = "Cont#1";
        _user = new User(_name, _email, password);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidUserPasswordException))]
    public void TestUserPasswordAllLowercaseIsInvalid()
    {
        string password = "contrasena#1";
        _user = new User(_name, _email, password);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidUserPasswordException))]
    public void TestUserPasswordAllUpercaseIsInvalid()
    {
        string password = "CONTRASENA#1";
        _user = new User(_name, _email, password);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidUserPasswordException))]
    public void TestUserPasswordWithoutDigitsIsInvalid()
    {
        string password = "Contrasena#";
        _user = new User(_name, _email, password);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidUserPasswordException))]
    public void TestUserPasswordWithoutSymbolsIsInvalid()
    {
        string password = "Contrasena1";
        _user = new User(_name, _email, password);
    }

    [TestMethod]
    [ExpectedException(typeof(UserPasswordsDoNotMatchException))]
    public void TestUserPasswordAndConfirmationDoNotMatch()
    {
        string password = "Contrasena#1";
        string confirmation = "Contrasena#2";
        User.ValidatePasswordConfirmation(password, confirmation);
    }
    
    [TestMethod]
    public void TestUserPasswordAndConfirmationMatch()
    {
        string password = "Contrasena#1";
        string confirmation = "Contrasena#1";
        Assert.IsFalse(ThrowsException(() => User.ValidatePasswordConfirmation(password, confirmation)));
    }

    /*
    [TestMethod] //modificarlo pq es por tabla
    public void TestValidActionGetsLogged()
    {
        User user = new User(_name, _email, _password);

        string action = "Login";
        DateTime timestamp = DateTime.Now;

        user.LogAction(action, timestamp);
        //verrrr
        List<LogEntry> actionsLog = user.Logs;
        
        

        Assert.AreEqual(action, actionsLog[0].Message);
        Assert.AreEqual(timestamp, actionsLog[0].Timestamp);
        Assert.AreEqual(user.Id, actionsLog[0].UserId);
    }

    [TestMethod]
    [ExpectedException(typeof(EmptyActionLogException))]
    public void TestEmptyActionIsInvalid()
    {
        User user = new User(_name, _email, _password);

        string action = "";
        DateTime timestamp = DateTime.Now;

        user.LogAction(action, timestamp);
    }
    */
    private bool ThrowsException(Action functionCall)
    {
        try
        {
            functionCall();
            return false; 
        }
        catch 
        {
            return true;
        }
    }
}