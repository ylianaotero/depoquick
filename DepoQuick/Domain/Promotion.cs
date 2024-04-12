using DepoQuick.Domain.Exceptions.PromotionExceptions;

namespace DepoQuick.Domain;
public class Promotion
{
    private int _id; 
    private String _label;
    private Double _discountRate;
    private DateRange _validityDate;

    public Promotion()
    {
        
    }
    
    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public String Label
    {
        get => _label;
        set
        {
            if(labelIsValid(value))
            {
                _label = value; 
            }
        }
    }
    
    private bool labelIsValid(String label)
    {
        if (labelIsEmpty(label))
        {
            throw new PromotionWithEmptyLabelException("La etiqueta no debe ser vacia");
        }
        else
        {
            if (labelHasMoreThan20Characters(label))
            {
                throw new PromotionLabelHasMoreThan20CharactersException("La etiqueta no debe ser de largo mayor a 20 caracteres");
            }
        }

        return true; 
    }

    private bool labelIsEmpty(String label)
    {
        return string.IsNullOrWhiteSpace(label);
    }

    private bool labelHasMoreThan20Characters(String label)
    {
        return label.Length > 20;
    }
    
    public Double DiscountRate
    {
        get => _discountRate;
        set
        {
            if(percentageIsValid(value))
            {
                _discountRate = value; 
            }
        }
    }

    private bool percentageIsValid(double percent)
    {
        if (itsBetween5And75Percent(percent))
        {
            return true; 
        }
        else
        {
            throw new InvalidPercentageForPromotionException("El porcentaje no es valido, debe estar entre 0.05 y 0.75");
        }
    }

    private bool itsBetween5And75Percent(double number)
    {
        return number >= 0.05 && number <= 0.75; 
    }
    
    public DateRange ValidityDate
    {
        get => _validityDate;
        set => _validityDate = value;
    }
    

}