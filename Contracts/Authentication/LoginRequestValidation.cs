namespace SurvayBacket.Api.Contracts.Authentication
{
    public class LoginRequestValidation:AbstractValidator<LoginRequest>
    {
        public LoginRequestValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("{PropertyName} must be a valid email address.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("{PropertyName} must be at least {MinLength} characters long.");
        }
    }
}
