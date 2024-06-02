using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DotEnv.Core;
using Fms.Application;
using Fms.Data;
using Fms.Dtos;
using Fms.Repositories;
using Fms.Repositories.Implementations;
using Fms.Services;
using Fms.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

// TODO: Split this file up

new EnvLoader()
    //.AddEnvFile("../.env")
    .IgnoreParserException()
    .Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(o => { });

if (Environment.GetEnvironmentVariable("SWAGGER_TOFILE") != "true")
{
    builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), false);
}

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<ErrorMessages>>();
    
            return new BadRequestObjectResult(new PublicClientErrorDto
            {
                Description = localizer[Fms.Localization.ErrorMessages.validation_general],
                ValidationErrors = errors
            });
        };
    });

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Localization";
});
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("uk")
    };

    options.DefaultRequestCulture = new RequestCulture("uk");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IOrganizationToUserRepository, OrganizationToUserRepository>();
builder.Services.AddScoped<OrganizationRoleRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "FMS", Version = "v1" });
    options.AddServer(new OpenApiServer
    {
        Url = "http://localhost:3333"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = """
                      JWT Authorization header using the Bearer scheme.
                      
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'
                      """,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
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
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
    
    options.CustomSchemaIds(type => type.Name.EndsWith("Dto") ? type.Name.Replace("Dto", string.Empty) : type.Name);
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddDbContext<FmsDbContext>(options =>
{
    var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
    var name = Environment.GetEnvironmentVariable("POSTGRES_NAME");
    var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
    var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
    var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
    var connection = $"Host={host};Database={name};Username={user};Password={password};Port={port}";

    options.UseLazyLoadingProxies(o => o.IgnoreNonVirtualNavigations());
    if (Environment.GetEnvironmentVariable("SWAGGER_TOFILE") != "true")
    {
        options.UseNpgsql(connection);
    }
    else
    {
        options.UseInMemoryDatabase("InMemoryDatabase");
    }
});

builder.Services.AddAuthentication(options => 
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RequireAudience = false,
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Secrets:JwtSecret"]))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.UseHttpLogging();
}

app.UseHttpsRedirection();
app.UseRequestLocalization();
app.UseMiddleware<PublicErrorHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (!EF.IsDesignTime && Environment.GetEnvironmentVariable("SWAGGER_TOFILE") != "true")
{
    MigrationManager.Migrate(app.Services);
}

app.Run();
