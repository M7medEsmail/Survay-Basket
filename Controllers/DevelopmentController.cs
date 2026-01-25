using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurvayBacket.Api.Services;

namespace SurvayBacket.Api.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class DevelopmentController : ControllerBase
    {


    #region WithOut DI
    //[HttpGet]
    //public IActionResult Run()
    //{
    //var os = new WindowsOsService(); // High Level module is depend on Low Level (Not match with dependancy inversion)
    //var message = os.RunApp();
    //    return Ok(message);
    //}
    #endregion
    #region Use DI
    //[HttpGet]
    //public IActionResult Run()
    //{
    //    var message = _Os.RunApp(); // Using Dependancy Injection
    //    return Ok(message);
    //}
    #endregion
    #region Service LifeTime
    //private readonly ILogger _logger;
    //private readonly IOperationScoped _operationScoped;
    //private readonly IOperationSingleton _operationSinglton;
    //private readonly IOperationTransient _operationTransient;

    //public DevelopmentController(ILogger<DevelopmentController> logger ,
    //    IOperationScoped operationScoped,
    //    IOperationSingleton operationSinglton, 
    //    IOperationTransient operationTransient)
    //{
    //    _logger = logger;
    //    _operationScoped = operationScoped;
    //    _operationSinglton = operationSinglton;
    //    _operationTransient = operationTransient;
    //}

    //[HttpGet]
    //public IActionResult Get() 
    //{
    //    _logger.LogInformation("Transient{0}", _operationTransient.OpetationId);
    //    _logger.LogWarning("Scoped{0}", _operationScoped.OpetationId);
    //    _logger.LogInformation("Singlton{0}", _operationSinglton.OpetationId);
    //    return Ok();
    //}
    #endregion

}

