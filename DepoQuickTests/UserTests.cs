using DepoQuick.Domain;
using DepoQuick.Exceptions.UserExceptions;

namespace DepoQuickTests;

[TestClass]
public class UserTests
{
    private User? _user;

    private const string _name = "Juan Perez";
    private string _nameWithInvalidCharacters = "Juan Pérez1";
    
    private string _email = "nombre@dominio.es";
    
    private string _emailWithInvalidFormat = "nombre@dominio";
    
    
    private string _password = "Contrasena#1";
    private string _password2 = "Contrasena#2";
    private string _passwordWithoutSymbols = "Contrasena1";
    private string _passwordWithoutDigits = "Contrasena#"; 
    private string _passwordAllUpercase = "CONTRASENA#1";
    private string _passwordAllLowercase = "contrasena#1";
    private string _passwordWithLessThan8Characters = "Cont#1";
    
    private string _emptyString = "";
    
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
        _user.Id = 0; 
        Assert.AreEqual(0, _user.Id);
        Assert.AreEqual(_name, _user.Name);
        Assert.AreEqual(_email, _user.Email);
        Assert.AreEqual(_password, _user.Password);
    }

    [TestMethod]
    [ExpectedException(typeof(EmptyUserNameException))]
    public void TestEmptyUserNameIsInvalid()
    {
        string name = _emptyString;
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
        string name = _nameWithInvalidCharacters;
        _user = new User(name, _email, _password);
    }

    [TestMethod]
    [ExpectedException(typeof(EmptyUserEmailException))]
    public void TestEmptyUserEmailIsInvalid()
    {
        string email = _emptyString;
        _user = new User(_name, email, _password);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidUserEmailException))]
    public void TestUserEmailWithInvalidFormatIsInvalid()
    {
        string email = _emailWithInvalidFormat;
        _user = new User(_name, email, _password);
    }

    [TestMethod]
    [ExpectedException(typeof(EmptyUserPasswordException))]
    public void TestEmptyUserPasswordIsInvalid()
    {
        string password = _emptyString;
        _user = new User(_name, _email, password);
    }

    [TestMethod]
    [ExpectedException(typeof(PasswordTooShortException))]
    public void TestUserPasswordWithLessThan8CharactersIsInvalid()
    {
        string password = _passwordWithLessThan8Characters;
        _user = new User(_name, _email, password);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidUserPasswordException))]
    public void TestUserPasswordAllLowercaseIsInvalid()
    {
        string password = _passwordAllLowercase;
        _user = new User(_name, _email, password);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidUserPasswordException))]
    public void TestUserPasswordAllUpercaseIsInvalid()
    {
        string password = _passwordAllUpercase;
        _user = new User(_name, _email, password);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidUserPasswordException))]
    public void TestUserPasswordWithoutDigitsIsInvalid()
    {
        string password = _passwordWithoutDigits;
        _user = new User(_name, _email, password);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidUserPasswordException))]
    public void TestUserPasswordWithoutSymbolsIsInvalid()
    {
        string password = _passwordWithoutSymbols;
        _user = new User(_name, _email, password);
    }

    [TestMethod]
    [ExpectedException(typeof(UserPasswordsDoNotMatchException))]
    public void TestUserPasswordAndConfirmationDoNotMatch()
    {
        string password = _password ;
        string confirmation = _password2;
        User.ValidatePasswordConfirmation(password, confirmation);
    }
    
    [TestMethod]
    public void TestUserPasswordAndConfirmationMatch()
    {
        string password = _password ;
        string confirmation = _password ;
        Assert.IsFalse(ThrowsException(() => User.ValidatePasswordConfirmation(password, confirmation)));
    }
    
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