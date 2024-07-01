using DataAcessLayer;
using DataAcessLayer.Entity;
using DataAcessLayer.Helpers;
using DataAcessLayer.Helpers.IHelpers;
using DataAcessLayer.Repository.IRepository;
using DataAcessLayer.Repository.Repository;
using DataAcessLayer.Service.IService;
using DataAcessLayer.Service.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.RequestHandlers;

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
            ConfigureDatabase(services);
            ConfigureRepositories(services);
            ConfigureServices(services);
            ConfigureHelpers(services);
            ConfigureRequestHandlers(services);

            services.AddSingleton<SocketServer>();
        });

    private static void ConfigureDatabase(IServiceCollection services)
    {
        services.AddDbContext<FoodRecommendationContext>(options =>
            options.UseSqlServer(@"Server=.;Database=FoodRecommendationSystem;Trusted_Connection=True;")
                   .EnableSensitiveDataLogging());
    }

    private static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddScoped<IRepository<MealName>, MealNameRepository>();
        services.AddScoped<IRepository<Food>, FoodRepository>();
        services.AddScoped<IRepository<MealMenu>, MealMenuRepository>();
        services.AddScoped<IRepository<Meal>, MealRepository>();
        services.AddScoped<IRepository<Notification>, NotificationRepository>();
        services.AddScoped<IRepository<Rating>, RatingRepository>();
        services.AddScoped<IRepository<Review>, ReviewRepository>();
        services.AddScoped<IRepository<SummaryRating>, SummaryRatingRepository>();
        services.AddScoped<IRepository<UserNotification>, UserNotificationRepository>();
        services.AddScoped<IRepository<User>, UserRepository>();
        services.AddScoped<IRepository<DiscardedMenu>, DiscardedMenuRepository>();
        services.AddScoped<IRepository<DiscardedMenuFeedback>, DiscardedMenuFeedbackRepository>();
        services.AddScoped<IRepository<Profile>, ProfileRepository>();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IMealNameService, MealNameService>();
        services.AddScoped<IFoodService, FoodService>();
        services.AddScoped<IMealMenuService, MealMenuService>();
        services.AddScoped<IMealService, MealService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<ISummaryRatingService, SummaryRatingService>();
        services.AddScoped<IUserNotificationService, UserNotificationService>();
        services.AddScoped<IRecommendationEngineService, RecommendationEngineService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDiscardedMenuService, DiscardedMenuService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IDiscardedMenuFeedbackService, DiscardedMenuFeedbackService>();
    }

    private static void ConfigureHelpers(IServiceCollection services)
    {
        services.AddScoped<IAdminHelper, AdminHelper>();
        services.AddScoped<IChefHelper, ChefHelper>();
        services.AddScoped<IEmployeeHelper, EmployeeHelper>();
        services.AddScoped<IFeedbackHelper, FeedbackHelper>();
        services.AddScoped<INotificationHelper, NotificationHelper>();
        services.AddScoped<IRecommendationHelper, RecommendationHelper>();
        services.AddScoped<ILoginHelper, LoginHelper>();
    }

    private static void ConfigureRequestHandlers(IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<LoginRequestHandler>, LoginRequestHandler>();
        services.AddScoped<IRequestHandler<DiscardedMenuRequestHandler>, DiscardedMenuRequestHandler>();
        services.AddScoped<IRequestHandler<FeedbackRequestHandler>, FeedbackRequestHandler>();
        services.AddScoped<IRequestHandler<MealMenuRequestHandler>, MealMenuRequestHandler>();
        services.AddScoped<IRequestHandler<MenuRequestHandler>, MenuRequestHandler>();
        services.AddScoped<IRequestHandler<NotificationRequestHandler>, NotificationRequestHandler>();
    }

}
