using Codraw.Utils;

namespace Codraw.Configurations;

public class ConfigManager() : IConfigManager
{
    public async Task<CodrawConfiguration> GetConfigurations()
    {
        return await FileDeserializer.DeserializeFromFileAsync<CodrawConfiguration>("appConfigurations/codrawConfigs.json");
    }
}
