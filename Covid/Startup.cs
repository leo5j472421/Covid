using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Covid.Enums;
using Covid.Filter;
using Covid.Repositories;
using Covid.Repositories.Interfaces;
using Covid.Services;
using Covid.Services.Interfaces;
using Newtonsoft.Json.Serialization;

namespace Covid
{
    public class Startup
    {
        private readonly IDbConnectionFactory _connections;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _connections = new DbConnectionFactory(new Dictionary<EnumDbConnection, string>
            {
                {EnumDbConnection.Mail, Configuration.GetConnectionString(nameof(EnumDbConnection.Mail))},
            });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config => { config.UseMemoryStorage(); });
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Covid", Version = "v1"}); });
            services.AddSignalR().AddJsonProtocol();
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials();
                }));

            services.AddSingleton<IMailService, GmailService>();
            services.AddControllersWithViews();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<ILoggerService, LoggerService>();
            services.TryAddSingleton<IMailRepository, MailRepository>();
            services.TryAddSingleton(_connections);
            services.TryAddScoped<RequestLogFilter>();
            services.TryAddScoped<WebExceptionFilter>();
            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireServer();
            // 加入Hangfire控制面板
            app.UseHangfireDashboard(
                pathMatch: "/hangfire",
                options: new DashboardOptions()
                {
                    // 使用自訂的認證過濾器
                    Authorization = new[] {new MyAuthorizeFilter()}
                }
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/", async context => { await context.Response.WriteAsync("Hi This is Covid"); });
                endpoints.MapControllers();
            });
        }
    }

    public class MyAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public MyAuthorizeFilter()
        {
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            return true; // 任何人都可以直接觀看
        }
    }
}