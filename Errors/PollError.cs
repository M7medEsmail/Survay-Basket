using SurvayBacket.Api.Abstractions;

namespace SurvayBacket.Api.Errors
{
    public static class PollError
    {
        public static readonly Error PollNotFound = new Error("Poll.NotFound", "No Poll was found with this given id." , StatusCodes.Status404NotFound);
        public static readonly Error PollAlreadyExists = new Error("Poll.PollAlreadyExists", "This has same poll with same title is exist." ,StatusCodes.Status409Conflict);
    }
}
