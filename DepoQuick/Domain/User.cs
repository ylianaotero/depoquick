using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using DepoQuick.Exceptions.UserExceptions;
using Microsoft.EntityFrameworkCore;

namespace DepoQuick.Domain;

public class User
{
    private const int MaxNameLength = 100;
    private const int MinPasswordLength = 8;
    
    private static int s_lastId = 0;
    
    [Key]
    public int Id { get; init; }
    
    public bool IsAdministrator { get; protected init; }

    private string _name;
    private string _email;
    private string _password;
    private List<(string, DateTime)> _logs;
    
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
    
    public List<(string, DateTime)> Logs
    {
        get => _logs;
        private init => _logs = value;
    }
    
    public User()
    {
        Id = s_lastId + 1;
        s_lastId = Id;
        
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

        Id = s_lastId + 1;
        s_lastId = Id;
    }
    
    private void ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new EmptyUserNameException("El nombre no puede estar vacío.");
        }
        
        if (name.Length > MaxNameLength)
        {
            throw new UserNameTooLongException("El nombre no puede tener más de " + MaxNameLength + " caracteres.");
        }
        
        if (!Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
        {
            throw new InvalidUserNameException("El nombre solo puede contener letras y espacios.");
        }
    }
    
    private void ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new EmptyUserEmailException("El correo electrónico no puede estar vacío.");
        }
        
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        
        if (!Regex.IsMatch(email, pattern))
        {
            throw new InvalidUserEmailException("El formato del correo electrónico no es válido.");
        }
    }
    
    private void ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new EmptyUserPasswordException("La contraseña no puede estar vacía.");
        }
        
        if (password.Length < MinPasswordLength)
        {
            throw new PasswordTooShortException("La contraseña debe tener al menos " + " caracteres.");
        }
        
        if (!Regex.IsMatch(password, @"^(?=.*[#@$.,%])(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
        {
            throw new InvalidUserPasswordException("La contraseña debe contener al menos un símbolo (#@$.,%), una letra minúscula, una letra mayúscula y un dígito.");
        }
    }
    
    public static void ValidatePasswordConfirmation(string password, string passwordConfirmation)
    {
        if (string.Compare(password,passwordConfirmation) != 0)
        {
            throw new UserPasswordsDoNotMatchException("Las contraseñas no coinciden.");
        }
    }
    
    public void LogAction(string message, DateTime timestamp)
    {
        if (string.IsNullOrEmpty(message))
        {
            throw new EmptyActionLogException("El mensaje no puede estar vacío.");
        }
        else
        {
            Logs.Add((message, timestamp));
        }
    }
}