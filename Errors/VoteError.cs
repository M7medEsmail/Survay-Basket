using SurvayBacket.Api.Abstractions;

namespace SurvayBacket.Api.Errors
{
    public static class VoteError
    {
        public static readonly Error VoteAlreadyExists = new Error("Vote.PollAlreadyExists", "This user already voted before.",StatusCodes.Status409Conflict);
        public static readonly Error InvalidQuestion = new Error("Vote.InvalidQuestion", "InvalidQuestion.", StatusCodes.Status404NotFound);
    }
}
