using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using School.Core;
using School.Core.Filters;
using School.Core.MiddleWare;
using School.Domain.Entities.Identity;
using School.Infrastructure;
using School.Infrastructure.Context;
using School.Infrastructure.Seeders;
using School.Service;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



//Add dbcontext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

#region add Dependencies
builder.Services.AddInfrastructureDependencies()
                .AddServiceDependencies()
                .AddCoreDependencies()
                .AddServiceRegisteration(builder.Configuration);
#endregion


#region Localization Services configuration
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization(opt =>
{
    opt.ResourcesPath = "";
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    List<CultureInfo> supportedCultures = new List<CultureInfo>
    {
            new CultureInfo("en-US"),
            new CultureInfo("ar-EG")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

#endregion

#region AllowCORS
var CORS = "_cors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CORS,
                      policy =>
                      {
                          //policy.WithOrigins("http://example.com");
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.AllowAnyOrigin();
                      });
});

#endregion

#region  register Filters
builder.Services.AddTransient<ValidateAdminRoleFilter>();
builder.Services.AddTransient<ValidateUserRoleFilter>();
#endregion


#region Serilog Configuration
try
{
    Log.Logger = new LoggerConfiguration()
                  .ReadFrom.Configuration(builder.Configuration).CreateLogger();
}
catch (Exception ex)
{
    // Fallback to console-only logging if SQL Server sink fails (e.g. DB not yet created)
    Log.Logger = new LoggerConfiguration()
                  .WriteTo.Console()
                  .CreateLogger();
    Log.Warning(ex, "Failed to configure Serilog from configuration, falling back to console-only logging.");
}
builder.Services.AddSerilog();
#endregion


var app = builder.Build();

#region Apply EF Core Migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}
#endregion

#region Seeding application Default user and Roles 
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    await RoleSeeder.SeedAsync(roleManager);
    await UserSeeder.SeedAsync(userManager);
}
#endregion


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Localization Middleware
var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);
#endregion


#region costume Error Handling Middleware
app.UseMiddleware<ErrorHandlerMiddleware>();
#endregion


if (!app.Environment.IsEnvironment("Development") || 
    string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")))
{
    app.UseHttpsRedirection();
}

#region Apply CORS
app.UseCors(CORS);
#endregion

app.UseStaticFiles();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
