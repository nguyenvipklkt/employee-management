using Helper.EmailHelper;
using Helper.FileHelper;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Service.Config;
using Service.Service.Authentication;
using Service.Service.CustomerService;
using Service.Service.UserService;

namespace Service.ServiceRegistration
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Đăng ký interfaces
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped(typeof(IBaseCommand<>), typeof(BaseCommand<>));
            services.AddScoped<IUserAuthentication, UserAuthentication>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICustomerService, CustomerService>();

            // Đăng ký class
            services.AddScoped<ConfigJWT>();
            services.AddScoped<EmailHelper>();
        }
    }
}
