using Jaeger;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTracing;
using OpenTracing.Util;
using System;
using System.Globalization;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using GS.Employee.Application.UseCases;
using GS.Employee.Domain.Interfaces.Repository;
using GS.Employee.Infra.CrossCutting.Configuration;
using GS.Employee.Infra.Data;
using GS.Employee.Infra.Data.Repositories;
using GS.Employee.Presentation.API.UseCases;

namespace GS.Employee.Presentation.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders(); // Limpa provedores de log padrão, se necessário
                loggingBuilder.AddConsole();    // Adiciona o provedor de log para console
                loggingBuilder.AddDebug();      // Adiciona o provedor de log para debug
                                                // Você pode adicionar outros provedores de log aqui, como Serilog, NLog, etc.
            });

            // Add Health Check
            services.AddHealthChecks();

            services.AddControllers();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddMediatR(typeof(UserInsertCommandHandler));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            //Repository Interface
            services.AddScoped<IRepositoryUser, RepositoryUser>();
            services.AddScoped<IRepositoryPermission, RepositoryPermission>();
            services.AddScoped<IRepositoryUserPermission, RepositoryUserPermission>();
            services.AddScoped<IRepositoryUserPhone, RepositoryUserPhone>();

            //User
            services.AddTransient<InsertUserPresenter>();
            services.AddTransient<UpdateUserPresenter>();
            services.AddTransient<GetUserPresenter>();
            services.AddTransient<GetByIdUserPresenter>();
            services.AddTransient<DeleteUserPresenter>();

            //Permission
            services.AddTransient<InsertPermissionPresenter>();
            services.AddTransient<UpdatePermissionPresenter>();
            services.AddTransient<GetPermissionPresenter>();
            services.AddTransient<GetByIdPermissionPresenter>();
            services.AddTransient<DeletePermissionPresenter>();

            //Authentication
            services.AddTransient<AuthenticationPresenter>();
            

            services.AddApiVersioning(p =>
            {
                p.DefaultApiVersion = new ApiVersion(1, 0);
                p.ReportApiVersions = true;
                p.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = "'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GS Employee"
                });
            });

            services.AddHttpClient("GS", c =>
            {
                c.BaseAddress = new Uri("http://localhost:5001/api/");
            }).AddHttpMessageHandler<InjectOpenTracingHeaderHandler>();

            var tokenConfigurations = new TokenConfiguration();

            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                    Configuration.GetSection("TokenConfigurations")
                )
                .Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenConfigurations.Issuer,
                    ValidAudience = tokenConfigurations.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });


            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: "GS",
            //                      builder =>
            //                      {
            //                          builder.WithOrigins("http://localhost:5000",
            //                                              "https://localhost:5001",
            //                                              "*");
            //                      });
            //});


            //services.AddCors(options => options.AddDefaultPolicy(builder =>
            //{
            //    builder.AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .SetIsOriginAllowed(origin => true)
            //    .AllowCredentials();
            //}));



            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var serviceName = serviceProvider
                    .GetRequiredService<IWebHostEnvironment>()
                    .ApplicationName;

                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                var tracer = new Tracer.Builder(serviceName)
                    .WithLoggerFactory(loggerFactory)
                    .Build();

                // Allows code that can't use DI to also access the tracer.
                GlobalTracer.Register(tracer);

                return tracer;
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Health check path
            app.UseHealthChecks("/check");

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GS.Employee V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHealthChecks("/status-json",
                new HealthCheckOptions()
                {
                    ResponseWriter = async (context, report) =>
                    {
                        var result = JsonSerializer.Serialize(
                            new
                            {
                                currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                statusApplication = report.Status.ToString(),
                                version = "1.1.3",
                            });

                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });

            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true) // allow any origin
              .AllowCredentials()); // allow credentials

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
