using MvcMovie.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;


//
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//using MvcMovie.Data;
using MvcMovie.Models;
using System;

using Amazon;
using Microsoft.Data.SqlClient;


namespace MvcMovie
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            Environment.SetEnvironmentVariable("AWS_PROFILE", "lab1sanjayProfile");

            builder.Services.AddDbContext<MvcMovieContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("MvcMovieContext")));

            /*
            builder.Services.AddDbContext<MvcUserContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("MvcUserContextLOCAL"))); //MvcUserContextLOCAL or MvcUserContext
            */

            
            builder.Configuration.AddSystemsManager("/MvcMovie", new Amazon.Extensions.NETCore.Setup.AWSOptions { Region = RegionEndpoint.CACentral1 });

            var connectionString = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("MvcUserContext"));
            connectionString.UserID = builder.Configuration["DbUser"];
            connectionString.Password = builder.Configuration["DbPassword"];
            builder.Services.AddDbContext<MvcUserContext>(options => options.UseSqlServer(connectionString.ConnectionString));
            

            var app = builder.Build();


            ////////////////////////
            ///
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }


            //////////////////////



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Users}/{action=Index}/{id?}");

            app.Run();
        }
    }
}