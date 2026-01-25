namespace SurvayBacket.Api.Services
{
    public class WindowsOsService : IOperationScoped, IOperationSingleton, IOperationTransient
    {
        public string OpetationId { get; }

        public WindowsOsService()
        {
            OpetationId = Guid.NewGuid().ToString()[^4..];
        }
        public string RunApp() => "Running from windows";
    }
}
