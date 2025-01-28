using Microsoft.Extensions.Configuration;

namespace SauceDemoUiBetsson.Utilities;

public static class ConfigurationHelper
{
    public static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }
}