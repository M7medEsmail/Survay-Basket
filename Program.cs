using Hangfire;
using HangfireBasicAuthenticationFilter;
using Mapster;
    using MapsterMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
using Serilog;
using SurvayBacket.Api;
    using SurvayBacket.Api.Middlewares;
    using SurvayBacket.Api.Persistence;
    using SurvayBacket.Api.Services;
    using System;
    using System.Reflection;

    var builder = WebApplication.CreateBuilder(args);

    #region Add DataBase
    builder.Services.AddDbContextByInject(builder.Configuration);

    //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    //    throw new InvalidOperationException("Connection String, DefaultConnection not found.");
    //builder.Services.AddDbContext<ApplicationDbContext>( options =>
    //    options.UseSqlServer(connectionString));
    #endregion

    #region Adding Services
    builder.Services.AddDependancies();

    builder.Services.AddAuthConfig(builder.Configuration);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddTransient<IOperationTransient, MacOsSevice>();
//builder.Services.AddScoped<IOperationScoped,MacOsSevice>();
//builder.Services.AddSingleton<IOperationSingleton,MacOsSevice>();
//builder.Services.AddScoped<IPollService, PollService>();
#endregion


#region Using Mapster

//var MappingConfig = TypeAdapterConfig.GlobalSettings;
//MappingConfig.Scan(Assembly.GetExecutingAssembly());

//builder.Services.AddSingleton<IMapper>(new Mapper(MappingConfig));
//builder.Services.AddMapster();
#endregion

//builder.Services.AddIdentityApiEndpoints<ApplicationUser>
//    .addentityframeworkStores<ApplicationDbContext>();


builder.Host.UseSerilog((context, configration) =>
{
    configration.ReadFrom.Configuration(context.Configuration);
});

///OutPut cache
builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("Polls", policy =>
    {
        policy.Cache().Expire(TimeSpan.FromSeconds(120)).Tag("OutPutCache");
     
    });
});



var app = builder.Build();

    // Configure the HTTP request pipeline.
    // some of medelware
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseMiddleware<CustomMiddleware>();

    app.UseSerilogRequestLogging();

    app.UseCustomMiddleware();

    app.UseHttpsRedirection();
    
    app.UseHangfireDashboard("/jobs" , new DashboardOptions
    {
        Authorization =
        [
            new HangfireCustomBasicAuthenticationFilter{
                User = builder.Configuration.GetValue<string>("HangfireSettings:Username"),
                Pass = builder.Configuration.GetValue<string>("HangfireSettings:Password")
            }
        ],
        DashboardTitle = "SurvayBacket BackgroundJobs Dashboard",
        //IsReadOnlyFunc = (httpContext) => true                     make it read only not be able to trigger jobs in dashboard
    });

/// configure recurring job to send notification about new polls daily
    var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
    using var scope = scopeFactory.CreateScope();
    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
    RecurringJob.AddOrUpdate(
        "SendNewPollNotificationJob",
        () => notificationService.SendNewPollNotification(null),
        Cron.Daily);

    app.UseCors("MyPolicy");

    app.UseAuthorization();

    app.MapControllers();

    app.UseOutputCache();
        
    app.UseExceptionHandler();

    //app.UseMiddleware<ExceptionHandelMiddleware>();

    app.Run();
