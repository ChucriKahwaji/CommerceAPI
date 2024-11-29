using log4net.Config;
using log4net;

namespace CommerceAPI
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
            });
        }


        // This method is called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseHttpsRedirection();

            app.UseRouting();

            _logger.Info("Configuring middleware pipeline...");

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
