using Helper.EmailHelper;
using Helper.FileHelper;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Service.Config;
using Service.Service.Authentication;
using Service.Service.DepartmentService;
using Service.Service.PermissionService;
using Service.Service.RoleService;
using Service.Service.TemplateService;
using Service.Service.UserService;
using Service.Service.WarehouseService;

namespace Service.ServiceRegistration
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Đăng ký interfaces
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped(typeof(IBaseCommand<>), typeof(BaseCommand<>));
            services.AddScoped<IBaseQuery, BaseQuery>();
            services.AddScoped<IUserAuthentication, UserAuthentication>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IWarehouseService, WarehouseService>();

            // Đăng ký class
            services.AddScoped<ConfigJWT>();
            services.AddScoped<EmailHelper>();
        }
    }
}
