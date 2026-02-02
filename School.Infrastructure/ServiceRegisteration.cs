using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using School.Domain.Entities.Identity;
using School.Domain.Options;
using School.Infrastructure.Context;
using System.Text;


namespace School.Infrastructure
{
    public static class ServiceRegisteration
    {
        public static IServiceCollection AddServiceRegisteration(this IServiceCollection services, IConfiguration configuration)
        {
            #region Identity
            //needed 		<FrameworkReference Include="Microsoft.AspNetCore.App" />

            services.AddIdentity<User, Role>(option =>
            {
                // Password settings.
                //checked already by FluentValidation.
                option.Password.RequireDigit = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequireUppercase = true;
                option.Password.RequiredLength = 8;
                option.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.AllowedForNewUsers = true;

                // User settings.
                option.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                option.User.RequireUniqueEmail = false;//added in FluentValidation
                option.SignIn.RequireConfirmedEmail = false;


            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders(); ;
            #endregion


            #region Authentication Service

            //read JWT settings from appsettings.json file and bind it to JwtSettings class
            var JwtSettings = new JwtSettings();
            var CookieSettings = new CookieSettings();
            var EmailSettings = new EmailSettings();
            configuration.GetSection(nameof(JwtSettings)).Bind(JwtSettings);
            configuration.GetSection(nameof(CookieSettings)).Bind(CookieSettings);
            configuration.GetSection(nameof(EmailSettings)).Bind(EmailSettings);



            services.AddSingleton(JwtSettings);
            services.AddSingleton(CookieSettings);
            services.AddSingleton(EmailSettings);


            //Enables access to headers, cookies, claims outside controllers, services or hundler classes!
            services.AddHttpContextAccessor();

            //configure Authentication service to use JWT Bearer Tokens
            // needs Microsoft.AspNetCore.Authentication.JwtBearer

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = JwtSettings.ValidateIssuer,
                   ValidIssuers = new[] { JwtSettings.Issuer },
                   ValidateIssuerSigningKey = JwtSettings.ValidateIssuerSigningKey,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey)),
                   ValidAudience = JwtSettings.Audience,
                   ValidateAudience = JwtSettings.ValidateAudience,
                   ValidateLifetime = JwtSettings.ValidateLifeTime,
               };
           });
            #endregion


            #region Swagger configuration to use JWT Bearer token

            //needs Package Swashbuckle.AspNetCore and Swashbuckle.AspNetCore.Annotations 

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "School Project", Version = "v1" });
                c.EnableAnnotations();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: '12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });
            #endregion



            #region add Authorization Policies
            services.AddAuthorization(option =>
            {
                option.AddPolicy("CreateStudent", policy =>
                {
                    policy.RequireClaim("Create Student", "True");
                });
                option.AddPolicy("EditStudent", policy =>
                {
                    policy.RequireClaim("Edit Student", "True");
                });
                option.AddPolicy("DeleteStudent", policy =>
                {
                    policy.RequireClaim("Delete Student", "True");
                });
            });
            #endregion


            return services;
        }
    }
}
