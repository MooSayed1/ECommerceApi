using Domain.Contracts;
using E_commerceApplication.Extentions;
using E_commerceApplication.Factories;
using E_commerceApplication.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistance.Data.Contexts;
using Persistance.Data.DataSeeding;
using Persistance.Repositories;
using Presintation;
using Services;
using Services.Abstraction.Interfaces;
using Services.MappingProfiles;

namespace E_commerceApplication;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Core Services
        builder.Services.AddCoreServices();
        // Infrastructure Services
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.ConfigureJwt(builder.Configuration);
        // Presentation Services
        builder.Services.AddPresentationServices();
        
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
        });
        

        var app = builder.Build();
        // Initialize the database
        await app.DbSeedingAsync();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<GlobalHandlingMiddleware>();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}