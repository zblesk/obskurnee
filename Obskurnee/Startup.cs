using AspNetCore.Identity.LiteDB;
using AspNetCore.Identity.LiteDB.Data;
using AspNetCore.Identity.LiteDB.Models;
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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VueCliMiddleware;

using LDM = AspNetCore.Identity.LiteDB;

namespace Obskurnee
{
    public class Startup
    {
        public static readonly SymmetricSecurityKey SecurityKey =
            new SymmetricSecurityKey(
                Encoding.Default.GetBytes("ghf345678oikjhgfde3456789ijbvcdsw6789opkjfdeuijknbvgfdre4567uij"));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Directory.CreateDirectory("images");
            Directory.CreateDirectory("data");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSpaStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "images")),
                RequestPath = "/images"
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                {
                    spa.Options.SourcePath = "ClientApp/";
                }
                else
                {
                    spa.Options.SourcePath = "dist";
                }

                if (env.IsDevelopment())
                {
                    spa.UseVueCli(npmScript: "serve");
                }
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });

            var databaseSingleton = new Database(Log.Logger.ForContext<Database>());

            services.AddSingleton<Database>(databaseSingleton);
            services.AddSingleton<ILiteDbContext>((ILiteDbContext)databaseSingleton);
            services.AddTransient<GoodreadsScraper, GoodreadsScraper>();
            services.AddTransient<PollService, PollService>();
            services.AddSingleton<UserService, UserService>();
            services.AddTransient<BookService>();

            ConfigureAuthAndIdentity(services);
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
                                    IssuerSigningKey = SecurityKey,
                                };
                    // The JwtBearer scheme knows how to extract the token from the Authorization header
                    // but we will need to manually extract it from the query string in the case of requests to the hub
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
                options.Password.RequiredLength = 15;
            })
                .AddRoles<LDM.IdentityRole>()
               .AddUserStore<LiteDbUserStore<Bookworm>>()
               .AddRoleStore<LiteDbRoleStore<LDM.IdentityRole>>()
               .AddSignInManager<SignInManager<Bookworm>>()
               .AddDefaultTokenProviders()
               ;

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim(BookclubClaims.Admin));
            });
        }
    }
}