using Obskurnee;
using Serilog.Events;
using Serilog;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using Obskurnee.Models;
using System.Configuration;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Processing;
using Obskurnee.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MECConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;
using SixLabors.ImageSharp;
using Microsoft.AspNetCore.HttpOverrides;
using Obskurnee.Hubs;
using System.Security.Claims;
using zblesk.Helpers.Web;
using Microsoft.AspNetCore.Mvc.Routing;

ConfigureLogging();
Log.Information("Starting up");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR()
    .AddNewtonsoftJsonProtocol(options =>
    {
        options.PayloadSerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.PayloadSerializerSettings.Converters.Add(new StringEnumConverter());
    });

ConfigureServices(builder.Services, builder.Configuration);

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration, sectionName: "Logging")
            .CreateLogger();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSerilogRequestLogging(); // added 


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

Configure(app);


app.Run();


void ConfigureLogging()
{
    // Set this up first to make sure even startup errors are caught. 
    // Will be overridden by Config values later.
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .WriteTo.RollingFile(Path.Join("logs", "events.log"))
        .WriteTo.Console()
        .CreateLogger();
}


void ConfigureServices(
    IServiceCollection services, MECConfigurationManager configuration)
{
    Config.Current = new Config();
    Directory.CreateDirectory(Config.DataFolder);

    services.AddControllers(options =>
    {
        options.Filters.Add(new HttpResponseExceptionFilter());
    })
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling
                = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        });

    configuration.Bind(Config.Current);
    Trace.Assert(
        Config.SupportedLanguages.Contains(Config.Current.DefaultCulture),
        "Unsupported Default Culture");

    ConfigureDI(services, configuration);
    ConfigureLocalization(services);

    services.AddSignalR()
        .AddNewtonsoftJsonProtocol(options =>
        {
            options.PayloadSerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            options.PayloadSerializerSettings.Converters.Add(new StringEnumConverter());
        });
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Obskurnee API", Version = "v1" });
    });
    ConfigureAuthAndIdentity(services);
}

void ConfigureDI(IServiceCollection services, MECConfigurationManager configuration)
{
    services.AddSingleton(Config.Current);
    services.AddTransient<GoodreadsScraper>();
    services.AddTransient<StorygraphScraper>();
    services.AddTransient<PollService>();
    services.AddTransient<BookService>();
    services.AddTransient<RoundManagerService>();
    services.AddTransient<DiscussionService>();
    services.AddTransient<SettingsService>();
    services.AddTransient<NewsletterService>();
    services.AddTransient<RecommendationService>();
    services.AddTransient<MatrixService>();
    services.AddTransient<ReviewService>();
    services.AddTransient<BackupService>();
    services.AddTransient<SearchService>();
    services.AddHostedService<FeedFetcherService>();

#if DEMOMODE
            services.AddTransient<UserServiceBase, DemoUserService>();
#else
    services.AddTransient<UserServiceBase, UserService>();
#endif

    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(
            configuration.GetConnectionString("SqliteConnection")));

#if DEMOMODE
            services.AddTransient<IMailerService, FakeMailerService>();
#else
    switch (Config.Current.MailerType)
    {
        case "mailgun":
            services.AddTransient<IMailerService, MailgunMailerService>();
            break;
        case "log-only":
            services.AddTransient<IMailerService, FakeMailerService>();
            break;
        default:
            throw new ConfigurationErrorsException($"Invalid mailer type: {configuration["MailerType"]}");
    }

    if (Config.Current.EnablePeriodicBackup)
    {
        services.AddHostedService<PeriodicBackupService>();
    }
#endif
}

static void ConfigureLocalization(IServiceCollection services)
{
    services.AddLocalization(options => options.ResourcesPath = "Resources");
    return; //todo
    services.Configure<RequestLocalizationOptions>(
        options =>
        {
            var supportedCultures = Config.SupportedLanguages.Select(l => new CultureInfo(l)).ToList();

            options.DefaultRequestCulture = new RequestCulture(
                culture: Config.Current.DefaultCulture,
                uiCulture: Config.Current.DefaultCulture);
            options.SetDefaultCulture(Config.Current.DefaultCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            options.AddInitialRequestCultureProvider(
                new AcceptLanguageHeaderRequestCultureProvider());
        });
}

static void ConfigureAuthAndIdentity(IServiceCollection services)
{
    services.AddAuthentication(cfg =>
    {
        cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                LifetimeValidator = (before, expires, token, param) =>
                {
                    return expires > DateTime.UtcNow;
                },
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateActor = false,
                ValidateLifetime = true,
                IssuerSigningKey = Config.Current.SecurityKey,
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = ctx =>
                {
                    if (ctx.Request.Query.ContainsKey("access_token"))
                    {
                        ctx.Token = ctx.Request.Query["access_token"];
                    }
                    return Task.CompletedTask;
                }
            };
        });

    services.AddIdentityCore<Bookworm>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = Config.Current.DefaultPasswordMinLength;
        options.ClaimsIdentity.UserIdClaimType = BookclubClaims.UserId;
        options.User.AllowedUserNameCharacters = ""; // all
    })
       .AddRoles<IdentityRole>()
       .AddEntityFrameworkStores<ApplicationDbContext>()
       .AddRoleManager<RoleManager<IdentityRole>>()
       .AddSignInManager<SignInManager<Bookworm>>()

       .AddDefaultTokenProviders();

    services.AddSingleton<IAuthorizationHandler, EditAuthorizationHandler>();
    services.AddAuthorization(options =>
    {
        options.AddPolicy("ModOnly", policy => policy.RequireRole(BookclubRoles.Moderator));
        options.AddPolicy("AdminOnly", policy => policy.RequireRole(BookclubRoles.Admin));
        options.AddPolicy("CanUpdate", policy => policy.RequireClaim(BookclubClaims.Operation, "global.write"));
        options.AddPolicy("EditAuthPolicy", policy => policy.Requirements.Add(new MatchingOwnerRequirement()));
    });
}

void Configure(WebApplication app)
{
    var env = app.Environment;
    var lifetime = app.Lifetime;
    using var configScope = app.Services.CreateScope();
    var userService = configScope.ServiceProvider.GetService<UserServiceBase>();
    var dbContext = configScope.ServiceProvider.GetService<ApplicationDbContext>();
    var roleManager = configScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
    var userManager = configScope.ServiceProvider.GetService<UserManager<Bookworm>>();


    Log.Information("Updating database");
    dbContext.Database.Migrate();

    Log.Information("Setting up for environment {env}", env.EnvironmentName);
    EnsureRoles(roleManager, userManager).Wait();
    app.UseRequestLocalization();
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
    // we do not need AllowCredentials - no cookies are used

    app.UseAuthentication();
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseRouting();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Obskurnee API"));

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseSerilogRequestLogging(opts =>
    {
        opts.EnrichDiagnosticContext = PushSerilogProperties;
    });
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHub<EventHub>("/hubs/events");
    });

    userService.LoadUsernameCache();
    lifetime.ApplicationStarted.Register(OnAppStarted);
    lifetime.ApplicationStopping.Register(OnAppStopping);
    lifetime.ApplicationStopped.Register(OnAppStopped);
}

static void OnAppStarted()
{
#if DEMOMODE
            Log.Warning("Application started at {@base} IN DEMO MODE",
                Config.Current.BaseUrl);
#else
    Log.Information("Application started at {@base}",
        Config.Current.BaseUrl);
#endif

}

static void OnAppStopping()
{
    Log.Information("Application stopping");
}

static void OnAppStopped()
{
    Log.Information("Application stopped");
    Log.CloseAndFlush();
}

static async Task EnsureRoles(RoleManager<IdentityRole> roleManager, UserManager<Bookworm> userManager)
{
    await roleManager.EnsureRoleExists(
               BookclubRoles.Bookworm,
               new Claim(BookclubClaims.Operation, "global.read"),
               new Claim(BookclubClaims.Operation, "global.write"));
    await roleManager.EnsureRoleExists(
               BookclubRoles.Admin);
    await roleManager.EnsureRoleExists(
               BookclubRoles.Moderator);
    await roleManager.EnsureRoleExists(
               BookclubRoles.Bot,
               new Claim(BookclubClaims.Operation, "global.read"));
}

void PushSerilogProperties(IDiagnosticContext diagnosticContext, HttpContext httpContext)
{
    diagnosticContext.Set("AspNetUserName", httpContext?.User?.Identity?.Name);
    diagnosticContext.Set("AspNetUserId", httpContext?.User?.GetUserId());
}