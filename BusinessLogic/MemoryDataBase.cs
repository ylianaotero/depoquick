using System.Collections;
using DepoQuick.Domain;
using DepoQuick.Domain.Exceptions.ControllerExceptions;
using DepoQuick.Domain.Exceptions.MemoryDataBaseExceptions;

namespace BusinessLogic;


public class MemoryDataBase
{
    private User _userActive;
    private Administrator _administrator;
    private List<User> _listOfUsers;
    private List<Deposit> _listOfDeposits;
    private List<Reservation> _listOfReservations; 
    private List<Promotion> _listOfPromotions;

    public MemoryDataBase()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true, false);
        Deposit deposit2 = new Deposit('C', "Pequeño", true, false);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit, client, stay);
        Reservation reservation2 = new Reservation(deposit2, client, stay);
        reservation.SetSate(1);
        reservation2.SetSate(1);
        _listOfReservations = new List<Reservation>();
        //_listOfDeposits = new List<Deposit>();
        //agrego usuario para probar pq todavia no esta listo
        //_userActive = new Client("Juan Perez", "nombre@dominio.es", "Contrasena#1"); 
        // _userActive = new Administrator("Juan Perez", "nombre@dominio.es", "Contrasena#1");
       // _userActive.SetIsAdministrator(true);
        //Reservation reservation2 = new Reservation(deposit, (Client)_userActive, stay);
        //reservation2.SetSate(-1);
        //le estoy poniendo depositos de ejemplo para probar mientras 
        _listOfReservations = new List<Reservation>()
        {
            reservation,reservation2,reservation
        };

        Deposit d = new Deposit('a', "Mediano", true, false);
        Promotion p = new Promotion();
        p.SetLabel("Promo1");
        p.SetDiscountRate(0.1);
        d.AddPromotion(p);
        
        Promotion p2 = new Promotion();
        p2.SetLabel("Promo2");
        p2.SetDiscountRate(0.2);
        d.AddPromotion(p2);
        

       /* Client user = (Client)_userActive; 
        user.AddReservation(reservation2);*/
        _listOfDeposits = new List<Deposit>()
             {
                 new Deposit('a', "Mediano", true, false),
                 new Deposit('B', "Mediano", true, false),
                 new Deposit('A', "Mediano", false, false),
                 new Deposit('E', "Mediano", false, false),
                 d
             }
            ;
        

        _listOfPromotions = new List<Promotion>();
        _listOfUsers = new List<User>();
        //_listOfUsers.Add(_userActive);
    }
    
    public User GetActiveUser()
    {
        return _userActive; 
    }
    
    public List<Deposit> GetListOfDeposits()
    {
        return _listOfDeposits; 
    }
    
    public List<Reservation> GetReservations()
    {
        return _listOfReservations;
    }

    public List<Promotion> GetPromotions()
    {
        return _listOfPromotions;
    }

    public List<User> GetListOfUsers()
    {
        return _listOfUsers;
    }


    public void setActiveUser(User user)
    {
        _userActive = user;
    }
}