using MapsterMapper;
using Microsoft.EntityFrameworkCore;
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
            services.AddSingleton<System.TimeProvider>(System.TimeProvider.System);
            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication();
            services.AddAuthorization();
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
