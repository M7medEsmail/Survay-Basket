using SurvayBacket.Api.Abstractions;

namespace SurvayBacket.Api.Errors
{
    public static class QuestionError
    {
        public static readonly Error QuestionNotFound = new Error("Question.NotFound", "No Question was found with this given id.");
        public static readonly Error QuestionAlreadyExists = new Error("Question.QuestionAlreadyExists", "This has same Question with same Content is exist.");
    }
}
