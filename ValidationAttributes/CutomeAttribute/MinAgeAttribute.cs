using System.ComponentModel.DataAnnotations;

namespace SurvayBacket.Api.ValidationAttributes.CutomeAttribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MinAgeAttribute(int minAge) : ValidationAttribute
    {
        private readonly int _minAge = minAge;
        
        protected override ValidationResult? IsValid(object? value , ValidationContext validationContext)
        {
            if (value is not null)
            {
                var date =(DateTime)value;
                if (DateTime.Today < date.AddYears(_minAge))
                    return new ValidationResult($" Invalid {validationContext.DisplayName}, Age Should be more than {_minAge} Years Old");
            }
            return ValidationResult.Success;
        }


    }
}
