using Coffee.API.Command.Coffee;
using Coffee.API.Command.Rating;
using Coffee.API.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Coffee.API.Repositories;
using Coffee.API.Validators;

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Coffee.API
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
            services.AddControllers();
            
            services.AddMediatR(typeof(Startup));

            services.AddSwaggerGen();

            services.AddSingleton(Configuration);
            
            services.AddTransient<ICoffeeRepository, CoffeeRepository>();
            services.AddTransient<ICoffeeRatingRepository, CoffeeRatingRepository>();

            services.AddTransient<ISqlServerConnectionProvider, SqlServerConnectionProvider>();
            
            services.AddTransient<IValidator<CreateCoffeeCommand>, CreateCoffeeCommandValidator>();
            services.AddTransient<IValidator<UpdateCoffeeCommand>, UpdateCoffeeCommandValidator>();
            services.AddTransient<IValidator<CreateCoffeeRatingCommand>, CreateCoffeeRatingCommandValidator>();
            services.AddTransient<IValidator<UpdateCoffeeRatingCommand>, UpdateCoffeeRatingCommandValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Coffee Api V1");
                });
                
                app.UseDeveloperExceptionPage();
            }

            if (env.IsProduction())
            {
                // return generic error json if in production and log to backend
                app.UseExceptionHandler(a => a.Run(async context =>
                {
                    var result = JsonConvert.SerializeObject(new { error = Constants.ErrorMessages.AnErrorOccurred });
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result);
                }));
            }
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
