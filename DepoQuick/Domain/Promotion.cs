using System.ComponentModel.DataAnnotations;
using DepoQuick.Exceptions.PromotionExceptions;

namespace DepoQuick.Domain;
public class Promotion
{
    
    [Key]
    public int Id { get; set; }
    
    public DateRange ValidityDate { get; set; }
    
    private String _label;
    private Double _discountRate;
    
    public List<Deposit> Deposits { get; set; }
    
    public String Label
    {
        get => _label;
        set
        {
            ValidateLabel(value);
            _label = value;
        }
    }

    public Double DiscountRate
    {
        get => _discountRate;
        set
        {
            ValidateDiscountRate(value);
            _discountRate = value;
        }
    }
    
    public Promotion()
    {
      
    }
    
    private void ValidateLabel(String label)
    {
        if (LabelIsEmpty(label))
        {
            throw new PromotionWithEmptyLabelException("La etiqueta no debe ser vacia");
        }
        
        if (LabelHasMoreThan20Characters(label))
        {
            throw new PromotionLabelHasMoreThan20CharactersException("La etiqueta no debe ser de largo mayor a 20 caracteres");
        }
    }

    private bool LabelIsEmpty(String label)
    {
        return string.IsNullOrWhiteSpace(label);
    }

    private bool LabelHasMoreThan20Characters(String label)
    {
        return label.Length > 20;
    }
    
    private void ValidateDiscountRate(double percent)
    {
        if (!ItsBetween5And75Percent(percent))
        {
            throw new InvalidPercentageForPromotionException("El porcentaje no es valido, debe estar entre 0.05 y 0.75");
        }
    }

    private bool ItsBetween5And75Percent(double number)
    {
        return number >= 0.05 && number <= 0.75; 
    }
    
    public bool IsCurrentlyAvailable()
    {
        return ValidityDate.GetInitialDate() <= DateTime.Now.AddDays(1) && ValidityDate.GetFinalDate() >= DateTime.Now.AddDays(-1);
    }
}