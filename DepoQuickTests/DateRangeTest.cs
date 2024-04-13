using DepoQuick.Domain;
using DepoQuick.Domain.Exceptions.DateRangeExceptions;

namespace DepoQuickTests;

[TestClass]
public class DateRangeTest
{
    [TestMethod]
    [ExpectedException(typeof(EmptyDateRangeException))] 
    public void TestEmptyDateFrom()
    {
        DateTime dateFrom = new DateTime().Date;
        DateTime dateTo = new DateTime(2024, 4, 10).Date;

        new DateRange(dateFrom, dateTo); 
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(EmptyDateRangeException))] 
    public void TestEmptyDateTo()
    {
        DateTime dateFrom = new DateTime(2024, 4, 10).Date;
        DateTime dateTo = new DateTime().Date;

        new DateRange(dateFrom, dateTo); 
        
    }
    
    [TestMethod]
    public void TestValidDateRange()
    {
        DateTime dateFrom = new DateTime(2024, 4, 1).Date;
        DateTime dateTo = new DateTime(2024,4,5).Date;

        DateRange dateRange = new DateRange(dateFrom, dateTo); 
        
        Assert.AreEqual(dateFrom, dateRange.getInitialDate());
        Assert.AreEqual(dateTo, dateRange.getFinalDate());
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidDateRangeException))] 
    public void TestInvalidDateRange()
    {
        DateTime dateFrom = new DateTime(2024, 4, 5).Date;
        DateTime dateTo = new DateTime(2024,4,1).Date;

        new DateRange(dateFrom, dateTo); 
        
    }
    
    [TestMethod]
    public void TestNumberOfDays()
    {
        DateTime dateFrom = new DateTime(2024, 4, 1).Date;
        DateTime dateTo = new DateTime(2024,4,5).Date;
        
        DateRange date = new DateRange(dateFrom, dateTo);
        Assert.AreEqual(4,date.numberOfDays());
        
    }
    
}