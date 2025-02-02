
using E_Insurance_App.Data;
using E_Insurance_App.Repositories.Implementation;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Implementation;
using E_Insurance_App.Services.Interface;
using E_Insurance_App.Utilities.Implementation;
using E_Insurance_App.Utilities.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace E_Insurance_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddControllers();
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "eInsuranceApp API", Version = "v1" });

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                  {
                    new OpenApiSecurityScheme
                    {
                      Reference = new OpenApiReference
                      {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                      }
                    },
                    new string[]{}
                  }
                });
            });

            // MsSql
            builder.Services.AddDbContext<InsuranceDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IJwtService, JwtService>();

            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAgentRepository, AgentRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IInsurancePlanRepository, InsurancePlanRepository>();
            builder.Services.AddScoped<ISchemeRepository, SchemeRepository>();
            builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
            builder.Services.AddScoped<IPremiumRepository, PremiumRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IAgentService, AgentService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();            
            builder.Services.AddScoped<IInsurancePlanService, InsurancePlanService>();
            builder.Services.AddScoped<ISchemeService, SchemeService>();
            builder.Services.AddScoped<IPolicyService, PolicyService>();
            builder.Services.AddScoped<IPremiumService, PremiumService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();


            //JWT
            //var jwtKey = builder.Configuration.GetSection("Jwt")["Key"];
            var key = builder.Configuration["JWT-Key"];
            var jwtKey = builder.Configuration["Jwt:Key"];

            //var decodedKey = Convert.FromBase64String(jwtKey);
            //var decodedKey = Encoding.ASCII.GetBytes(jwtKey);
            var decodedKey = Encoding.UTF8.GetBytes(jwtKey);

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new Exception("JWT key is missing in the configuration.");
            }
            //var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
            //var key = Encoding.ASCII.GetBytes(jwtKey);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    //ValidateIssuerSigningKey = true,

                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(decodedKey)
                };
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Token failed validation: " + context.Exception);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token validated successfully");
                        return Task.CompletedTask;
                    },
                    //OnChallenge = context =>
                    //{
                    //    context.Response.Headers.Add("WWW-Authenticate", "Bearer error=\"invalid_token\"");
                    //    return Task.CompletedTask;
                    //}
                };
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
        }
    }
}
