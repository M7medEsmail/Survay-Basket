namespace SurvayBacket.Api.Contracts
{
    public record ChangePasswordRequest(
        string CurrentPassword,
        string NewPassword


        );
}
