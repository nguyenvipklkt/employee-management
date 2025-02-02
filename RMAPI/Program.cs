using Helper.NLog;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using RMAPI.ConfigApp;
using RMAPI.ServiceRegistration;
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddAutoMapper(typeof(MapperRegistraion));
builder.Services.RegisterServices();

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

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
