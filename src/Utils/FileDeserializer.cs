using System.Text.Json;

namespace Codraw.Utils
{
    public static class FileDeserializer
    {
        public static async Task<T> DeserializeFromFileAsync<T>(string filePath)
        {
            try
            {
                string jsonString =
                    await File.ReadAllTextAsync(filePath);
                var result = JsonSerializer.Deserialize<T>(jsonString) ?? throw new Exception("File content is null");
                return result;
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException($"The specified file was not found: {filePath}");
            }
            catch (JsonException)
            {
                throw new JsonException(
                    $"Failed to deserialize the file content from {filePath} to the specified type."
                );
            }
        }
    }
}
