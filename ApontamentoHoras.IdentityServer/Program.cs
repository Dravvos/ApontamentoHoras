using ApontamentoHoras.IdentityServer.Initializer;
using ApontamentoHoras.IdentityServer.Models;
using ApontamentoHoras.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("ApontamentoHorasConn"))
 );


builder.Services.AddIdentity<ApplicationUser,IdentityRole>()
    .AddEntityFrameworkStores<PostgresContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;

});

builder.Services.AddMemoryCache();

builder.Services.AddAuthorization();

builder.Services.AddScoped<IDBInitializer, DBInitializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using var scope = app.Services.CreateScope();
var initializer = scope.ServiceProvider.GetService<IDBInitializer>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
initializer.Initialize();
app.MapControllers();

app.Run();
