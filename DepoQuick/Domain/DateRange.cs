using DepoQuick.Domain.Exceptions.DateRangeExceptions;

namespace DepoQuick.Domain;

public class DateRange
{
    private DateTime _initialDate { get; set; }
    private DateTime _finalDate { get; set; }

    public DateRange(DateTime initialDate, DateTime finalDate)
    {
        if (datesAreValid(initialDate, finalDate))
        {
            _initialDate = initialDate;
            _finalDate = finalDate;
        }
    }
    
    public DateTime getInitialDate()
    {
        return _initialDate;
    }
    
    public DateTime getFinalDate()
    {
        return _finalDate;
    }
    
    private bool datesAreValid(DateTime initialDate, DateTime finalDate)
    {
        if (dateIsNotEmpty(initialDate) && dateIsNotEmpty(finalDate) && endDateIsLater(initialDate, finalDate))
        {
            return true; 
        }
        return false; 
    }

    private bool dateIsNotEmpty(DateTime date)
    {
        if (date != DateTime.MinValue)
        {
            return true; 
        }
        else
        {
            throw new EmptyDateRangeException ("No se puede ingresar una fecha vacia");
        }
    }

    private bool endDateIsLater(DateTime initialDate, DateTime finalDate)
    {
        if (initialDate < finalDate)
        {
            return true; 
        }
        else
        {
            throw new InvalidDateRangeException("Rango de fechas no valido, fecha final debe ser posterior a fecha inicial");
        }
    }
    
}