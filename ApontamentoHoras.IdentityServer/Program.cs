using ApontamentoHoras.IdentityServer.Initializer;
using ApontamentoHoras.IdentityServer.Models;
using ApontamentoHoras.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("ApontamentoHorasConn"))
 );


builder.Services.AddIdentity<ApplicationUser,IdentityRole>()
    .AddEntityFrameworkStores<PostgresContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("AuthToken", out var token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;

});

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddAuthorization();

builder.Services.AddScoped<IDBInitializer, DBInitializer>();
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "X-CSRF-TOKEN";
});

var app = builder.Build();

app.UseAntiforgery();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder =>
    builder.WithOrigins("http://localhost:5173") 
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials());

using var scope = app.Services.CreateScope();
var initializer = scope.ServiceProvider.GetService<IDBInitializer>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
initializer.Initialize();
app.MapControllers();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<PostgresContext>();
context.Database.Migrate();
app.Run();
