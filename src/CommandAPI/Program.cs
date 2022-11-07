using CommandAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<CommandContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSqlConnection"]);
});
builder.Services.AddScoped<ICommandRepository, EFCommandRepository>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapDefaultControllerRoute();

app.Run();

