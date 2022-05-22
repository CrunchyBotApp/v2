using Newtonsoft.Json;

namespace CrunchyBotNext.Bot
{
    public static class JsonConfigurationManager
    {
        public static async Task<string> GetRawJson() => await File.ReadAllTextAsync("crunchyconfig.json");

        public static async Task<Configuration?> GetConfiguration()
        {
            string json = await GetRawJson();
            return JsonConvert.DeserializeObject<Configuration>(json);
        }
    }
}
