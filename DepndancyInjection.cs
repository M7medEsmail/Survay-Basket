using Hangfire;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SurvayBacket.Api.Authentication;
using SurvayBacket.Api.Errors;
using SurvayBacket.Api.Persistence;
using SurvayBacket.Api.Settings;
using System.Reflection;

namespace SurvayBacket.Api
{
    public static  class DepndancyInjection
    {
        public static IServiceCollection AddDependancies(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy",builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                    //.WithOrigins("specific domain"); // to allow specific domain
                });
            });
            services.AddScoped<IPollService, PollService>();

            var MappingConfig = TypeAdapterConfig.GlobalSettings;
            MappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(MappingConfig));

            services.AddMapster();
            services.AddSwaggerGen();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAuthentication();
            services.AddAuthorization();
            services.AddExceptionHandler < GlobalExceptionHandler>();
            services.AddProblemDetails();


            return services; 
        }
        public static IServiceCollection AddAuthConfig(this IServiceCollection services ,IConfiguration configuration )
        {
            var jwtOptions = configuration.GetSection("Jwt").Get<JwtOption>();
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IResultService, ResultService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IEmailSender, MailService>();

            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            services.AddHttpContextAccessor();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //services.Configure<JwtOption>(configuration.GetSection("Jwt")); // used to map value in Appsettings.json file to JwtOption class
            services.AddOptions<JwtOption>().BindConfiguration(JwtOption.SectionName).ValidateDataAnnotations().ValidateOnStart();

            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
            });
            services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions?.Issuer,
                    ValidAudience = jwtOptions?.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions?.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddBackgroundJobsConfig(configuration);
            return services;
        }
        public static IServiceCollection AddDbContextByInject(this IServiceCollection services , IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection String, DefaultConnection not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection AddBackgroundJobsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"));
            });

            services.AddHangfireServer();
            return services;
        }
    }
}
