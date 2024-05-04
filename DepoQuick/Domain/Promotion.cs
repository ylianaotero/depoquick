using DepoQuick.Domain.Exceptions.PromotionExceptions;

namespace DepoQuick.Domain;
public class Promotion
{
    private static int s_lastId = 0;
    
    private int _id;
    
    private String _label;
    private Double _discountRate;
    private DateRange _validityDate;
    
    private List<Deposit> _deposits;
    
    public Promotion()
    {
        _id = s_lastId++;
        _deposits = new List<Deposit>();
    }

    public void SetLabel(String label)
    {
        if(LabelIsValid(label))
        {
            _label = label; 
        }
    }
    
    private bool LabelIsValid(String label)
    {
        if (LabelIsEmpty(label))
        {
            throw new PromotionWithEmptyLabelException("La etiqueta no debe ser vacia");
        }
        else
        {
            if (LabelHasMoreThan20Characters(label))
            {
                throw new PromotionLabelHasMoreThan20CharactersException("La etiqueta no debe ser de largo mayor a 20 caracteres");
            }
        }

        return true; 
    }

    private bool LabelIsEmpty(String label)
    {
        return string.IsNullOrWhiteSpace(label);
    }

    private bool LabelHasMoreThan20Characters(String label)
    {
        return label.Length > 20;
    }
    
    public String GetLabel()
    {
        return _label; 
    }

    public void SetDiscountRate(Double discountRate)
    {
        if(PercentageIsValid(discountRate))
        {
            _discountRate = discountRate; 
        }
    }
    

    private bool PercentageIsValid(double percent)
    {
        if (ItsBetween5And75Percent(percent))
        {
            return true; 
        }
        else
        {
            throw new InvalidPercentageForPromotionException("El porcentaje no es valido, debe estar entre 0.05 y 0.75");
        }
    }

    private bool ItsBetween5And75Percent(double number)
    {
        return number >= 0.05 && number <= 0.75; 
    }

    public Double GetDiscountRate()
    {
        return _discountRate; 
    }

    public void SetValidityDate(DateRange validityDate)
    {
        _validityDate = validityDate; 
    }

    public DateRange GetValidityDate()
    {
        return _validityDate; 
    }
    
    public int GetId()
    {
        return _id;
    }
    
    public void AddDeposit(Deposit deposit)
    {
        _deposits.Add(deposit);
    }
    
    public void RemoveDeposit(Deposit deposit)
    {
        _deposits.Remove(deposit);
    }
    
    public List<Deposit> GetDeposits()
    {
        return _deposits; 
    }
}