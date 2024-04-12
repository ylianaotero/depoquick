using DepoQuick.Domain.Exceptions.DepositExceptions;

namespace DepoQuick.Domain;

public class Deposit
{
    private const int DEFAULT_ADDIONAL_PRICE_FOR_AIR_CONDITIONING = 20;
    
    private const int DEFAULT_PRICE_FOR_SMALL_DEPOSIT = 50;
    private const int DEFAULT_PRICE_FOR_MEDIUM_DEPOSIT = 75;
    private const int DEFAULT_PRICE_FOR_BIG_DEPOSIT = 100;
    
    private const int DEFAULT_NUMBER_OF_DAYS_FOR_LONG_STAYS = 7;
    private const int DEFAULT_NUMBER_OF_DAYS_FOR_VERY_LONG_STAYS = 14;
    
    private const int DEFAULT_DISCOUNT = 0; 
    private const double DEFAULT_DISCOUNT_FOR_LONG_STAY = 0.05;
    private const double DEFAULT_DISCOUNT_FOR_VERY_LONG_STAY = 0.1;

    private int _id; 
    private Char _area;
    private String _size;
    private bool _airConditioning;
    private bool _reserved;
    private List<Promotion> _promotions; 
    private List<Rating> _ratings;
    
    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public int calculatePrice(int numberOfDays)
    {
        int basePrice = multiply(priceAccordingToSize(), numberOfDays);
        int priceWithDiscountForNumberOfDays = applyDiscount(basePrice, discountAccordingToNumberOfDays(numberOfDays));
        int priceWithIncreaseForAirConditioning =
            applyIncreaseForAirConditioning(priceWithDiscountForNumberOfDays, numberOfDays);
        int finalPrice = priceWithIncreaseForAirConditioning; 
        if (isThereAnySpecialDiscount())
        {
            finalPrice = applyDiscount(priceWithIncreaseForAirConditioning, getAvailableDiscount()); 
        }
        return finalPrice; 
    }

    private double getAvailableDiscount()
    {
        List<Promotion> listOfPromotion = getPromotions();
        double discount = 0; 
        foreach (Promotion promotion in listOfPromotion)
        {
            DateRange dateRange = promotion.ValidityDate;
            if (dateRangeIsWithinToday(dateRange) && Issmaller(discount, promotion.DiscountRate))
            {
                discount =  promotion.DiscountRate; 
            }
            
        }

        return discount; 
    }

    private bool Issmaller(double numberOne, double numberTwo)
    {
        return numberOne < numberTwo; 
    }

    private bool isThereAnySpecialDiscount()
    {
        List<Promotion> listOfPromotion = getPromotions();
        foreach (Promotion promotion in listOfPromotion)
        {
            DateRange dateRange = promotion.ValidityDate;
            if (dateRangeIsWithinToday(dateRange))
            {
                return true; 
            }
            
        }

        return false; 

    }
    

    private bool dateRangeIsWithinToday(DateRange dateRange)
    {
        return dateRange.getInitialDate() <= today() && dateRange.getFinalDate() >= today(); 
    }

    private DateTime today()
    {
        return DateTime.Now.Date; 
    }

    private int applyIncreaseForAirConditioning(int price, int numberOfDays)
    {
        if (_airConditioning)
        {
            price += (multiply(numberOfDays, DEFAULT_ADDIONAL_PRICE_FOR_AIR_CONDITIONING)); 
        }

        return price; 
    }

    private int applyDiscount(int price, double discount)
    {
        return (int) (price * (1 - discount));
    }

    private int multiply(int numberOne, int numberTwo)
    {
        return numberOne * numberTwo; 
    }

    private int priceAccordingToSize()
    {
        switch (_size)
        {
            case "Mediano":
                return DEFAULT_PRICE_FOR_MEDIUM_DEPOSIT;
            case "Grande":
                return DEFAULT_PRICE_FOR_BIG_DEPOSIT;
            default:
                return DEFAULT_PRICE_FOR_SMALL_DEPOSIT;
        }
        
    }
    
    private double discountAccordingToNumberOfDays(int numberOfDays)
    {
        double discount = DEFAULT_DISCOUNT; 
        if (numberOfDays >= DEFAULT_NUMBER_OF_DAYS_FOR_LONG_STAYS && numberOfDays <= DEFAULT_NUMBER_OF_DAYS_FOR_VERY_LONG_STAYS)
        {
            discount =  DEFAULT_DISCOUNT_FOR_LONG_STAY;
        }
        if (numberOfDays > DEFAULT_NUMBER_OF_DAYS_FOR_VERY_LONG_STAYS)
        {
            discount = DEFAULT_DISCOUNT_FOR_VERY_LONG_STAY;
        }
        return discount; 
    }
    
    public void addPromotion(Promotion promotion)
    {
        _promotions.Add(promotion);
    }
    
    public List<Promotion> getPromotions()
    {
        return _promotions; 
    }
    
    public Char Area
    {
        get => _area;
        set
        {
            if(areaIsValid(value))
            {
                _area = value; 
            }
        }
    }
    
    public bool AirConditioning
    {
        get => _airConditioning;
        set => _airConditioning = value;
    }
    
    public bool Reserved
    {
        get => _reserved;
        set => _reserved = value;
    }
    
    public String Size
    {
        get => _size;
        set
        {
            if(sizeIsValid(value))
            {
                _size = value; 
            }
        }
    }

    public void addRating(Rating rating)
    {
        _ratings.Add(rating);
    }
    
    public List<Rating> getRating()
    {
        return _ratings; 
    }

    public Deposit(Char area, String size, bool airConditioning, bool reserved)
    {
        if (depositIsValid(area, size))
        {
            _area = area;
            _reserved = reserved;
            _airConditioning = airConditioning;
            _size = size;
            _ratings = new List<Rating>();
            _promotions = new List<Promotion>(); 
        }
    }

    private bool depositIsValid(Char area, String size)
    {
        if (areaIsValid(area) && sizeIsValid(size))
        {
            return true; 
        }
        return false; 
    }

    private bool areaIsValid(Char area)
    {
        List<Char> possibleAreas = new List<char> { 'A', 'B', 'C', 'D', 'E' };
        if (possibleAreas.Contains(area))
        {
            return true; 
        }
        else
        { 
            throw new DepositWithInvalidAreaException("Area no válida (Debe ser A, B, C, D o E)");
            
        }
    }

    private bool sizeIsValid(String size)
    {
        List<String> possibleSize = new List<String> {"Pequeño","Mediano","Grande"};
        if (possibleSize.Contains(size))
        {
            return true; 
        }
        else
        {
            throw new DepositWithInvalidSizeException("Tamaño no válido (Puede ser Pequeño, Mediano o Grande)");
        }
    }
    
    


}