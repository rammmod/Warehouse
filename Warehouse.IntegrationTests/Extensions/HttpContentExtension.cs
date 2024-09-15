using Newtonsoft.Json;

namespace Warehouse.IntegrationTests.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ToObject<T>(this HttpContent content)
        {
            if (content is object && content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = await content.ReadAsStreamAsync();

                using var streamReader = new StreamReader(contentStream);
                using var jsonReader = new JsonTextReader(streamReader);

                JsonSerializer serializer = new();
                try
                {
                    return serializer.Deserialize<T>(jsonReader);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("HTTP Response was invalid and cannot be deserialised.");
            }

            return default;
        }
    }
}
