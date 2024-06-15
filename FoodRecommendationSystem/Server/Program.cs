using DataAcessLayer;
using DataAcessLayer.Repository.IRepository;
using DataAcessLayer.Repository.Repository;
using DataAcessLayer.Service.IService;
using DataAcessLayer.Service.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        var serviceProvider = host.Services;

        var server = host.Services.GetRequiredService<SocketServer>();
        server.Start();

        Console.WriteLine("This is the Server");
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<FoodRecommendationContext>(options =>
                    options.UseSqlServer(@"Server=.;Database=FoodRecommendationSystem;Trusted_Connection=True;"));
                services.AddScoped<IMealNameRepository, MealNameRepository>();
                services.AddScoped<IMealNameService, MealNameService>();
                services.AddSingleton<SocketServer>();
            });
}
