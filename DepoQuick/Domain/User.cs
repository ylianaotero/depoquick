using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using DepoQuick.Exceptions.UserExceptions;

namespace DepoQuick.Domain;

public class User
{
    private const string EmptyUserNameMessage = "El nombre no puede estar vacío";
    private const string UserNameTooLongMessage = "El nombre no puede tener más de 100 caracteres";
    private const string InvalidUserNameMessage = "El nombre solo puede contener letras y espacios";

    private const string EmptyUserEmailMessage = "El correo electrónico no puede estar vacío";
    private const string InvalidUserEmailMessage = "El formato del correo electrónico no es válido";
    
    private const string EmptyUserPasswordMessage = "La contraseña no puede estar vacía";
    private const string PasswordTooShortMessage = "La contraseña debe tener al menos 8 caracteres";
    private const string InvalidUserPasswordMessage = "La contraseña debe contener al menos un símbolo (#@$.,%), " +
                                                      "una letra minúscula, una letra mayúscula y un dígito";
    private const string PasswordsDoNotMatchMessage = "Las contraseñas no coinciden";

    private const int MaxNameLength = 100;
    private const int MinPasswordLength = 8;
    
    [Key]
    public int Id { get; set; }
    
    public bool IsAdministrator { get; protected init; }

    private string _name;
    private string _email;
    private string _password;
    
    private List<LogEntry> _logs;
    
    
    public string Name
    {
        get => _name;
        set
        {
            ValidateName(value);
            _name = value;
        }
    }

    public string Email { 
        get => _email;
        set
        {
            ValidateEmail(value);
            _email = value; 
        } 
    }
    
    public string Password { 
        get => _password;
        set
        {
            ValidatePassword(value);
            _password = value;
        }
    }
    
    public List<LogEntry> Logs
    {
        get => _logs;
        private init => _logs = value;
    }
    
    public User()
    {
        Logs = new();
    }
    
    public User(string name, string email, string password)
    {
        ValidateName(name);
        ValidateEmail(email);
        ValidatePassword(password);
        
        Name = name;
        Email = email;
        Password = password;

        Logs = new();
    }
    
    public static void ValidatePasswordConfirmation(string password, string passwordConfirmation)
    {
        if (string.Compare(password,passwordConfirmation) != 0)
        {
            throw new UserPasswordsDoNotMatchException(PasswordsDoNotMatchMessage);
        }
    }
    
    private void ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new EmptyUserNameException(EmptyUserNameMessage);
        }
        
        if (name.Length > MaxNameLength)
        {
            throw new UserNameTooLongException(UserNameTooLongMessage);
        }
        
        if (!Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
        {
            throw new InvalidUserNameException(InvalidUserNameMessage);
        }
    }
    
    private void ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new EmptyUserEmailException(EmptyUserEmailMessage);
        }
        
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        
        if (!Regex.IsMatch(email, pattern))
        {
            throw new InvalidUserEmailException(InvalidUserEmailMessage);
        }
    }
    
    private void ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new EmptyUserPasswordException(EmptyUserPasswordMessage);
        }
        
        if (password.Length < MinPasswordLength)
        {
            throw new PasswordTooShortException(PasswordTooShortMessage);
        }
        
        if (!Regex.IsMatch(password, @"^(?=.*[#@$.,%])(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
        {
            throw new InvalidUserPasswordException(InvalidUserPasswordMessage);
        }
    }
}