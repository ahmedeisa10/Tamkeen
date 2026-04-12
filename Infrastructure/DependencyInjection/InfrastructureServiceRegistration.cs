using Infrastructure.Services;
using Infrastructure.Setting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tamkeen.Application.Interfaces.Auth;
using Tamkeen.Application.Interfaces.Ticket_Interface;
using Tamkeen.Application.Mapping;
using Tamkeen.Domain.Entities;
using Tamkeen.Infrastructure.Data;
using Tamkeen.Infrastructure.Implementation.Auth;
using Tamkeen.Infrastructure.Implementation.Ticket_Implementation;
using Tamkeen.Infrastructure.Services;
using Tamkeen.Infrastructure.Setting;

namespace Tamkeen.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Identity
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            var jwtKey = configuration.GetSection("Jwt").Get<JwtSetting>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey.Key))
                };
            });
            // Services
            services.Configure<JwtSetting>(configuration.GetSection("Jwt"));
            services.Configure<EmailSetting>(configuration.GetSection("EmailSetting"));
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IImageService, ImageService>();

            //// Repositories
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }

}
