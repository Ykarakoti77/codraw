namespace Codraw.Utils;
public static class ConfigurationHelper
{
    public static string GetJwtKey()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var jeyKey = config.GetSection("Jwt").GetSection("Key").Value;
        return jeyKey ?? throw new ArgumentNullException(jeyKey, "jwt Key is null");
    }
}