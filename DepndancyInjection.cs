using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SurvayBacket.Api.Authentication;
using SurvayBacket.Api.Persistence;
using System.Reflection;

namespace SurvayBacket.Api
{
    public static  class DepndancyInjection
    {
        public static IServiceCollection AddDependancies(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddScoped<IPollService, PollService>();

            var MappingConfig = TypeAdapterConfig.GlobalSettings;
            MappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(MappingConfig));

            services.AddMapster();
            services.AddSwaggerGen();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAuthentication();
            services.AddAuthorization();
            return services; 
        }
        public static IServiceCollection AddAuthConfig(this IServiceCollection services ,IConfiguration configuration )
        {
            var jwtOptions = configuration.GetSection("Jwt").Get<JwtOption>();
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.Configure<JwtOption>(configuration.GetSection("Jwt")); // used to map value in Appsettings.json file to JwtOption class
            services.AddOptions<JwtOption>().BindConfiguration(JwtOption.SectionName).ValidateDataAnnotations().ValidateOnStart();


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
    }
}
