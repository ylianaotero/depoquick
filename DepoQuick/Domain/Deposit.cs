using System.ComponentModel.DataAnnotations;
using DepoQuick.Exceptions.DepositExceptions;
using System.Text.RegularExpressions;


namespace DepoQuick.Domain;

public class Deposit
{
    private const string DepositNameIsNotValidMessage = "El nombre del depósito no es válido";
    private const string DepositWithInvalidAreaMessage = "El área del depósito no es válida";
    private const string DepositWithInvalidSizeMessage = "El tamaño del depósito no es válido";
    
    private const int AdditionalPriceForAirConditioning = 20;
    
    private const int PriceForSmallDeposit = 50;
    private const int PriceForMediumDeposit = 75;
    private const int PriceForBigDeposit = 100;
    
    private const int NumberOfDaysForLongStays = 7;
    private const int NumberOfDaysForVeryLongStays = 14;
    
    private const int InitialDiscount = 0; 
    private const double DiscountForLongStay = 0.05;
    private const double DiscountForVeryLongStay = 0.1;

    private const String SmallSize = "PEQUEÑO"; 
    private const String MediumSize = "MEDIANO";
    private const String BigSize = "GRANDE";   
    
    private const int ReservationAccepted = 1;
    private const int ReservationRejected = -1;
    
    public static readonly char[] PossibleAreas = { 'A', 'B', 'C', 'D', 'E' };

    [Key]
    public int Id { get; set; }

    private String _name;
    
    private Char _area;
    private String _size;
    
    public bool AirConditioning { get; set; }
    public List<Promotion> Promotions { get; set; }  
    public List<Rating> Ratings { get; set; }  
    public List<Reservation> Reservations { get; set; }  
    public List<DateRange> AvailableDates { get; set;}
    
    public string Name
    {
        get => _name;
        set
        {
            ValidateName(value);
            _name = value;
        }
    }

    private void ValidateName(string value)
    {
        bool nameOnlyContainsLetters = Regex.IsMatch(value, @"^[A-Za-z]+$");
        if (!nameOnlyContainsLetters || value == "" || value == null)
        {
            throw new DepositNameIsNotValidException(DepositNameIsNotValidMessage);
        }
    }

    public Char Area
    {
        get => _area;
        set
        {
            ValidateArea(value);
            _area = char.ToUpper(value);
        }
    }
    
    public string Size
    {
        get => _size;
        set
        {
            ValidateSize(value);
            _size = value.ToUpper();
        }
    }
    
    public Deposit()
    {
       AvailableDates = new List<DateRange>();
       Ratings = new List<Rating>();
       Promotions = new List<Promotion>(); 
       Reservations = new List<Reservation>();
    }
    
    public Deposit(String name, Char area, String size, bool airConditioning)
    {
        ValidateName(name);
        ValidateArea(area);
        ValidateSize(size);
        
        Name = name;
        Area = char.ToUpper(area);
        Size = size.ToUpper();
        AirConditioning = airConditioning;
        
        Ratings = new List<Rating>();
        Promotions = new List<Promotion>(); 
        Reservations = new List<Reservation>();
        AvailableDates = new List<DateRange>();
    }

    public int CalculatePrice(int numberOfDays)
    {
        int basePrice = PriceAccordingToSize() * numberOfDays;
        
        int priceWithIncreaseForAirConditioning =
            ApplyIncreaseForAirConditioning(basePrice, numberOfDays);

        int finalPrice = priceWithIncreaseForAirConditioning; 

        double discount = DiscountAccordingToNumberOfDays(numberOfDays);

        finalPrice = ApplyDiscount(finalPrice,
            AddTheDiscountAccordingToNumberOfDaysToPromotions(discount)); 
        
        return finalPrice; 
    }
    
    public bool IsReserved()
    {
        foreach (var reservation in Reservations)
        {
            bool isAccepted = reservation.Status == ReservationAccepted;
            DateRange dateRange = reservation.Date;
                
            if (isAccepted && dateRange.IsDateInRange(DateTime.Now))
            {
                return true; 
            }
        }
        return false;
    }
    
    public bool IsReserved(DateRange dateRange)
    {
        foreach (var reservation in Reservations)
        {
            bool isAccepted = reservation.Status == ReservationAccepted;
            DateRange reservationDateRange = reservation.Date;
            
            if (isAccepted && reservationDateRange.DateRangeIsOverlapping(dateRange))
            {
                return true; 
            }
        }
        return false;
    }
    
    public bool HasUpcomingReservations()
    {
        foreach (var reservation in Reservations)
        {
            bool isAcceptedOrPending = reservation.Status != ReservationRejected;
            DateTime reservationInitialDate = reservation.Date.GetInitialDate();
            
            if (isAcceptedOrPending && reservationInitialDate > DateTime.Now)
            {
                return true; 
            }
        }
        return false;
    }
    
    public double GetAverageRating()
    {
        if (Ratings.Count == 0)
        {
            return 0; 
        }
        
        double sum = 0; 
        
        foreach (var rating in Ratings)
        {
            sum += rating.Stars; 
        }
        
        return sum / Ratings.Count; 
    }
    
    public void AddRating(Rating rating)
    {
        Ratings.Add(rating);
    }
    
    public void AddPromotion(Promotion promotion)
    {
        Promotions.Add(promotion);
    }
    
    public void RemovePromotion(Promotion promotion)
    {
        Promotions.Remove(promotion);
    }
    
    private void ValidateArea(Char area)
    {
        if (!PossibleAreas.Contains(char.ToUpper(area)))
        {
            throw new DepositWithInvalidAreaException(DepositWithInvalidAreaMessage);
        }
    }

    private void ValidateSize(String size)
    {
        List<String> possibleSize = new List<String> {SmallSize,MediumSize,BigSize};
        if (!possibleSize.Contains(size.ToUpper()))
        {
            throw new DepositWithInvalidSizeException(DepositWithInvalidSizeMessage);
        }
    }

    private int PriceAccordingToSize()
    {
        switch (Size)
        {
            case MediumSize:
                return PriceForMediumDeposit;
            case BigSize:
                return PriceForBigDeposit;
            default:
                return PriceForSmallDeposit;
        }
    }

    private int ApplyDiscount(int price, double discount)
    {
        return (int) (price * (1 - discount));
    }

    private double DiscountAccordingToNumberOfDays(int numberOfDays)
    {
        double discount = InitialDiscount; 
        if (numberOfDays >= NumberOfDaysForLongStays && numberOfDays <= NumberOfDaysForVeryLongStays)
        {
            discount =  DiscountForLongStay;
        }
        
        if (numberOfDays > NumberOfDaysForVeryLongStays)
        {
            discount = DiscountForVeryLongStay;
        }
        
        return discount; 
    }

    private int ApplyIncreaseForAirConditioning(int price, int numberOfDays)
    {
        if (AirConditioning)
        {
            price += numberOfDays * AdditionalPriceForAirConditioning;
        }

        return price; 
    }

    private double AddTheDiscountAccordingToNumberOfDaysToPromotions(double discountAccordingToNumberOfDays)
    {
        List<Promotion> listOfPromotion = Promotions;
        double discount = discountAccordingToNumberOfDays; 
        foreach (Promotion promotion in listOfPromotion)
        {
            if (promotion.IsCurrentlyAvailable() 
                && TheSumOfTheDiscountsIsLessThan100(discount, promotion.DiscountRate))
            {
                discount +=  promotion.DiscountRate; 
            }
        }

        return discount; 
    }
    
    private bool TheSumOfTheDiscountsIsLessThan100(double discountOne, double discountTwo)
    {
        return (discountOne+discountTwo) <= 1; 
    }
}