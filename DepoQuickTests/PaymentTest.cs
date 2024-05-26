using DepoQuick.Domain;
using DepoQuick.Exceptions.PaymentExceptions;

namespace DepoQuickTests;

[TestClass]
public class PaymentTest
{
    private const string ClientName1 = "Maria Perez";
    private const string ClientEmail1 = "maria@gmail.com";
    private const string ClientPassword1 = "Mariaaa1.";
    private const char DepositArea1 = 'A';
    private const string DepositSize1 = "Pequeño";
    private const bool DepositAirConditioning1 = true;

    private Reservation _reservation; 
    
    [TestInitialize]
    public void Initialize()
    {
        Client client = new Client(ClientName1,ClientEmail1,ClientPassword1);
        Deposit deposit = new Deposit(DepositArea1, DepositSize1, DepositAirConditioning1);
        DateTime dayIn = new DateTime(2024, 04, 07);
        DateTime dayOut = new DateTime(2024, 04, 08);
        DateRange stay = new DateRange(dayIn, dayOut);
        _reservation = new Reservation(deposit,client,stay);
    }
    
    [TestMethod]
    public void TestNewPayment()
    {
        Payment newPayment = new Payment(); 
        
        Assert.AreEqual(0,newPayment.Status);
        
    }
    
    [TestMethod]
    public void TestTwoNewPaymentsWithDiferentId()
    {
        Payment newPayment1 = new Payment(); 
        Payment newPayment2 = new Payment(); 
        
        Assert.AreNotSame(newPayment1.Id, newPayment2.Id);

        Assert.AreEqual(0,newPayment1.Status);
        Assert.AreEqual(0,newPayment2.Status);
        
    }
    
    [TestMethod]
    public void TestCapturePayment()
    {
        Payment newPayment = new Payment();

        newPayment.Reservation = _reservation; 

        newPayment.Capture(); 

        Assert.AreEqual(1,newPayment.Status);
    }
    
    [TestMethod]
    [ExpectedException(typeof(CannotCapturePaymentIfDoesNotHaveAnAssociatedReservation))] 
    public void TestCannotCapturePaymentIfDoesNotHaveAnAssociatedReservation()
    {
        Payment newPayment = new Payment();

        newPayment.Capture(); 

        Assert.AreEqual(1,newPayment.Status);
    }


    
}