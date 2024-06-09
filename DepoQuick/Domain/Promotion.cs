using System.ComponentModel.DataAnnotations;
using DepoQuick.Exceptions.PromotionExceptions;

namespace DepoQuick.Domain;
public class Promotion
{
    private const string PromotionWithEmptyLabelMessage = "La etiqueta no debe ser vacia";
    private const string PromotionLabelHasMoreThanMaxCharactersMessage = "La etiqueta no debe ser de largo mayor a 20 caracteres";
    private const string InvalidPercentageForPromotionMessage = "El porcentaje no es valido, debe estar entre 0.05 y 0.75";
    
    private const double MinimumDiscountRate = 0.05;
    private const double MaximumDiscountRate = 0.75;
    private const int MaximumLabelLength = 20;
    
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
    
    public Promotion() {}
    
    public bool IsCurrentlyAvailable()
    {
        return ValidityDate.GetInitialDate() <= DateTime.Now.AddDays(1) 
               && ValidityDate.GetFinalDate() >= DateTime.Now.AddDays(-1);
    }
    
    private void ValidateLabel(String label)
    {
        if (LabelIsEmpty(label))
        {
            throw new PromotionWithEmptyLabelException(PromotionWithEmptyLabelMessage);
        }
        
        if (LabelHasMoreThanMaxCharacters(label))
        {
            throw new PromotionLabelHasMoreThan20CharactersException(PromotionLabelHasMoreThanMaxCharactersMessage);
        }
    }

    private bool LabelIsEmpty(String label)
    {
        return string.IsNullOrWhiteSpace(label);
    }

    private bool LabelHasMoreThanMaxCharacters(String label)
    {
        return label.Length > MaximumLabelLength;
    }
    
    private void ValidateDiscountRate(double percent)
    {
        if (!IsBetweenMinAndMaxPercentage(percent))
        {
            throw new InvalidPercentageForPromotionException(InvalidPercentageForPromotionMessage);
        }
    }

    private bool IsBetweenMinAndMaxPercentage(double number)
    {
        return number >= MinimumDiscountRate && number <= MaximumDiscountRate;
    }
}