using AutoMapper;

namespace DemoCRUD
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<AppDbContext>();
           
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            services.AddScoped<IMapper>(provider =>
            {
                var configuration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<Mapper>();
                });
                return configuration.CreateMapper();
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Product API",
                    Version = "v1",
                    Description = "API to manage products",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Lapphan",
                        Email = "Lapphan@example.com"
                    }
                });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1");
                    c.RoutePrefix = "docs";
                });
            }
            app.UseCors("AllowAll");
            app.UseMiddleware<LoggingMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}