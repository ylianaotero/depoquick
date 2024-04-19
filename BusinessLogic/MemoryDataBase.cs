using DepoQuick.Domain;
using DepoQuick.Domain.Exceptions.ControllerExceptions;

namespace BusinessLogic;


public class MemoryDataBase
{
    
    private List<Deposit> _listOfDeposits;
    private User _user;
    private List<Reservation> _listOfReservations; 

    public MemoryDataBase()
    {
        Client client = new Client("Maria Perez","maria@gmail.com","Maria1..");
        Deposit deposit = new Deposit('A', "Pequeño", true, false);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        Reservation reservation = new Reservation(deposit, client, stay);
        
        //agrego usuario para probar pq todavia no esta listo
        _user = new Client("Juan Perez", "nombre@dominio.es", "Contrasena#1"); 
        //_user = new Administrator("Juan Perez", "nombre@dominio.es", "Contrasena#1");
        //_user.SetIsAdministrator(true);
        Reservation reservation2 = new Reservation(deposit, (Client)_user, stay);
        reservation2.SetSate(-1);
        //le estoy poniendo depositos de ejemplo para probar mientras 
        _listOfReservations = new List<Reservation>()
        {
            reservation,reservation2
        };
        Client user = (Client)_user; 
        user.AddReservation(reservation2);
        _listOfDeposits = new List<Deposit>()
             {
                 new Deposit('a', "Mediano", true, false),
                 new Deposit('B', "Mediano", true, false),
                 new Deposit('A', "Mediano", false, false)
             }
            ; 
    }
    
    public User GetUser()
    {
        return _user; 
    }
    
    public void LoginUser(User user)
    {
        _user = user;
    }
    
    public List<Deposit> GetListOfDeposits()
    {
        return _listOfDeposits; 
    }
    
    public List<Reservation> GetReservations()
    {
        return _listOfReservations;
    }
    
}