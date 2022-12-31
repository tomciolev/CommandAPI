using CommandAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using AutoMapper;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(s =>
{
    s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});

var connectionBuilder = new NpgsqlConnectionStringBuilder();
connectionBuilder.ConnectionString = builder.Configuration["ConnectionStrings:PostgreSqlConnection"];
connectionBuilder.Username = builder.Configuration["UserID"];
connectionBuilder.Password = builder.Configuration["Password"];
builder.Services.AddDbContext<CommandContext>(opts =>
{
    opts.UseNpgsql(connectionBuilder.ConnectionString);
});

builder.Services.AddScoped<ICommandRepository, EFCommandRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapDefaultControllerRoute();

app.Run();

