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
        return IsDateInRange(dateRange.InitialDate) || IsDateInRange(dateRange.FinalDate);
    }

    public bool IsDateInRange(DateTime date)
    {
        return date >= InitialDate && date <= FinalDate;
    }

    public bool Contains(DateRange dateRange)
    {
        return IsDateInRange(dateRange.InitialDate) || IsDateInRange(dateRange.FinalDate);
    }

    private void ValidateDateRange(DateTime initialDate, DateTime finalDate)
    {
        if (!(DateIsNotEmpty(initialDate) && DateIsNotEmpty(finalDate)))
        {
            throw new EmptyDateRangeException(EmptyDateRangeMessage);
        }

        if (!EndDateIsLater(initialDate, finalDate))
        {
            throw new InvalidDateRangeException(InvalidDateRangeMessage);
        }
    }

    private bool DateIsNotEmpty(DateTime date)
    {
        return date != DateTime.MinValue;
    }

    private bool EndDateIsLater(DateTime initialDate, DateTime finalDate)
    {
      return initialDate.Date < finalDate.Date;
    }
}