using Helper.EmailHelper;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Service.Config;
using Service.Service.Authentication;
using Service.Service.UserService;

namespace Service.ServiceRegistration
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Đăng ký interfaces
            services.AddScoped(typeof(IBaseCommand<>));
            services.AddScoped<IBaseQuery>();
            services.AddScoped<IUserAuthentication>();
            services.AddScoped<IUserService>();

            // Đăng ký class
            services.AddScoped<ConfigJWT>();
            services.AddScoped<EmailHelper>();
        }
    }
}
