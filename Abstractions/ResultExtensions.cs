namespace SurvayBacket.Api.Abstractions
{
    public static class ResultExtensions
    {
        public static ObjectResult ToProblem(this Result result )
        {
            if(result.IsSuccess)
                throw new InvalidOperationException("Success Result can't be converted to ProblemDetails");

            var problem = Results.Problem(statusCode  : result.Error.StatusCode);
            var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails)).GetValue(problem) as ProblemDetails;
            problemDetails!.Extensions = new Dictionary<string, object>
                {
                    { 
                    "Errors", new []{result.Error }
                    },
                };

            return new ObjectResult(problemDetails);
        }
    }
}
    