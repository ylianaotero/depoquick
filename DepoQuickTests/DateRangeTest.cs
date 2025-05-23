using DepoQuick.Domain;
using DepoQuick.Exceptions.DateRangeExceptions;

namespace DepoQuickTests;

[TestClass]
public class DateRangeTest
{
    [TestMethod]
    public void TestDefaultDateRange()
    {
        DateRange dateRange = new DateRange();
        
        Assert.AreEqual(DateTime.MinValue, dateRange.GetInitialDate());
        Assert.AreEqual(DateTime.Now.Date, dateRange.GetFinalDate());
    }
    
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
    [ExpectedException(typeof(EmptyDateRangeException))] 
    public void TestBothDatesEmpty()
    {
        DateTime dateFrom = new DateTime().Date;
        DateTime dateTo = new DateTime().Date;

        new DateRange(dateFrom, dateTo); 
        
    }
    
    [TestMethod]
    public void TestValidDateRange()
    {
        DateTime dateFrom = new DateTime(2024, 4, 1).Date;
        DateTime dateTo = new DateTime(2024,4,5).Date;

        DateRange dateRange = new DateRange(dateFrom, dateTo); 
        
        Assert.AreEqual(dateFrom, dateRange.GetInitialDate());
        Assert.AreEqual(dateTo, dateRange.GetFinalDate());
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
    [ExpectedException(typeof(InvalidDateRangeException))] 
    public void TestInvalidDateRangeSameDay()
    {
        DateTime dateFrom = new DateTime(2024, 4, 1).Date;
        DateTime dateTo = new DateTime(2024,4,1).Date;

        new DateRange(dateFrom, dateTo); 
        
    }
    
    [TestMethod]
    public void TestNumberOfDays()
    {
        DateTime dateFrom = new DateTime(2024, 4, 1).Date;
        DateTime dateTo = new DateTime(2024,4,5).Date;
        
        DateRange date = new DateRange(dateFrom, dateTo);
        Assert.AreEqual(4,date.NumberOfDays());
        
    }
    
    [TestMethod]
    public void TestNumberOfDays1Day()
    {
        DateTime dateFrom = new DateTime(2024, 4, 1).Date;
        DateTime dateTo = new DateTime(2024,4,2).Date;
        
        DateRange date = new DateRange(dateFrom, dateTo);
        Assert.AreEqual(1,date.NumberOfDays());
        
    }
    
    [TestMethod]
    public void TestDateRangeIsOverlapping()
    {
        DateTime dateFrom = new DateTime(2024, 4, 1).Date;
        DateTime dateTo = new DateTime(2024,4,2).Date;
        
        DateTime dateFrom2 = new DateTime(2024, 4, 1).Date;
        DateTime dateTo2 = new DateTime(2024,4,2).Date;
        
        DateRange date = new DateRange(dateFrom, dateTo);
        DateRange date2 = new DateRange(dateFrom2, dateTo2);
        
        Assert.IsTrue(date.DateRangeIsOverlapping(date2));
        
    }
    
    [TestMethod]
    public void TestDateRangeIsNoOverlapping()
    {
        DateTime dateFrom = new DateTime(2024, 4, 1).Date;
        DateTime dateTo = new DateTime(2024,4,2).Date;
        
        DateTime dateFrom2 = new DateTime(2022, 4, 1).Date;
        DateTime dateTo2 = new DateTime(2022,4,2).Date;
        
        DateRange date = new DateRange(dateFrom, dateTo);
        DateRange date2 = new DateRange(dateFrom2, dateTo2);
        
        Assert.IsFalse(date.DateRangeIsOverlapping(date2));
        
    }
    
    [TestMethod]
    public void TestDateRangeIsNoContained()
    {
        DateTime dateFrom = new DateTime(2024, 4, 1).Date;
        DateTime dateTo = new DateTime(2024,4,2).Date;
        
        DateTime dateFrom2 = new DateTime(2022, 4, 1).Date;
        DateTime dateTo2 = new DateTime(2022,4,2).Date;
        
        DateRange date = new DateRange(dateFrom, dateTo);
        DateRange date2 = new DateRange(dateFrom2, dateTo2);
        
        Assert.IsFalse(date.Contains(date2));
        
    }
    
    
}