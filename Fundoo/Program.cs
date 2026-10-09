using BusinessLayer.Service;
using BusinessLayer.Interface;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using RepositoryLayer.Context;

using AuthenticationService.Interface;
using AuthenticationService.Service;

using MessageService.Interface;
using MessageService.Service;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using System.Text;

namespace Fundoo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter JWT token"
                    });

                options.AddSecurityRequirement(document =>
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference(
                            "Bearer", document)] = []
                    });
            });

            builder.Services.AddScoped<IUserBL, UserBL>();
            builder.Services.AddScoped<IUserRL, UserRL>();

            builder.Services.AddScoped<IJwtService, JwtService>();

            builder.Services.AddScoped<IMessageService, EmailService>();

            builder.Services.AddDbContext<FundooContext>(options =>
            {
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString(
                        "FundooConnection"));
            });

            builder.Services
                .AddAuthentication(
                    JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer =
                                builder.Configuration["Jwt:Issuer"],

                            ValidAudience =
                                builder.Configuration["Jwt:Audience"],

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        builder.Configuration["Jwt:Key"]!))
                        };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}