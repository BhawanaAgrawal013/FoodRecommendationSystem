using Serilog;

public static class SerilogConfig
{
    private static bool _isConfigured = false;
    
    public static void ConfigureLogger()
    {
        if (!_isConfigured)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("F:\\Bhawana\\Food Recommendation System\\Logs\\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            _isConfigured = true;
        }
    }
     
    public static void Information(string info)
    {
        Log.Information(info);
    }
}
