namespace SurvayBacket.Api.Contracts.Authentication
{
    public class ResendInformationEmailRequestValidator : AbstractValidator<ResendInformationEmailRequest>
    {
        public ResendInformationEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("{PropertyName} must be a valid email address.");

           
        }
    }
}
