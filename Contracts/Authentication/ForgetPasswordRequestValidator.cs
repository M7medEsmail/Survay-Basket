namespace SurvayBacket.Api.Contracts.Authentication
{
    public class ForgetPasswordRequestValidator : AbstractValidator<ForgetPasswordRequest>
    {
        public ForgetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("{PropertyName} must be a valid email address.");
 
        }
    }
}
