using Infrastructure.Services;
using Infrastructure.Setting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tamkeen.Application.Interfaces;
using Tamkeen.Application.Interfaces.Auth;
using Tamkeen.Application.Interfaces.Ticket_Interface;
using Tamkeen.Domain.Entities;
using Tamkeen.Infrastructure.Data;
using Tamkeen.Infrastructure.Implementation.Auth;
using Tamkeen.Infrastructure.Implementation.Ticket_Implementation;
using Tamkeen.Infrastructure.Services;
using Tamkeen.Infrastructure.Setting;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // ✅ AddIdentityCore بدل AddIdentity عشان متـ override شالـ JWT scheme
        services.AddIdentityCore<AppUser>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSetting>()!;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {

                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = "Tamkeen",
                ValidAudience = "TamkeenUsers",

                IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"])
        ),

                ClockSkew = TimeSpan.Zero,

                // 🔥 أهم تعديل
                RoleClaimType = "role",
                NameClaimType = "sub"
            };
        });

        services.Configure<JwtSetting>(configuration.GetSection("Jwt"));
        services.Configure<EmailSetting>(configuration.GetSection("EmailSetting"));
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IImageService, ImageService>();

        return services;
    }
}