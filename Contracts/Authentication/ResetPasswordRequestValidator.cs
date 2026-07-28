namespace SurvayBacket.Api.Contracts.Authentication
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("{PropertyName} must be a valid email address.");

            RuleFor(RuleFor => RuleFor.Code)
                .NotEmpty();    

            RuleFor(x => x.NewPassword)
                .Matches("[A-Z]").WithMessage("{PropertyName} must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("{PropertyName} must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("{PropertyName} must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("{PropertyName} must contain at least one special character.");

        }
    }
}