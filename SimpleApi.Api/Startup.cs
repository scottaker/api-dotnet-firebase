﻿using SimpleApi.API.Infrastructure;

namespace SimpleApi.API;

public class Startup
{

    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        var dependency = new DependencyManager();
        dependency.Services(services, this.Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            //app.UseSwagger();
            //app.UseSwaggerUI();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //});
            app.UseCors(builder =>
            {
                var origins = new[]
                {
                    "http://localhost:5173",
                    "http://127.0.0.1:5173",
                    "http://localhost:4200",
                    "http://127.0.0.1:4200"
                };
                builder
                    .WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

        }
        else
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        // ERROR HANDLING
        app.UseExceptionHandler("/api/error/error-handler");

        app.UseRouting();
        app.UseAuthentication();
        //app.UseHttpsRedirection();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

    }

    

}
