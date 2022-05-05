namespace Depot.Services
{
    using Depot.Enitities;
    using Newtonsoft.Json;

    public class ConfigService
    {
        public Config GetConfig()
        {
            if (File.Exists("config.json"))
                return JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json")) ?? new();
            return new();
        }
    }
}