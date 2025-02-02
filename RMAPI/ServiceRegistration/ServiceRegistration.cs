using Infrastructure.Repositories;
using RMAPI.ConfigApp;
using RMAPI.Services.Authentication;
using RMAPI.Services.UserService;

namespace RMAPI.ServiceRegistration
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Đăng ký các dịch vụ tại đây
            services.AddScoped(typeof(BaseCommand<>));
            services.AddScoped<BaseQuery>();
            services.AddScoped<ConfigJWT>();
            services.AddScoped<UserAuthentication>();
            services.AddScoped<UserService>();
        }
    }
}
