using Azure.Storage.Blobs;
using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Services.Admins;
using InternshipProgressTracker.Services.InternshipStreams;
using InternshipProgressTracker.Services.Mentors;
using InternshipProgressTracker.Services.Students;
using InternshipProgressTracker.Services.StudyPlanEntries;
using InternshipProgressTracker.Services.StudyPlans;
using InternshipProgressTracker.Services.Users;
using InternshipProgressTracker.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InternshipProgressTracker
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

            services.AddDbContext<InternshipProgressTrackerDbContext>(options =>
            {
                options.UseSqlServer(isProduction ? 
                    _configuration.GetConnectionString("DefaultConnection") : 
                    _configuration.GetConnectionString("LocalhostConnection"));
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<ITokenGenerator, TokenGenerator>();
            services.AddSingleton<IPhotoManager, PhotoManager>();
            services.AddSingleton(options => new BlobServiceClient(_configuration["BlobStorageConnectionString"]));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IInternshipStreamService, InternshipStreamService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudyPlanService, StudyPlanService>();
            services.AddScoped<IStudyPlanEntryService, StudyPlanEntryService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IMentorService, MentorService>();

            services
                .AddIdentity<User, Role>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<InternshipProgressTrackerDbContext>();

            services
                .AddAuthentication("Bearer")
                .AddJwtBearer("MyBearer", options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["ServiceApiKey"])),
                        };
                    })
                .AddMicrosoftIdentityWebApi(_configuration)
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddMicrosoftGraph(_configuration.GetSection("DownstreamApi"))
                .AddInMemoryTokenCaches()
                ;

            services.AddAuthorization(options =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder(
                        JwtBearerDefaults.AuthenticationScheme, "MyBearer")
                        .RequireAuthenticatedUser()
                        .Build();
                });

            services
                .AddControllers(options =>
                {
                    options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "InternshipProgressTracker API", Version = "v1" });

                options.IncludeXmlComments(Path.Combine(
                        AppContext.BaseDirectory,
                        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer xxxxxxxxxxxxxxxxx')",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS-CONNECTIONSTRING"]);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InternshipProgressTracker v1"));

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SeedDatabase(serviceProvider);
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services
                .BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }

        private void SeedDatabase(IServiceProvider serviceProvider)
        {
            CreateRoles(serviceProvider).Wait();
            CreateAdmin(serviceProvider).Wait();
        }

        /// <summary>
        /// Creates identity roles
        /// </summary>
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var roleNames = new[] { "Admin", "Lead", "Mentor", "Student" };

            foreach (var roleName in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    await roleManager.CreateAsync(new Role { Name = roleName });
                }
            }
        }

        /// <summary>
        /// Creates default admin
        /// </summary>
        private async Task CreateAdmin(IServiceProvider serviceProvider)
        {
            var adminEmail = _configuration["Admin:Email"];
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var user = await userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                var admin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = _configuration["Admin:FirstName"],
                    LastName = _configuration["Admin:LastName"],
                };

                var creationResult = await userManager.CreateAsync(admin, _configuration["Admin:Password"]);

                if (creationResult.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }
            }
        }
    }
}
