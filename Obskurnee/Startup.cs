using AspNetCore.Identity.LiteDB;
using AspNetCore.Identity.LiteDB.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Obskurnee.Models;
using Obskurnee.Services;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VueCliMiddleware;
using System.Text.Json.Serialization;
using System.Configuration;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.HttpOverrides;

namespace Obskurnee
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(
            IServiceCollection services)
        {
            Config.Current = new Config();
            Directory.CreateDirectory(Config.Current.DataFolder);
            Directory.CreateDirectory(Path.Combine(Config.Current.DataFolder, Config.Current.ImageFolder));

            services.AddControllers()
                .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });

            Configuration.Bind(Config.Current);

            services.AddTransient<Database>();
            services.AddTransient<ILiteDbContext, Database>();
            services.AddSingleton(Config.Current);
            services.AddTransient<GoodreadsScraper>();
            services.AddTransient<PollService>();
            services.AddTransient<UserService>();
            services.AddTransient<BookService>();
            services.AddTransient<RoundManagerService>();
            services.AddTransient<DiscussionService>();
            services.AddTransient<SettingsService>();
            services.AddTransient<NewsletterService>();
            services.AddTransient<RecommendationService>();
            services.AddSingleton<MatrixService>();
            services.AddTransient<ReviewService>();
            services.AddHostedService<FeedFetcherService>();

            switch (Configuration["MailerType"])
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

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new[]
                    {
                        new CultureInfo(Config.Current.GlobalCulture)
                    };

                    options.DefaultRequestCulture = new RequestCulture(culture: Config.Current.GlobalCulture, uiCulture: Config.Current.GlobalCulture);
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;

                    options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
                    {
                        return new ProviderCultureResult(Config.Current.GlobalCulture);
                    }));
                });

            ConfigureAuthAndIdentity(services);
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IHostApplicationLifetime lifetime,
            UserService userService)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSpaStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, Config.Current.DataFolder, Config.Current.ImageFolder)),
                RequestPath = "/images"
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            Log.Information("Setting up for environment {env}", env.EnvironmentName);
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp/";
                if (env.IsDevelopment())
                {
                    spa.UseVueCli(npmScript: "serve");
                }
            });

            userService.ReloadCache();
            lifetime.ApplicationStarted.Register(() => 
                Log.Information("Application started at {@base}", 
                    Config.Current.BaseUrl));
            lifetime.ApplicationStopping.Register(() => Log.Information("Application stopping"));
            lifetime.ApplicationStopped.Register(() => Log.Information("Application stopped"));
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
                });

            services.AddIdentityCore<Bookworm>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = Config.Current.DefaultPasswordMinLength;
                options.ClaimsIdentity.UserIdClaimType = BookclubClaims.UserId;
            })
               .AddUserStore<LiteDbUserStore<Bookworm>>()
               .AddSignInManager<SignInManager<Bookworm>>()
               .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ModOnly", policy => policy.RequireClaim(BookclubClaims.Moderator));
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim(BookclubClaims.Admin));
            });
        }
    }
}