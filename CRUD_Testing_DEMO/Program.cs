using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddScoped<ICountriesService, CountriesService>();
        builder.Services.AddScoped<IPeopleService, PeopleService>();
        builder.Services.AddDbContext<PeopleDbContext>(
            opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllers();

        app.Run();
    }
}