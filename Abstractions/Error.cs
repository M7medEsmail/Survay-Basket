namespace SurvayBacket.Api.Abstractions
{
    public record Error(string Code , string Description , int? StatusCode)
    {
        public static Error None = new Error(string.Empty, string.Empty , null);
    }
}
