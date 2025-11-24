using SurvayBacket.Api.Abstractions;

namespace SurvayBacket.Api.Errors
{
    public static class PollError
    {
        public static readonly Error PollNotFound = new Error("Poll.NotFound", "No Poll was found with this given id.");
    }
}
