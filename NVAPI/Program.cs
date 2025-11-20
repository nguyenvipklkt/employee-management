using CoreValidation.Requests.Authentication;
using FluentValidation;
using Helper.EmailHelper;
using Helper.NLog;
using Infrastructure.Context;
using Infrastructure.Seeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using Object.Setting;
using NVAPI.ConfigApp;
using Service.ServiceRegistration;
using Service.Worker;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigApp.GetConfigSetting(builder);

var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidation>();

//setup logger
var baseDirectory = AppContext.BaseDirectory;
var parentDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent;
if (parentDirectory == null)
{
    throw new InvalidOperationException("Parent directory structure is not valid.");
}
string configPath = Path.Combine(parentDirectory.FullName, "ConfigLog", "nlog.config");

string fixedPath = configPath.Replace("\\", "/");
LogManager.Setup().LoadConfigurationFromFile(fixedPath);
BaseNLog.logger.Info($"Run on Platform [{Environment.OSVersion.Platform}] - Debug version");

//setup db
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(ConfigApp.DBConnection));
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(ConfigApp.DBConnection));

// load error definitions to memory
var errorService = new ErrorMem(new SqlConnection(ConfigApp.DBConnection));
errorService.LoadErrorsToMemory();

// Auto Mapper Configurations
builder.Services.AddAutoMapper(typeof(MapperRegistraion));

// register services
builder.Services.RegisterServices();

// Email configuration
builder.Services.Configure<EmailSetting>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailHelper>();

// JWT configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token like this: Bearer {your_token_here}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securityScheme,
            new string[] {}
        }
    });
});

//add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Seed functions from Excel
var env = app.Environment;
FunctionSeeder.SeedFunctionsFromExcel(new SqlConnection(ConfigApp.DBConnection), Path.Combine(env.WebRootPath, "functions", "Functions.xlsx"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = string.Empty; // Set to empty string to serve Swagger UI at the root
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

// Redirect root URL to Swagger UI
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.Run();
