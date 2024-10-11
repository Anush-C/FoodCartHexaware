using FoodCart_Hexaware.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Text;
using FoodCart_Hexaware.Repositories;
using FoodCart_Hexaware.Services;
using Stripe;
using FoodCart_Hexaware.Models;
using Microsoft.Extensions.Logging; // For logging
using Serilog; // Add Serilog namespace

namespace FoodCart_Hexaware
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() // Set minimum log level (Debug)
                .WriteTo.Console() // Optional: log to console
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Log to a file
                .CreateLogger();

            // Use Serilog for logging
            try
            {
                Log.Information("Starting up the application");

                var builder = WebApplication.CreateBuilder(args);

                // Clear existing providers and add Serilog
                builder.Logging.ClearProviders();
                builder.Logging.AddSerilog(); // Use Serilog for logging

                builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

                // Configure JSON options
                builder.Services.AddControllers().AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

                // Register repositories
                builder.Services.AddScoped<IMenuRepository, MenuRepository>();
                builder.Services.AddScoped<ICartRepository, CartRepository>();
                builder.Services.AddScoped<IMenuCategoryRepository, MenuCategoryRepository>();
                builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
                builder.Services.AddScoped<IUserRepository, UserRepository>();
                builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
                builder.Services.AddScoped<IStripeGatewayService, StripeGatewayService>();
                builder.Services.AddScoped<IOrderService, OrderService>();
                builder.Services.AddScoped<IDeliveryAgentService, DeliveryAgentService>();

                // Configure CORS
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowReactApp", builder =>
                    {
                        builder
                            .WithOrigins("http://localhost:3001") // Add your frontend URL here
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials(); // If using authentication cookies, add this
                    });
                });

                // Configure DbContext
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("FoodCartApp")));

                // Configure JWT Authentication
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

                // Configure Swagger
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FoodCart API", Version = "v1" });

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new List<string>()
                        }
                    });
                });

                // Register services
                builder.Services.AddScoped<IOrderConfirmationService, OrderConfirmationService>();
                builder.Services.AddScoped<IEmailService, EmailService>();
                builder.Services.AddScoped<IRestaurantService, RestaurantService>();

                var app = builder.Build();

                // Configure the HTTP request pipeline
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodCart API v1");
                    });
                }

                StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

                app.UseHttpsRedirection();

                // Enable CORS middleware
                app.UseCors("AllowReactApp");

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                // Log a message when the application starts
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Application started");

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush(); // Ensure to flush and close the log
            }
        }
    }
}
