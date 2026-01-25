namespace SurvayBacket.Api.Services
{
    public class LinuxOsService : IOperationScoped , IOperationSingleton , IOperationTransient
    {
        
        public string OpetationId { get; }

        public LinuxOsService()
        {
            OpetationId = Guid.NewGuid().ToString()[^4..];
        }
        public string RunApp() => "Running from Linux";

   }
}
