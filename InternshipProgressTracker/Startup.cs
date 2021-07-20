using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Services.Users;
using InternshipProgressTracker.Utils;
using InternshipProgressTracker.Services.InternshipStreams;
using InternshipProgressTracker.Services.Students;
using InternshipProgressTracker.Database;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Reflection;
using InternshipProgressTracker.Settings;
using System.Threading.Tasks;

namespace InternshipProgressTracker
{
    public class Startup
    {
        private readonly InternshipProgressTrackerSecrets _secrets;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _secrets = Configuration.GetSection("InternshipProgressTracker").Get<InternshipProgressTrackerSecrets>();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<InternshipProgressTrackerDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services
                .AddScoped<IUserService, UserService>()
                .AddSingleton<ITokenGenerator, TokenGenerator>();

            services
                .AddScoped<IInternshipStreamService, InternshipStreamService>();

            services
                .AddScoped<IStudentService, StudentService>();

            services
                .AddIdentity<User, IdentityRole<int>>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<InternshipProgressTrackerDbContext>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
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
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secrets.ServiceApiKey)),
                    };
                });
            services.AddAuthorization();

            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "InternshipProgressTracker API", Version = "v1" });

                options.IncludeXmlComments(GetXmlCommentsPath());

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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InternshipProgressTracker v1"));
            }

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

        /// <summary>
        /// Gets path for file with xml comments
        /// </summary>
        private string GetXmlCommentsPath()
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
            return Path.Combine(basePath, fileName);
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
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var roleNames = new [] { "Admin", "Lead", "Mentor", "Student" };

            foreach (var roleName in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
            }
        }

        /// <summary>
        /// Creates default admin
        /// </summary>
        private async Task CreateAdmin(IServiceProvider serviceProvider)
        {
            var adminEmail = Configuration["Admin:Email"];
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var user = await userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                var admin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = Configuration["Admin:FirstName"],
                    LastName = Configuration["Admin:LastName"],
                };

                var creationResult = await userManager.CreateAsync(admin, _secrets.AdminPassword);

                if (creationResult.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }
            }
        }
    }
}
