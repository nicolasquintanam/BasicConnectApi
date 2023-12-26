using BasicConnectApi.Data;
using Microsoft.EntityFrameworkCore;
using BasicConnectApi.Middleware;
using BasicConnectApi.Filters;
using BasicConnectApi.Models;
using BasicConnectApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));

builder.Services.AddDbContext<ApplicationDbContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);



// Configure JWT authentication
var jwtConfiguration = builder.Configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();
var key = Encoding.ASCII.GetBytes(jwtConfiguration.Secret);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfiguration.Issuer,
            ValidAudience = jwtConfiguration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var jwtService = context.HttpContext.RequestServices.GetRequiredService<IJwtService>();
                var tokenId = context.SecurityToken.Id;
                var revokedTokens = jwtService.TokenIsRevoked(tokenId);
                if (revokedTokens)
                {
                    context.Fail("Revoked Token");
                    context.Response.OnStarting(state =>
                    {
                        var httpContext = (HttpContext)state;
                        var jsonResponse = JsonSerializer.Serialize(new BaseResponse(false, "Invalid token. Please log in again."));
                        httpContext.Response.ContentType = "application/json";
                        httpContext.Response.ContentLength = Encoding.UTF8.GetBytes(jsonResponse).Length;
                        return httpContext.Response.WriteAsync(jsonResponse);
                    }, context.HttpContext);
                }
                return Task.CompletedTask;

            }
        };
    });

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();




builder.Services.AddControllers(
    options =>
    {
        options.Filters.Add<ValidationFilter>();
    }
);
builder.Services.AddScoped<ValidationFilter>();

builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.DocumentFilter<LowercaseDocumentFilter>();
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();


app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
