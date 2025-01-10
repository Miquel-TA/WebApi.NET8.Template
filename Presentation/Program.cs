using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using MyApp.Logic.Implementations.Dependencies; // for ServiceRegistration
using Microsoft.EntityFrameworkCore;
using MyApp.Repository; 
using MyApp.Cross.Models; 
using MyApp.Cross.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add the JSON settings from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

// Check that AdminSettings:Password, JwtSettings:Key, and JwtSettings:Issuer exist
var adminPassword = builder.Configuration["AdminSettings:Password"];
if (string.IsNullOrWhiteSpace(adminPassword))
    throw new Exception("AdminSettings:Password is missing in appsettings.json");

var jwtKey = builder.Configuration["JwtSettings:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new Exception("JwtSettings:Key is missing in appsettings.json");

var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
if (string.IsNullOrWhiteSpace(jwtIssuer))
    throw new Exception("JwtSettings:Issuer is missing in appsettings.json");

// Logging with Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/myapp-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Bearer token",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Setup JWT
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

// Setup logic with an in-memory database:
ServiceRegistration.ConfigureServices_InMemory(builder.Services, builder.Configuration);

var app = builder.Build();

// Use the config-based admin password
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MyAppDbContext>();
    if (!db.Users.Any(u => u.Username == "admin"))
    {
        var user = new User
        {
            Username = "admin",
            PasswordHash = BcryptUtils.HashPassword(adminPassword)
        };
        db.Users.Add(user);
        db.SaveChanges();

        Console.WriteLine($"Default admin user is 'admin' with password from appsettings: {adminPassword}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
