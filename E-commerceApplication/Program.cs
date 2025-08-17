using Domain.Contracts;
using E_commerceApplication.Extentions;
using Microsoft.EntityFrameworkCore;
using Persistance.Data.Contexts;
using Persistance.Data.DataSeeding;
using Persistance.Repositories;
using Presintation;
using Services;
using Services.Abstraction.Interfaces;
using Services.MappingProfiles;

namespace E_commerceApplication;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Core Services
        builder.Services.AddCoreServices();
        // Presentation Services
        builder.Services.AddPresentationServices();
        // Infrastructure Services
        builder.Services.AddInfrastructureServices(builder.Configuration);
        

        var app = builder.Build();
        // Initialize the database
        await app.DbSeedingAsync();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();

    }
}