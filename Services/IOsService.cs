namespace SurvayBacket.Api.Services
{
    public interface IOsService
    {
        string RunApp();
        public string OpetationId { get; }
    }

    public interface IOperationScoped : IOsService;
    public interface IOperationSingleton : IOsService;
    public interface IOperationTransient : IOsService;
}
