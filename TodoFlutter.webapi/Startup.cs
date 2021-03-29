using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoFlutter.core.Models;
using TodoFlutter.data;
using TodoFlutter.data.Infrastructure;
using TodoFlutter.data.Infrastructure.Helpers;
using TodoFlutter.webapi.Extensions;
using TodoFlutter.webapi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using Azure.Identity;

namespace TodoFlutter.webapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<ITokenFactory, TokenFactory>();
            services.AddScoped<IToDoData, SqlToDoData>();
            services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();
            services.AddScoped<IJwtFactory, JwtFactory>();
            services.AddScoped<IJwtTokenValidator, JwtTokenValidator>();
            

            services.AddDbContextPool<ToDoDbContext>(options => {
                options.UseSqlServer(Configuration["ConnectionStrings:ToDb"]);
            });

            services.AddDbContext<ToDoDbContext>(options =>
                // options.UseSqlite(
                options.UseSqlServer(
                    Configuration["ConnectionStrings:ToDb"]));


            services.AddRazorPages();

            services.AddDatabaseDeveloperPageExceptionFilter();


            /*
             * For Roles
             *             services.AddDefaultIdentity<AppUser>().AddRoles<Role>()     
                .AddEntityFrameworkStores<ToDoDbContext>();
             */
            services.AddDefaultIdentity<AppUser>()     
                .AddEntityFrameworkStores<ToDoDbContext>();

            //  get auth settings from secrets/config file
            //var authsettingsSeceretKey = Configuration.GetSection("AuthSettings:SecretKey").Value;
            //var authsettingsSeceretKey = Configuration["AuthSettings:SecretKey"];


            //  Config secrects
            var clientSecrects = new SecretClient(new Uri("https://todoflutterkeyvalut.vault.azure.net/"), new DefaultAzureCredential());
            KeyVaultSecret authsettingsSeceretKey = clientSecrects.GetSecret("AuthSettings--SecretKey").Value;
            KeyVaultSecret authsettingsJwtIssuerOptionsIssuer = clientSecrects.GetSecret("JwtIssuerOptions--Issuer").Value;
            KeyVaultSecret authsettingsJwtIssuerOptionsAudience = clientSecrects.GetSecret("JwtIssuerOptions--Audience").Value;
            KeyVaultSecret authsettingsJwtIssuerAudience = clientSecrects.GetSecret("JwtIssuerOptions--Audience").Value;

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authsettingsSeceretKey.ToString()));

            // jwt wire up
            // Get options from app settings
            //  
            //var JwtIssuerOptionsIssuer = Configuration.GetSection("JwtIssuerOptions:Issuer").Value;
            //var JwtIssuerOptionsAudience = Configuration.GetSection("JwtIssuerOptions:Audience").Value;
            //var JwtIssuerAudience = Configuration.GetSection("JwtIssuerOptions:Authority").Value;

            var JwtIssuerOptionsIssuer = authsettingsJwtIssuerOptionsIssuer;
            var JwtIssuerOptionsAudience = authsettingsJwtIssuerOptionsAudience;
            var JwtIssuerAudience = authsettingsJwtIssuerAudience;


            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = JwtIssuerOptionsIssuer.ToString();
                options.Audience = JwtIssuerOptionsAudience.ToString();
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = JwtIssuerOptionsIssuer.ToString(),

                ValidateAudience = true,
                ValidAudience = JwtIssuerOptionsAudience.ToString(),

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                /*IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                {
                    return certificates
                    .Where(x => x.Key.ToUpper() == kid.ToUpper())
                    .Select(x => new X509SecurityKey(x.Value));
                }*/
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = JwtIssuerOptionsIssuer.ToString();
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;

                configureOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // api user claim policy
            //  Only users with the rol claim value of API access can access a protected controller or action.
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });

            // add identity
            var identityBuilder = services.AddIdentityCore<AppUser>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoFlutter.webapi", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });

                //  https://pradeeploganathan.com/rest/add-security-requirements-oas3-swagger-netcore3-1-using-swashbuckle/
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoFlutter.webapi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            //  For JWT must come before: app.UseEndpoints
            app.UseAuth();

            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
            };
            var client = new SecretClient(new Uri("https://todoflutterkeyvalut.vault.azure.net/"), new DefaultAzureCredential(), options);

            //  Test getting secerts
            //KeyVaultSecret secret = client.GetSecret("ConnectionStrings--AppConfig");
            //string secretValue = secret.Value;

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
