using DepoQuick.Domain;
using DepoQuick.Domain.Exceptions.ControllerExceptions;

namespace BusinessLogic;


public class Controller
{
    
    private List<Deposit> _listOfDeposits;
    private User _user; 

    public Controller()
    {
        //agrego usuario para probar pq todavia no esta listo
        //_user = new User("Juan Perez", "nombre@dominio.es", "Contrasena#1"); 
        //le estoy poniendo depositos de ejemplo para probar mientras 
        _listOfDeposits = new List<Deposit>()
            /* {
                 new Deposit('a', "Mediano", true, false),
                 new Deposit('B', "Mediano", true, false),
                 new Deposit('A', "Mediano", false, false)
             }*/
            ; 
    }
    
    public void AddDeposit(Deposit deposit)
    {
        _listOfDeposits.Add(deposit);
    }

    public List<Deposit> GetListOfDeposits()
    {
        return _listOfDeposits; 
    }

    public Deposit GetDeposit(int id)
    {
        Deposit deposit = SearchDeposit(id); 
        if (IsNull(deposit))
        {
            throw new ExceptionDepositNotFound("Deposito no encontrado"); 
        }
        else
        {
            return deposit; 
        }
    }

    public Deposit SearchDeposit(int id)
    {
        return _listOfDeposits.FirstOrDefault(deposit => deposit.GetId() == id);
    }

    public bool IsNull(Deposit deposit)
    {
        return deposit == null; 
    }

    public User GetUser()
    {
        return _user; 
    }
}