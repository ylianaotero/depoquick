using DepoQuick.Domain;

namespace BusinessLogic;

public class MemoryDataBase
{
    private User _activeUser;
    private Administrator _administrator;
    private List<User> _listOfUsers;
    private List<Deposit> _listOfDeposits;
    private List<Reservation> _listOfReservations; 
    private List<Promotion> _listOfPromotions;
    private List<Rating> _listOfRatings;


    public MemoryDataBase()
    {
       _listOfReservations = new List<Reservation>();
        _listOfDeposits = new List<Deposit>();
        _listOfPromotions = new List<Promotion>();
        _listOfUsers = new List<User>();
        _listOfRatings = new List<Rating>();
    }
    
    public Administrator GetAdministrator() 
    {
        return _administrator; 
    }

    public void SetAdministrator(Administrator administrator)
    {
        _administrator = administrator; 
    }
    
    public User GetActiveUser()
    {
        return _activeUser; 
    }
    
    public List<Deposit> GetDeposits()
    {
        return _listOfDeposits; 
    }
    
    public List<Reservation> GetReservations()
    {
        return _listOfReservations;
    }
    
    public List<Rating> GetRatings()
    {
        return _listOfRatings;
    }

    public List<Promotion> GetPromotions()
    {
        return _listOfPromotions;
    }

    public List<User> GetUsers()
    {
        return _listOfUsers;
    }

    public void SetActiveUser(User user)
    {
        _activeUser = user;
    }
}