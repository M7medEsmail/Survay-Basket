namespace SurvayBacket.Api.Contracts.Authentication
{
    public class RegisterRequestValidation :AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("{PropertyName} must be a valid email address.");
          
            RuleFor(x => x.Password)
                .Matches("[A-Z]").WithMessage("{PropertyName} must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("{PropertyName} must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("{PropertyName} must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("{PropertyName} must contain at least one special character.");

            RuleFor(x=>x.FirstName)
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
