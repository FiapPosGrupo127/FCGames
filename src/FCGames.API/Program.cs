using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using FCGames.API.Filters;
using FCGames.API.Logs;
using FCGames.Application.Authorization;
using FCGames.Application.Dto;
using FCGames.Application.Interfaces;
using FCGames.Application.Services;
using FCGames.Domain.Configuration;
using FCGames.Domain.Entities;
using FCGames.Domain.Enums;
using FCGames.Domain.Interfaces;
using FCGames.Domain.Interfaces.Infraestructure;
using FCGames.Domain.Interfaces.Security;
using FCGames.Domain.Services;
using FCGames.Domain.Services.Security;
using FCGames.Infrastructure.Data;
using FCGames.Infrastructure.Data.Repositories;
using FCGames.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using static FCGames.API.Constants.AppConstants;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

var jwtKeyConfig = builder.Configuration["JWT:Key"];
if (string.IsNullOrEmpty(jwtKeyConfig))
    throw new InvalidOperationException("JWT:Key configuration is missing or empty.");

builder.Services.Configure<TokenConfiguration>(builder.Configuration.GetSection("JWT"));

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtKeyConfig)),
        RequireExpirationTime = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicyWithPermission(Policies.Admin, AccessLevel.Admin)
           .AddPolicyWithPermission(Policies.User, AccessLevel.User)
           .AddPolicyWithPermission(Policies.Guest, AccessLevel.Guest);
}).AddAuthorizationBuilder();

builder.Services.AddControllers(options => options.Filters.Add<UserFilter>()).AddNewtonsoftJson(options =>
{
    var settings = options.SerializerSettings;
    settings.NullValueHandling = NullValueHandling.Ignore;
    settings.FloatFormatHandling = FloatFormatHandling.DefaultValue;
    settings.FloatParseHandling = FloatParseHandling.Double;
    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    settings.DateFormatString = "yyyy-MM-ddTHH:mm:ss";
    settings.Culture = new CultureInfo("en-US");
    settings.Converters.Add(new StringEnumConverter());
    settings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new SnakeCaseNamingStrategy() };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FCGames API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.CustomSchemaIds(type =>
    {
        var namingStrategy = new SnakeCaseNamingStrategy();
        return namingStrategy.GetPropertyName(type.Name, false);
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization Header - utilizado com Bearer Authentication. \r\n\r\n Insira 'Bearer' [espaço] e então seu token na caixa abaixo.\r\n\r\nExemplo: (informar sem as aspas): 'Bearer 1234sdfgsdf' ",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAutoMapper((sp, cfg) =>
{
    cfg.AllowNullDestinationValues = true;
    cfg.AllowNullCollections = true;
    cfg.ConstructServicesUsing(sp.GetService);
}, Assembly.GetAssembly(typeof(BaseModel)));

builder.Logging.ClearProviders();
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.LogTo(message => Debug.WriteLine(message), LogLevel.Information);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddMemoryCache();

#region Repositories

builder.Services.AddScoped<IUserRepository, UserRepository>();

#endregion

#region Services

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();

#endregion

#region Application Services

builder.Services.AddScoped<IUserApplicationService, UserApplicationService>();
builder.Services.AddScoped<ITokenApplicationService, TokenApplicationService>();

#endregion

#region Authorization

builder.Services.AddSingleton<IAuthorizationHandler, RolesAuthorizationHandler>();

#endregion

#region Filters

builder.Services.AddScoped<IAuthorizationFilter, UserFilter>();
builder.Services.AddScoped(x => new UserData());

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FCGames API v1"));
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDBContext>();

    context.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();