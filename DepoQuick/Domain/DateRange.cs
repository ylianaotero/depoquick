using System.ComponentModel.DataAnnotations.Schema;
using DepoQuick.Exceptions.DateRangeExceptions;

namespace DepoQuick.Domain;

[ComplexType]
public class DateRange
{
    private const string EmptyDateRangeMessage = "Las fechas no pueden ser vacias";
    private const string InvalidDateRangeMessage = "La fecha final debe ser posterior a fecha inicial";
    
    public DateTime InitialDate { get; private set; }
    public DateTime FinalDate { get; private set; }

    public DateRange()
    {
        InitialDate = DateTime.MinValue;
        FinalDate = DateTime.Now.Date;
    }

    public DateRange(DateTime initialDate, DateTime finalDate)
    {
        ValidateDateRange(initialDate, finalDate);
        InitialDate = initialDate;
        FinalDate = finalDate;
    }

    public DateTime GetInitialDate()
    {
        return InitialDate;
    }

    public DateTime GetFinalDate()
    {
        return FinalDate;
    }

    public int NumberOfDays()
    {
        TimeSpan difference = FinalDate - InitialDate;
        int numberOfDays = difference.Days;
        return numberOfDays;
    }
    
    public bool DateRangeIsOverlapping(DateRange dateRange)
    {
        if (IsDateInRange(dateRange.InitialDate) || IsDateInRange(dateRange.FinalDate))
        {
            return true;
        }

        return false;
    }

    public bool IsDateInRange(DateTime date)
    {
        if (date >= InitialDate && date <= FinalDate)
        {
            return true;
        }

        return false;
    }

    public bool Contains(DateRange dateRange)
    {
        if (IsDateInRange(dateRange.InitialDate) || IsDateInRange(dateRange.FinalDate))
        {
            return true;
        }

        return false;
    }

    private void ValidateDateRange(DateTime initialDate, DateTime finalDate)
    {
        if (TheTwoDatesAreEmpty(initialDate, finalDate) || InitialDateIsEmpty(initialDate) || FinalDateIsEmpty(finalDate))
        {
            throw new EmptyDateRangeException(EmptyDateRangeMessage);
        }

        if (!EndDateIsLater(initialDate, finalDate))
        {
            throw new InvalidDateRangeException(InvalidDateRangeMessage);
        }
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
        
        return false;
    }

    private bool EndDateIsLater(DateTime initialDate, DateTime finalDate)
    {
        if (initialDate.Date < finalDate.Date)
        {
            return true;
        }
        
        return false;
    }
}