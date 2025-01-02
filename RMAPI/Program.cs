using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RMAPI.ConfigApp;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigApp.GetConfigSetting(builder);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//setup db
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(ConfigApp.DBConnection));
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(ConfigApp.DBConnection));

builder.Services.AddScoped(typeof(BaseCommand<>));
builder.Services.AddScoped<BaseQuery>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
