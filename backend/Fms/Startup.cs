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

namespace Fms;

public class Startup
{
    private const string CorsPolicyName = "defaultCorsPolicy"; 
    
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public static void CreateConfiguration(IConfigurationBuilder builder)
    {
        if (!IsSwaggerRun())
            builder.AddUserSecrets(Assembly.GetExecutingAssembly(), false);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        new EnvLoader()
            .IgnoreParserException()
            .Load();
        
        services.AddHttpLogging(o => { });
        services.AddHttpContextAccessor();
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
            {
                policy.WithOrigins("http://fms_frontend:3000", "http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        
        AddDatabase(services);
        AddRepositories(services);
        AddLocalization(services);
        AddServices(services);
        AddControllers(services);
        AddAuthentication(services);
        AddSwagger(services);
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
    
            app.UseHttpLogging();
        }

        // app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(CorsPolicyName);
        app.UseRequestLocalization();
        app.UseMiddleware<PublicErrorHandlerMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        if (!EF.IsDesignTime && !IsSwaggerRun())
            RunMigrations(app.Services);
    }

    private void RunMigrations(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FmsDbContext>();
        dbContext.Database.Migrate();
    }

    private void AddServices(IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IWorkspaceService, WorkspaceService>();
        services.AddScoped<ITransactionCategoryService, TransactionCategoryService>();
        services.AddScoped<ITransactionService, TransactionService>();
    }

    private void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<OrganizationRoleRepository>();
        services.AddScoped<IOrganizationToUserRepository, OrganizationToUserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
        services.AddScoped<WorkspaceRoleRepository>();
        services.AddScoped<WorkspaceKindRepository>();
        services.AddScoped<IWorkspaceToAccountRepository, WorkspaceToAccountRepository>();
        services.AddScoped<ITransactionCategoryRepository, TransactionCategoryRepository>();
        services.AddScoped<TransactionCategoryKindRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
    }

    private void AddDatabase(IServiceCollection services)
    {
        services.AddDbContext<FmsDbContext>(options => 
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
    }

    private void AddLocalization(IServiceCollection services)
    {
        services.AddLocalization(options =>
        {
            options.ResourcesPath = "Localization";
        });
        services.Configure<RequestLocalizationOptions>(options =>
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
    }
    
    private void AddAuthentication(IServiceCollection services)
    {
        services.AddAuthentication(options => 
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
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(Configuration["Secrets:JwtSecret"]!))
                };
            });
    }

    private void AddControllers(IServiceCollection services)
    {
        services.AddControllers()
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
    }

    private void AddSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options => {
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
    }

    private static bool IsSwaggerRun()
    {
        return Environment.GetEnvironmentVariable("SWAGGER_TOFILE") == "true";
    }
}
