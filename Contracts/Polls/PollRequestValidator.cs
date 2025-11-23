
namespace SurvayBacket.Api.Contracts.Polls
{
    public class PollRequestValidator :AbstractValidator<PollRequest>
    {
        public PollRequestValidator()
        {
            RuleFor(x => x.Title)
        .NotEmpty()
        .Length(3, 100)
        .WithMessage("{PropertyName} must be min: {MinLength} max: {MaxLength}, you entered {TotalLength}");

            RuleFor(x=>x.Summary)
                .NotEmpty()
                .Length(5 , 1500)
                .WithMessage("{PropertyName} must be max: {MaxLength}, you entered {TotalLength}");

            //RuleFor(x => x.StartAt)
            //    .NotEmpty()
            //    .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            //    .WithMessage("{PropertyName} must be more than today");
            RuleFor(x => x.EndAt)
                .NotEmpty();
            RuleFor(x => x)
                .Must(BeAValidDate)
                .WithName(nameof(PollRequest.EndAt))
                .WithMessage("End date must be greater than or equal to start date.");
        }

        private bool BeAValidDate(PollRequest request)
        {
            return request.EndAt >= request.StartAt;
        }
    }
}
