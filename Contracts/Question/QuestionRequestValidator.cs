namespace SurvayBacket.Api.Contracts.Question
{
    public class QuestionRequestValidator :AbstractValidator<QuestionRequest>   
    {
        public QuestionRequestValidator()
        {
            RuleFor(x => x.Content).NotEmpty().Length(5, 1000);

            RuleFor(x => x.Answers)
                .NotNull();

           
            RuleFor(x=>x.Answers)
                .NotEmpty()
                .Must(answers => answers.Count > 1)
                .WithMessage("A question must have at least two answers.")
                .When(x=>x.Answers != null);


            RuleFor(x=>x.Answers)
                .Must(answers => answers.Distinct().Count() == answers.Count)
                .WithMessage("Answers must be unique.")
                .When(x => x.Answers != null);

        }

    }
}
