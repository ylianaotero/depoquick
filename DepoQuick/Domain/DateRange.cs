using DepoQuick.Domain.Exceptions.DateRangeExceptions;

namespace DepoQuick.Domain;

public class DateRange
{
    private DateTime _initialDate;
    private DateTime _finalDate;
    
    public DateRange(DateTime initialDate, DateTime finalDate)
    {
        if (DatesAreValid(initialDate, finalDate))
        {
            _initialDate = initialDate;
            _finalDate = finalDate;
        }
    }
    
    public DateTime GetInitialDate()
    {
        return _initialDate;
    }
    
    public DateTime GetFinalDate()
    {
        return _finalDate;
    }
    
    public int NumberOfDays()
    {
        TimeSpan difference = _finalDate - _initialDate;
        int numberOfDays = difference.Days;
        return numberOfDays;
    }
    
    private bool DatesAreValid(DateTime initialDate, DateTime finalDate)
    {
        if (TheTwoDatesAreEmpty(initialDate,finalDate))
        {
            throw new EmptyDateRangeException ("Ambas fechas vacias, no se puede");
        }
        if (InitialDateIsEmpty(initialDate))
        {
            throw new EmptyDateRangeException ("Fecha inicial vacia, no se puede");
        }

        if (FinalDateIsEmpty(finalDate))
        {
            throw new EmptyDateRangeException ("Fecha final vacia, no se puede");
        }

        if (!EndDateIsLater(initialDate, finalDate))
        {
            throw new InvalidDateRangeException("Rango de fechas no valido, fecha final debe ser posterior a fecha inicial");
        }

        return true; 


    }

    private bool TheTwoDatesAreEmpty(DateTime initialDate, DateTime finalDate)
    {
        if (!DateIsNotEmpty(initialDate) && !DateIsNotEmpty(finalDate))
        {
            return true; 
        }
        return false; 
    }

    private bool InitialDateIsEmpty(DateTime initialDate)
    {
        if (!DateIsNotEmpty(initialDate))
        {
            return true; 
        }

        return false; 
    }
    
    private bool FinalDateIsEmpty(DateTime finalDate)
    {
        if (!DateIsNotEmpty(finalDate))
        {
            return true; 
        }

        return false; 
    }

    private bool DateIsNotEmpty(DateTime date)
    {
        if (date != DateTime.MinValue)
        {
            return true; 
        }
        else
        {
            return false;
        }
    }

    private bool EndDateIsLater(DateTime initialDate, DateTime finalDate)
    {
        if (initialDate < finalDate)
        {
            return true; 
        }
        else
        {
            return false; 
        }
    }
    
    public bool DateRangeIsOverlapping(DateRange dateRange)
    {
        if (IsDateInRange(dateRange.GetInitialDate()) || IsDateInRange(dateRange.GetFinalDate()))
        {
            return true; 
        }
        return false; 
    }
    
    public bool IsDateInRange(DateTime date)
    {
        if (date >= _initialDate && date <= _finalDate)
        {
            return true; 
        }
        return false; 
    }
    
    
}