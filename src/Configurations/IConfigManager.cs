namespace Codraw.Configurations;

public interface IConfigManager
{
    Task<CodrawConfiguration> GetConfigurations();
}
