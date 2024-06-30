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
                options.UseSqlServer(@"Server=.;Database=FoodRecommendationSystem;Trusted_Connection=True;").EnableSensitiveDataLogging());
                services.AddScoped<IRepository<MealName>, MealNameRepository>();
                services.AddScoped<IMealNameService, MealNameService>();
                services.AddScoped<IRepository<Food>, FoodRepository>();
                services.AddScoped<IFoodService, FoodService>();
                services.AddScoped<IRepository<MealMenu>, MealMenuRepository>();
                services.AddScoped<IMealMenuService, MealMenuService>();
                services.AddScoped<IRepository<Meal>, MealRepository>();
                services.AddScoped<IMealService, MealService>();
                services.AddScoped<IRepository<Notification>,  NotificationRepository>();
                services.AddScoped<INotificationService, NotificationService>();
                services.AddScoped<IRepository<Rating>, RatingRepository>();
                services.AddScoped<IRatingService, RatingService>();
                services.AddScoped<IRepository<Review>, ReviewRepository>();
                services.AddScoped<IReviewService, ReviewService>();
                services.AddScoped<IRepository<SummaryRating>, SummaryRatingRepository>();
                services.AddScoped<ISummaryRatingService, SummaryRatingService>();
                services.AddScoped<IRepository<UserNotification>, UserNotificationRepository>();
                services.AddScoped<IUserNotificationService, UserNotificationService>();
                services.AddScoped<IRepository<User>,  UserRepository>();
                services.AddScoped<IAdminHelper, AdminHelper>();
                services.AddScoped<IChefHelper, ChefHelper>();
                services.AddScoped<IEmployeeHelper, EmployeeHelper>();
                services.AddScoped<IFeedbackHelper, FeedbackHelper>();
                services.AddScoped<IRecommendationEngineService, RecommendationEngineService>();
                services.AddScoped<ILoginService, LoginService>();
                services.AddScoped<INotificationHelper, NotificationHelper>();
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IRepository<DiscardedMenu>, DiscardedMenuRepository>();
                services.AddScoped<IDiscardedMenuService, DiscardedMenuService>();
                services.AddScoped<IRecommendationHelper, RecommendationHelper>();
                services.AddScoped<IDiscardedMenuFeedbackService, DiscardedMenuFeedbackService>();
                services.AddScoped<IRepository<DiscardedMenuFeedback>, DiscardedMenuFeedbackRepository>();
                services.AddScoped<IRepository<Profile>, ProfileRepository>();
                services.AddScoped<IProfileService, ProfileService>();
                services.AddSingleton<SocketServer>();
            });
}
