using DepoQuick.Domain.Exceptions.DepositExceptions;

namespace DepoQuick.Domain;

public class Deposit
{
    
    private static int s_nextId = 0;

    private const int DefaultAddionalPriceForAirConditioning = 20;
    
    private const int DefaultPriceForSmallDeposit = 50;
    private const int DefaultPriceForMediumDeposit = 75;
    private const int DefaultPriceForBigDeposit = 100;
    
    private const int DefaultNumberOfDaysForLongStays = 7;
    private const int DefaultNumberOfDaysForVeryLongStays = 14;
    
    private const int DefaultDiscount = 0; 
    private const double DefaultDiscountForLongStay = 0.05;
    private const double DefaultDiscountForVeryLongStay = 0.1;

    private const String DefaultSmallSize = "PEQUEÑO"; 
    private const String DefaultMediumSize = "MEDIANO";
    private const String DefaultBigSize = "GRANDE";   

    private int _id; 
    private Char _area;
    private String _size;
    private bool _airConditioning;
    private bool _reserved;
    private List<Promotion> _promotions; 
    private List<Rating> _ratings;
    
    
    public Deposit(Char area, String size, bool airConditioning, bool reserved)
    {
        if (DepositIsValid(area, size))
        {
            _id = s_nextId; 
            s_nextId++; 
            _area = char.ToUpper(area);
            _reserved = reserved;
            _airConditioning = airConditioning;
            _size = size.ToUpper();
            _ratings = new List<Rating>();
            _promotions = new List<Promotion>(); 
        }
    }
    
    private bool DepositIsValid(Char area, String size)
    {
        if (!AreaIsValid(area))
        {
            throw new DepositWithInvalidAreaException("Area no válida (Debe ser A, B, C, D o E)");
        }

        if (!SizeIsValid(size))
        {
            throw new DepositWithInvalidSizeException("Tamaño no válido (Puede ser Pequeño, Mediano o Grande)");
        }
        
        return true; 
    }

    private bool AreaIsValid(Char area)
    {
        List<Char> possibleAreas = new List<char> { 'A', 'B', 'C', 'D', 'E' };
        if (possibleAreas.Contains(char.ToUpper(area)))
        {
            return true; 
        }
        return false; 
    }

    private bool SizeIsValid(String size)
    {
        List<String> possibleSize = new List<String> {DefaultSmallSize,DefaultMediumSize,DefaultBigSize};
        if (possibleSize.Contains(size.ToUpper()))
        {
            return true; 
        }
        return false;
    }
    
    public void SetId(int id)
    {
        _id = id; 
    }

    public int GetId()
    {
        return _id; 
    }
    
    public void AddPromotion(Promotion promotion)
    {
        _promotions.Add(promotion);
    }
    
    public List<Promotion> GetPromotions()
    {
        return _promotions; 
    }

    public Char GetArea()
    {
        return _area; 
    }

    public bool GetAirConditioning()
    {
        return _airConditioning; 
    }

    public bool IsReserved()
    {
        return _reserved; 
    }

    public String GetSize()
    {
        return _size; 
    }
    
    public void AddRating(Rating rating)
    {
        _ratings.Add(rating);
    }
    
    public List<Rating> GetRatings()
    {
        return _ratings; 
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
        switch (_size)
        {
            case DefaultMediumSize:
                return DefaultPriceForMediumDeposit;
            case DefaultBigSize:
                return DefaultPriceForBigDeposit;
            default:
                return DefaultPriceForSmallDeposit;
        }
        
    }

    private int ApplyDiscount(int price, double discount)
    {
        return (int) (price * (1 - discount));
    }

    private double DiscountAccordingToNumberOfDays(int numberOfDays)
    {
        double discount = DefaultDiscount; 
        if (numberOfDays >= DefaultNumberOfDaysForLongStays && numberOfDays <= DefaultNumberOfDaysForVeryLongStays)
        {
            discount =  DefaultDiscountForLongStay;
        }
        if (numberOfDays > DefaultNumberOfDaysForVeryLongStays)
        {
            discount = DefaultDiscountForVeryLongStay;
        }
        return discount; 
    }

    private int ApplyIncreaseForAirConditioning(int price, int numberOfDays)
    {
        if (_airConditioning)
        {
            price += (Multiply(numberOfDays, DefaultAddionalPriceForAirConditioning)); 
        }

        return price; 
    }

    private bool DateRangeIsWithinToday(DateRange dateRange)
    {
        return dateRange.GetInitialDate() <= Today() && dateRange.GetFinalDate() >= Today(); 
    }

    private DateTime Today()
    {
        return DateTime.Now.Date; 
    }

    private double AddTheDiscountAccordingToNumberOfDaysToPromotions(double discountAccordingToNumberOfDays)
    {
        List<Promotion> listOfPromotion = GetPromotions();
        double discount = discountAccordingToNumberOfDays; 
        foreach (Promotion promotion in listOfPromotion)
        {
            DateRange dateRange = promotion.GetValidityDate();
            if (DateRangeIsWithinToday(dateRange) && TheSumOfTheDiscountsIsLessThan100(discount, promotion.GetDiscountRate()))
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
    
}