using EmployeeManagementSystem.Data; // Change this to match your namespace
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Register your services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100MB
});


var tokenKey = builder.Configuration["Jwt:TokenKey"];

Console.WriteLine("🔐 TokenKey = " + tokenKey);
Console.WriteLine("📏 TokenKey byte length = " + Encoding.UTF8.GetBytes(tokenKey ?? "").Length);

if (string.IsNullOrEmpty(tokenKey) || Encoding.UTF8.GetBytes(tokenKey).Length < 32)
    throw new Exception("TokenKey must be at least 32 characters long (256 bits)");

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Role-based Authorization (Optional)
builder.Services.AddAuthorization();

// Register JWTService
builder.Services.AddScoped<JWTService>();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

// 👇 Add this before auth
app.UseCors("AllowLocalhost4200");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();


