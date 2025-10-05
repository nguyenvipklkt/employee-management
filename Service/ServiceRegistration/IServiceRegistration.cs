using Helper.EmailHelper;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Service.Config;
using Service.Service.Authentication;
using Service.Service.PermissionService;
using Service.Service.UserService;

namespace Service.ServiceRegistration
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Đăng ký interfaces
            services.AddScoped(typeof(IBaseCommand<>), typeof(BaseCommand<>));
            services.AddScoped<IBaseQuery, BaseQuery>();
            services.AddScoped<IUserAuthentication, UserAuthentication>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPermissionService, PermissionService>();

            // Đăng ký class
            services.AddScoped<ConfigJWT>();
            services.AddScoped<EmailHelper>();
        }
    }
}
