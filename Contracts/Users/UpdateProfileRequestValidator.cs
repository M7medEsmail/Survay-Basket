namespace SurvayBacket.Api.Contracts.Users
{
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator()
        {
          

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(2, 50)
                .WithMessage("{PropertyName} must be min: {MinLength} max: {MaxLength}, you entered {TotalLength}");
            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(2, 50)
                .WithMessage("{PropertyName} must be min: {MinLength} max: {MaxLength}, you entered {TotalLength}");
        }
    }
}
