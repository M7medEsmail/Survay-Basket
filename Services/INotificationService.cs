namespace SurvayBacket.Api.Services
{
    public interface INotificationService
    {
        Task SendNewPollNotification(int? poolId = null);
    }
}
