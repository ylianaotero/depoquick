using DepoQuick.Domain;
using DepoQuick.Domain.Exceptions.ControllerExceptions;

namespace BusinessLogic;

public class Controller
{
    private readonly MemoryDataBase _memoryDataBase;
    
    public Controller(MemoryDataBase memoryDatabase)
    {
        _memoryDataBase = memoryDatabase;
    }
    
    public void AddDeposit(Deposit deposit)
    {
        _memoryDataBase.GetListOfDeposits().Add(deposit);
    }
    
    public Deposit GetDeposit(int id)
    {
        Deposit deposit = SearchDeposit(id); 
        if (deposit == null)
        {
            throw new DepositNotFoundException("Deposito no encontrado"); 
        }
        else
        {
            return deposit; 
        }
    }
    

    public Deposit SearchDeposit(int id)
    {
        return _memoryDataBase.GetListOfDeposits().FirstOrDefault(deposit => deposit.GetId() == id);
    }
    
    public Reservation GetReservation(int id)
    {
        Reservation reservation = SearchReservation(id); 
        if (reservation == null)
        {
            throw new ReservationNotFoundException("Reservacion no encontrada"); 
        }
        else
        {
            return reservation; 
        }
    }
    
    private Reservation SearchReservation(int id)
    {
        return _memoryDataBase.GetReservations().FirstOrDefault(reservation => reservation.GetId() == id);
    }

    public void AddReservation(Reservation reservation)
    {
        _memoryDataBase.GetReservations().Add(reservation);
    }

    public void AddPromotion(Promotion promotion)
    {
        _memoryDataBase.GetPromotions().Add(promotion);
    }

    public Promotion GetPromotion(int id)
    {
        Promotion promotion = SearchPromotion(id);
        if (promotion == null)
        {
            throw new PromotionNotFoundException("Promoción no encontrada.");
        }
        return promotion;
    }

    private Promotion SearchPromotion(int id)
    {
        return _memoryDataBase.GetPromotions().Find(p => p.GetId() == id);
    }
}
