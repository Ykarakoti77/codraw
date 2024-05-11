namespace Codraw.Utils
{
    public static class ConfigurationHelper
    {
        private static readonly IConfiguration _configuration;

        static ConfigurationHelper()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
        }

        public static string GetJwtKey()
        {
            return GetConfigurationValue("Jwt:Key");
        }

        public static string GetGoogleClientId()
        {
            return GetConfigurationValue("GoogleAuth:ClientId");
        }

        public static string GetGoogleClientSecret()
        {
            return GetConfigurationValue("GoogleAuth:ClientSecret");
        }

        private static string GetConfigurationValue(string sectionKey)
        {
            var value = _configuration[sectionKey];
            return value ?? throw new ArgumentNullException(sectionKey, $"{sectionKey} is null");
        }
    }
}
