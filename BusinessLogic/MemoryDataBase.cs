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
       /* Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true, false);
        Deposit deposit2 = new Deposit('C', "Pequeño", true, false);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit, client, stay);
        Reservation reservation2 = new Reservation(deposit2, client, stay);
        

        reservation.SetSate(1);
        reservation2.SetSate(1);*/
        
        _listOfReservations = new List<Reservation>()
        /*{
            reservation,reservation2,reservation
        }*/;
        
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