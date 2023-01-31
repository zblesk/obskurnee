using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Obskurnee.Hubs;
using Obskurnee.Models;
using Obskurnee.Services;
using Serilog;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Claims;
using VueCliMiddleware;
using zblesk.Helpers.Web;

namespace Obskurnee;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration, sectionName: "Logging")
            .CreateLogger();
    }

    public void ConfigureServices(
        IServiceCollection services)
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

        services.AddSpaStaticFiles(configuration => { configuration.RootPath = Path.Combine("ClientApp", "dist"); });

        Configuration.Bind(Config.Current);
        Trace.Assert(
            Config.SupportedLanguages.Contains(Config.Current.DefaultCulture),
            "Unsupported Default Culture");

        ConfigureDI(services);
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

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        IHostApplicationLifetime lifetime,
        UserServiceBase userService,
        ApplicationDbContext dbContext,
        RoleManager<IdentityRole> roleManager,
        UserManager<Bookworm> userManager)
    {
        Log.Information("Updating database");
        dbContext.Database.Migrate();

        Log.Information("Setting up for environment {env}", env.EnvironmentName);
        EnsureRoles(roleManager, userManager).Wait();
        app.UseRequestLocalization();
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        app.UseCors(builder =>
        {
            builder.WithOrigins(Config.Current.BaseUrl, Config.Current.Urls)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });

        app.UseAuthentication();
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseSpaStaticFiles();
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

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "ClientApp/";
            if (env.IsDevelopment())
            {
                spa.UseVueCli(npmScript: "serve");
            }
        });

        userService.LoadUsernameCache();
        lifetime.ApplicationStarted.Register(OnAppStarted);
        lifetime.ApplicationStopping.Register(OnAppStopping);
        lifetime.ApplicationStopped.Register(OnAppStopped);
    }

    private static void OnAppStarted()
    {
#if DEMOMODE
            Log.Warning("Application started at {@base} IN DEMO MODE",
                Config.Current.BaseUrl);
#else
        Log.Information("Application started at {@base}",
            Config.Current.BaseUrl);
#endif

    }

    private static void OnAppStopping()
    {
        Log.Information("Application stopping");
    }

    private static void OnAppStopped()
    {
        Log.Information("Application stopped");
        Log.CloseAndFlush();
    }

    private static void ConfigureAuthAndIdentity(IServiceCollection services)
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

    private static async Task EnsureRoles(RoleManager<IdentityRole> roleManager, UserManager<Bookworm> userManager)
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

        // temp - migration
        foreach (var u in userManager.Users)
        {
            var r = await userManager.GetRolesAsync(u);
            if (r.Contains(BookclubRoles.Bot))
                continue;
            if (!r.Contains(BookclubRoles.Bookworm))
                await userManager.AddToRoleAsync(u, BookclubRoles.Bookworm);
            var cl = await userManager.GetClaimsAsync(u);
            if (cl.Any(cl => cl.Type == "moderator"))
            {
                await userManager.AddToRoleAsync(u, BookclubRoles.Moderator);
                await userManager.RemoveClaimsAsync(u, cl.Where(cl => cl.Type == "moderator"));
            }
            if (cl.Any(cl => cl.Type == "admin"))
            {
                await userManager.AddToRoleAsync(u, BookclubRoles.Admin);
                await userManager.RemoveClaimsAsync(u, cl.Where(cl => cl.Type == "admin"));
            }
        }
    }

    private void ConfigureDI(IServiceCollection services)
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
                Configuration.GetConnectionString("SqliteConnection")));

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
                throw new ConfigurationErrorsException($"Invalid mailer type: {Configuration["MailerType"]}");
        }

        if (Config.Current.EnablePeriodicBackup)
        {
            services.AddHostedService<PeriodicBackupService>();
        }
#endif
    }

    private static void ConfigureLocalization(IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");
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

    public void PushSerilogProperties(IDiagnosticContext diagnosticContext, HttpContext httpContext)
    {
        diagnosticContext.Set("AspNetUserName", httpContext?.User?.Identity?.Name);
        diagnosticContext.Set("AspNetUserId", httpContext?.User?.GetUserId());
    }
}
