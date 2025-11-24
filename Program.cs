    using Mapster;
    using MapsterMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
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


var app = builder.Build();

    // Configure the HTTP request pipeline.
    // some of medelware
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseMiddleware<CustomMiddleware>();

    app.UseCustomMiddleware();

    app.UseHttpsRedirection();

    app.UseCors("MyPolicy");

    app.UseAuthorization();

    app.MapControllers();
    
    app.Run();
