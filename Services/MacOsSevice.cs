namespace SurvayBacket.Api.Services
{
    public class MacOsSevice : IOperationScoped , IOperationSingleton , IOperationTransient
    {
        
     public string OpetationId { get; }

    public MacOsSevice()
    {
        OpetationId = Guid.NewGuid().ToString()[^4..];
    }
    public string RunApp() => "Running from Mac";

    }
}
