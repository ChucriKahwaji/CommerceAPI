﻿using BusinessLogic.Configuration;
using CommerceEntities.Entities;
using DataAccess.Configuration;
using log4net;
using log4net.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        private readonly ILog _logger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Configure log4net
            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            // Initialize the logger for the Startup class
            _logger = LogManager.GetLogger(typeof(Startup));
            _logger.Info("Application is starting...");
        }

        public IConfiguration Configuration { get; }

        // This method is called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<CommerceContext>(options => 
            options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 21))));

            // Configure the Business Logic Services
            services.ConfigureServices();  

            // Configure repositories
            services.ConfigureRepositories();

            services.AddEndpointsApiExplorer();

            // Add Swagger services
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "CommerceAPI",
                    Version = "v1",
                    Description = "A simple API for managing products, orders, and customers."
                });
                // Add Basic Authentication to Swagger
                options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Enter your username and password as 'Basic <Base64(username:password)>'"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[] { }
                    }
                });

            });

            // Register IHttpContextAccessor and CorrelationIdAccessor
            services.AddHttpContextAccessor();
        }


        // This method is called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseMiddleware<Middleware.GlobalExceptionHandlingMiddleware>();
                // Enable Swagger middleware
                app.UseSwagger(c =>
                {

                    c.RouteTemplate = "/swagger/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CommerceAPI v1");
                });
            }
            else
            {
                app.UseMiddleware<Middleware.GlobalExceptionHandlingMiddleware>();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            _logger.Info("Configuring middleware pipeline...");

            // Add the Correlation ID Middleware
            app.UseMiddleware<Middleware.CorrelationIdMiddleware>();

            // Add Basic Authentication Middleware
            app.UseMiddleware<Middleware.BasicAuthenticationMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            _logger.Info("Application has started.");
        }

    }
}
