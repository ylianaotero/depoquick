using System.ComponentModel.DataAnnotations;
using DepoQuick.Exceptions.DepositExceptions;

namespace DepoQuick.Domain;

public class Deposit
{
    
    private static int s_nextId;

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

    [Key]
    public int Id { get; }
    
    private Char _area;
    private String _size;
    
    public bool AirConditioning { get; set; }
    public List<Promotion> Promotions { get; }  
    public List<Rating> Ratings { get; }  
    public List<Reservation> Reservations { get; }  
    
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
        Id = s_nextId; 
        s_nextId++; 
    }
    
    public Deposit(Char area, String size, bool airConditioning)
    {
        ValidateArea(area);
        ValidateSize(size);
        
        Id = s_nextId; 
        s_nextId++; 
        
        Area = char.ToUpper(area);
        Size = size.ToUpper();
        AirConditioning = airConditioning;
        
        Ratings = new List<Rating>();
        Promotions = new List<Promotion>(); 
        Reservations = new List<Reservation>();
    }

    private void ValidateArea(Char area)
    {
        List<Char> possibleAreas = new List<char> { 'A', 'B', 'C', 'D', 'E' };
        if (!possibleAreas.Contains(char.ToUpper(area)))
        {
            throw new DepositWithInvalidAreaException("Area no válida (Debe ser A, B, C, D o E)");
        }
    }

    private void ValidateSize(String size)
    {
        List<String> possibleSize = new List<String> {SmallSize,MediumSize,BigSize};
        if (!possibleSize.Contains(size.ToUpper()))
        {
            throw new DepositWithInvalidSizeException("Tamaño no válido (Puede ser Pequeño, Mediano o Grande)");
        }
    }
    
    public void AddPromotion(Promotion promotion)
    {
        Promotions.Add(promotion);
    }
    
    public void RemovePromotion(Promotion promotion)
    {
        Promotions.Remove(promotion);
    }

    public bool IsReserved()
    {
        foreach (var reservation in Reservations)
        {
            bool isAccepted = reservation.GetState() == 1;
            DateRange dateRange = reservation.GetDateRange();
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
            bool isAccepted = reservation.GetState() == 1;
            DateRange reservationDateRange = reservation.GetDateRange();
            
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
            bool isAcceptedOrPending = reservation.GetState() != -1;
            DateTime reservationInitialDate = reservation.GetDateRange().GetInitialDate();
            
            if (isAcceptedOrPending && reservationInitialDate > DateTime.Now)
            {
                return true; 
            }
        }
        return false;
    }
    
    public void AddRating(Rating rating)
    {
        Ratings.Add(rating);
    }

    public int CalculatePrice(int numberOfDays)
    {
        int basePrice = Multiply(PriceAccordingToSize(), numberOfDays);
        
        int priceWithIncreaseForAirConditioning =
            ApplyIncreaseForAirConditioning(basePrice, numberOfDays);

        int finalPrice = priceWithIncreaseForAirConditioning; 

        double discount = DiscountAccordingToNumberOfDays(numberOfDays);

        finalPrice = ApplyDiscount(finalPrice,
            AddTheDiscountAccordingToNumberOfDaysToPromotions(discount)); 
        
        return finalPrice; 
    }

    private int Multiply(int numberOne, int numberTwo)
    {
        return numberOne * numberTwo; 
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
            price += (Multiply(numberOfDays, AdditionalPriceForAirConditioning)); 
        }

        return price; 
    }

    private double AddTheDiscountAccordingToNumberOfDaysToPromotions(double discountAccordingToNumberOfDays)
    {
        List<Promotion> listOfPromotion = Promotions;
        double discount = discountAccordingToNumberOfDays; 
        foreach (Promotion promotion in listOfPromotion)
        {
            if (promotion.IsCurrentlyAvailable() && TheSumOfTheDiscountsIsLessThan100(discount, promotion.GetDiscountRate()))
            {
                discount +=  promotion.GetDiscountRate(); 
            }
        }

        return discount; 
    }
    
    private bool TheSumOfTheDiscountsIsLessThan100(double discountOne, double discountTwo)

    {
        return (discountOne+discountTwo) <= 1; 
    }
    
    public void AddReservation(Reservation reservation)
    {
        Reservations.Add(reservation);
    }
    
    public void RemoveReservation(Reservation reservation)
    {
        Reservations.Remove(reservation);
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
            sum += rating.GetStars(); 
        }
        return sum / Ratings.Count; 
    }
}